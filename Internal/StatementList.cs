using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace Apache.Cassandra.Cql.Internal
{
	class StatementList
	{
		public enum StatementType {
			Use,
			Select,
			Update,
			Delete,
			Truncate,
			CreateKeyspace,
			CreateColumnfamily,
			CreateIndex,
			BeginBatch,
			Drop
		}

		private static IDictionary<string, StatementType> _QueryTypeByCommand = new Dictionary<string, StatementType>()
		{
			{ "use", StatementType.Use },
			{ "select", StatementType.Select },
			{ "update", StatementType.Update },
			{ "delete", StatementType.Delete },
			{ "truncate", StatementType.Truncate },
			{ "create keyspace", StatementType.CreateKeyspace },
			{ "create columnfamily", StatementType.CreateColumnfamily },
			{ "create index", StatementType.CreateIndex },
			{ "begin batch", StatementType.BeginBatch },
			{ "drop", StatementType.Drop }
		};

		private class CommandComponent
		{
			public enum ComponentType
			{
				Text,
				Parameter
			}

			public ComponentType Type { get; private set; }
			public string Text { get; set; }

			public CommandComponent(ComponentType type)
			{
				Type = type;
				Text = "";
			}
		}

		public string CommandText { get; private set; }

		private IEnumerable<CommandComponent> _Components;

		public StatementList(string queryStatement)
		{
			CommandText = queryStatement;
			_Components = ParseCommandText(CommandText).ToList();

			// TODO: set Could change keyspace
			CouldChangeKeyspace = false;
		}

		public bool CouldChangeKeyspace { get; private set; }

		private IEnumerable<CommandComponent> ParseCommandText(string text)
		{
			var realComponents = ParseCommandTextReal(text).GetEnumerator();
			if (realComponents.MoveNext())
			{
				var lastComp = realComponents.Current;
				while (realComponents.MoveNext())
				{
					if (realComponents.Current.Type == CommandComponent.ComponentType.Text
							&& lastComp.Type == CommandComponent.ComponentType.Text)
					{
						lastComp.Text += realComponents.Current.Text;
					}
					else
					{
						yield return lastComp;
						lastComp = realComponents.Current;
					}
				}

				yield return lastComp;
			}
		}

		private IEnumerable<CommandComponent> ParseCommandTextReal(string commandText)
		{
			var queue = new Queue<char>(commandText);

			while (queue.Count != 0)
			{
				char c = queue.Peek();
				if (c == '\'')
				{
					yield return EatQuotedString(queue, '\'');
				}
				else if (c == '@')
				{
					queue.Dequeue();	// skip leading '@'
					var paramComponent = EatTextWhile(queue, p => char.IsLetterOrDigit(p), CommandComponent.ComponentType.Parameter);
					paramComponent.Text = "@" + paramComponent.Text;
					if (paramComponent.Text == "")
						throw new CqlException("command text contains empty parameter name (\"@\"): " + CommandText);

					yield return paramComponent;
				}
				else
				{
					yield return EatTextWhile(queue, p => p != '\'' && p != '@', CommandComponent.ComponentType.Text);
				}
			}
		}

		private CommandComponent EatQuotedString(Queue<char> queue, char quoteChar)
		{
			var comp = new CommandComponent(CommandComponent.ComponentType.Text);
			
			comp.Text += queue.Dequeue();	// add first ' char

			bool inEscape = false;
			while (queue.Count != 0)
			{
				char c = queue.Peek();
				if (c == '\'')
				{
					comp.Text += queue.Dequeue();
					inEscape = !inEscape;
				}
				else
				{
					if (inEscape)
						return comp;

					comp.Text += queue.Dequeue();
				}
			}		

			if (!inEscape)
				throw new CqlException("command text contains unterminated strings");

			return comp;
		}

		private CommandComponent EatTextWhile(Queue<char> queue, Func<char, bool> predicate, CommandComponent.ComponentType type)
		{
			var comp = new CommandComponent(type);

			while (queue.Count != 0 && predicate(queue.Peek()))
				comp.Text += queue.Dequeue();

			return comp;
		}

		private CommandComponent EatParameterName(Queue<char> queue)
		{
			var comp = new CommandComponent(CommandComponent.ComponentType.Parameter);

			while (queue.Count != 0 && char.IsLetterOrDigit(queue.Peek()))
				comp.Text += queue.Dequeue();

			return comp;
		}

		public string MakeCqlToExecute(CqlParameterCollection cmdParamCollection) 
		{
			cmdParamCollection.EnsureParameterCollectionIsComplete();

			// setup parameter->value dictionary
			var statementParams = _Components.Where(c => c.Type == CommandComponent.ComponentType.Parameter)
				.ToLookup(c => c.Text)
				.Select(c => c.Key);

			var firstParamWoValue = statementParams.FirstOrDefault(sp => !cmdParamCollection.Contains(sp));
			if (firstParamWoValue != null)
				throw new CqlException(string.Format("parameter {0} is not set for command text: {1}", firstParamWoValue, CommandText));

			var paramsWithCqlValues = statementParams.ToDictionary(p => p, p => ((CqlParameter)cmdParamCollection[p]).AsCqlString() );

			var cql = _Components.Select(
				c => c.Type == CommandComponent.ComponentType.Text
					? c.Text
					: paramsWithCqlValues[c.Text]
			).Aggregate("", (a, b) => a + b);

			return cql;
		}

	}
}
