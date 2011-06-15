using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apache.Cassandra.Cql.Internal
{
	interface IKeyspaceMetadata
	{
		string KeyspaceName { get; }
		ColumnFamilyMetadata GetColumnFamilyMetadata(string cfName);
	}
}
