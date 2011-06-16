using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Apache.Cassandra.Cql.Internal.Marshal
{
	interface IMarshaller
	{
		byte[] Marshall(object obj);
		string MarshallToCqlParamValue(object obj);
		object Unmarshall(byte[] bytes);
		Type MarshalledType { get; }
		DbType DbType { get; }
		string CassandraTypeName { get; }
	}
}
