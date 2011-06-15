using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apache.Cassandra.Cql.Internal.Marshal
{
	class Utf8TypeMarshaller: IMarshaller
	{
		public byte[] Marshall(object obj)
		{
			return Encoding.UTF8.GetBytes(Convert.ToString(obj));
		}

		public object Unmarshall(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}

		public Type MarshalledType
		{
			get { return typeof(string); }
		}

		public string CassandraTypeName
		{
			get { return "org.apache.cassandra.db.marshal.UTF8Type"; }
		}
	}
}
