using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apache.Cassandra.Cql.Internal
{
	class KeyspaceMetadata: IKeyspaceMetadata
	{
		private string _Name;
		private IDictionary<string, ColumnFamilyMetadata> _CF;
		private ActualCqlConnection _Connection;

		public KeyspaceMetadata(ActualCqlConnection connection, string name)
		{
			_Connection = connection;
			_Name = name;
			// TODO: validate keyspace name
		}

		private IDictionary<string, ColumnFamilyMetadata> FetchCfMetadata()
		{
			if (_CF == null)
			{
				KsDef ksDef = _Connection.ExecuteClientCommandWithResult(cl => cl.describe_keyspace(_Name));
				_CF = ksDef.Cf_defs.ToDictionary(cf => cf.Name, cf => new ColumnFamilyMetadata(cf));
			}

			return _CF;
		}

		public string KeyspaceName
		{
			get { return _Name; }
		}
		
		public ColumnFamilyMetadata GetColumnFamilyMetadata(string cfName)
		{
			ColumnFamilyMetadata md;
			if (FetchCfMetadata().TryGetValue(cfName, out md))
				return md;
			else
				return null;
		}
	}
}
