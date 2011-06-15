using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apache.Cassandra.Cql.Internal.Marshal
{
	class AsciiTypeMarshaller: IMarshaller
	{
		public byte[] Marshall(object obj)
		{
			return Encoding.ASCII.GetBytes(Convert.ToString(obj));
		}

		public object Unmarshall(byte[] bytes)
		{
			return Encoding.ASCII.GetString(bytes);
		}


		public Type MarshalledType
		{
			get { return typeof(string); }
		}

		public string CassandraTypeName
		{
			get { return "org.apache.cassandra.db.marshal.AsciiType"; }
		}
	}
}
