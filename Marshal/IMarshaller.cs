using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Apache.Cassandra.Cql.Marshal
{
	public interface IMarshaller
	{
		object ToValue(object obj);
		byte[] Marshall(object obj);
		object Unmarshall(byte[] bytes);
		Type MarshalledType { get; }
		DbType DbType { get; }
		string CassandraTypeName { get; }
	}
}
