using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apache.Cassandra.Cql.Internal.Marshal
{
	interface IMarshaller
	{
		byte[] Marshall(object obj);
		object Unmarshall(byte[] bytes);
		Type MarshalledType { get; }
		string CassandraTypeName { get; }
	}
}
