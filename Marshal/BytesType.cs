using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Apache.Cassandra.Cql.Marshal
{
	public class BytesType: IMarshaller
	{
		public static byte[] ToBytes(object obj)
		{
			if (obj is byte[])
				return (byte[])obj;
			else
				return Encoding.UTF8.GetBytes(Convert.ToString(obj));		
		}

		public object ToValue(object obj)
		{
			return ToBytes(obj);
		}

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

		public DbType DbType
		{
			get { return DbType.Binary; }
		}
	}
}
