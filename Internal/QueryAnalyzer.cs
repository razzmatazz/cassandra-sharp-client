using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Apache.Cassandra.Cql.Internal
{
	class QueryAnalyzer
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
			Drop
		}

		public string Query { get; private set; }
		public QueryType Type { get; private set; }
		public string ColumnFamilyName { get; private set; }

		private IDictionary<string, QueryType> _QueryTypeByCommand = new Dictionary<string, QueryType>()
		{
			{ "use", QueryType.Use },
			{ "select", QueryType.Select },
			{ "update", QueryType.Update },
			{ "delete", QueryType.Delete },
			{ "truncate", QueryType.Truncate },
			{ "create keyspace", QueryType.CreateKeyspace },
			{ "create columnfamily", QueryType.CreateColumnfamily },
			{ "create index", QueryType.CreateIndex },
			{ "drop", QueryType.Drop }
		};

		public QueryAnalyzer(string query)
		{
			Query = query;
			Type = ResolveType();
			ColumnFamilyName = ResolveColumnFamilyName();
		}

		private static Regex ReCf = new Regex("(?:SELECT|DELETE)\\s+.+\\s+FROM\\s+(\\w+).*", RegexOptions.IgnoreCase | RegexOptions.Multiline);

		private string ResolveColumnFamilyName()
		{
			// TODO: check for other types of queries too
			if (Type == QueryType.Select || Type == QueryType.Delete)
			{
				var match = ReCf.Match(Query);
				if (!match.Success)
					throw new CqlException("cannot resolve column family in a select or delete query");

				return match.Groups[1].ToString();
			}

			return null;
		}

		private QueryType ResolveType()
		{
			string normalizedSpaces = string.Join(" ", Query.Split('\t', '\r', '\n', ' ')).ToLower();

			foreach (var cmdAndType in _QueryTypeByCommand)
			{
				if (normalizedSpaces.StartsWith(cmdAndType.Key + " "))
					return cmdAndType.Value;
			}

			throw new CqlException("cannot determine command type for the query (looking for one of: "
				+ string.Join(",", _QueryTypeByCommand.Keys.ToArray()) + ")");
		}
	}
}
