using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.Cassandra.Cql;

namespace Apache.Cassandra.Cql.Internal
{
	class NullKeyspaceMetadata: IKeyspaceMetadata
	{
		public string KeyspaceName
		{
			get { return ""; }
		}


		public ColumnFamilyMetadata GetColumnFamilyMetadata(string cfName)
		{
			return null;
		}
	}
}
