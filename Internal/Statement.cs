using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace Apache.Cassandra.Cql.Internal
{
	class Statement
	{
		public enum QueryType {
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

		private static IDictionary<string, QueryType> _QueryTypeByCommand = new Dictionary<string, QueryType>()
		{
			{ "use", QueryType.Use },
			{ "select", QueryType.Select },
			{ "update", QueryType.Update },
			{ "delete", QueryType.Delete },
			{ "truncate", QueryType.Truncate },
			{ "create keyspace", QueryType.CreateKeyspace },
			{ "create columnfamily", QueryType.CreateColumnfamily },
			{ "create index", QueryType.CreateIndex },
			{ "begin batch", QueryType.BeginBatch },
			{ "drop", QueryType.Drop }
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
		public QueryType Type { get; private set; }
		public string ColumnFamilyName { get; private set; }
		public string Keyspace { get; private set; }
		private IEnumerable<CommandComponent> _Components;

		public bool IsSelectStatement
		{
			get	{ return Type == QueryType.Select; }
		}

		public bool IsNonQueryStatement
		{
			get { return !IsSelectStatement; }
		}

		public bool RequiresKeyspaceSelected
		{
			get
			{
				return Type != QueryType.Use && Type != QueryType.CreateKeyspace;
			}
		}

		public Statement(string queryStatement)
		{
			CommandText = queryStatement;
			_Components = ParseCommandText(CommandText).ToList();
			ResolveCommandProperties();
		}

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
				else if (c == '\"')
				{
					yield return EatQuotedString(queue, '\"');
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
					yield return EatTextWhile(queue, p => !"\"'@".Contains(p), CommandComponent.ComponentType.Text);
				}
			}
		}

		private CommandComponent EatQuotedString(Queue<char> queue, char quoteChar)
		{
			// TODO: not sure what happens with ' inside "string" or with " inside ''
			var comp = new CommandComponent(CommandComponent.ComponentType.Text);
			comp.Text += queue.Dequeue();

			bool inEscape = false;
			while (queue.Count != 0)
			{
				char c = queue.Dequeue();
				comp.Text += c;

				if (inEscape)
				{
					inEscape = false;
				}
				else
				{
					if (c == '\\')
						inEscape = true;
					else if (c == quoteChar)
						return comp;
				}
			}

			throw new CqlException("command text contains unterminated strings");
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

			var paramsWithCqlValues = statementParams.ToDictionary(p => p, p => ((CqlParameter)cmdParamCollection[p]).MarshallToCqlParamValue());

			var cql = _Components.Select(
				c => c.Type == CommandComponent.ComponentType.Text
					? c.Text
					: paramsWithCqlValues[c.Text]
			).Aggregate("", (a, b) => a + b);

			return cql;
		}

		private static Regex ReCf = new Regex("(?:SELECT|DELETE)\\s+.+\\s+FROM\\s+(\\w+).*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		private static Regex ReUseCommand = new Regex("\\s*USE\\s+(\\w+)\\s*", RegexOptions.IgnoreCase | RegexOptions.Multiline);

		private void ResolveCommandProperties()
		{
			Type = ResolveType();

			if (Type == QueryType.Select || Type == QueryType.Delete)
			{
				var match = ReCf.Match(CommandText);
				if (!match.Success)
					throw new CqlException("cannot resolve column family in a select or delete query");

				ColumnFamilyName = match.Groups[1].ToString();
			}
			else if (Type == QueryType.Use)
			{
				var match = ReUseCommand.Match(CommandText);
				if (!match.Success)
					throw new CqlException("cannot resolve keyspace in USE command \"" + CommandText + "\"");

				Keyspace = match.Groups[1].ToString();
			}
		}

		private QueryType ResolveType()
		{
			string normalizedSpaces = string.Join(" ", CommandText.Split('\t', '\r', '\n', ' ')).ToLower();

			foreach (var cmdAndType in _QueryTypeByCommand)
			{
				if (normalizedSpaces.StartsWith(cmdAndType.Key + " "))
					return cmdAndType.Value;
			}

			throw new CqlException("cannot determine statement type for the query (looking for one of: "
				+ string.Join(",", _QueryTypeByCommand.Keys.ToArray()) + ")");
		}

	}
}
