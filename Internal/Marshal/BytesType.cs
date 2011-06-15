using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apache.Cassandra.Cql.Internal.Marshal
{
	class BytesType: IMarshaller
	{
		public byte[] Marshall(object obj)
		{
			return (byte[])obj;
		}

		public object Unmarshall(byte[] bytes)
		{
			return bytes;
		}

		public Type MarshalledType
		{
			get { return typeof(byte[]); }
		}

		public string CassandraTypeName
		{
			get { return "org.apache.cassandra.db.marshal.BytesType"; }
		}
	}
}
