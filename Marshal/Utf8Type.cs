using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace Apache.Cassandra.Cql.Marshal
{
	public class Utf8Type : IMarshaller
	{
		public static string ToUtf8(object obj)
		{
			return Convert.ToString(obj);
		}

		public object ToValue(object obj)
		{
			return ToUtf8(obj);
		}

		public byte[] Marshall(object obj)
		{
			return Encoding.UTF8.GetBytes(Convert.ToString(obj));
		}

		public object Unmarshall(byte[] bytes)
		{
			if (bytes == null)
				return null;

			return Encoding.UTF8.GetString(bytes);
		}

		public Type MarshalledType
		{
			get { return typeof(string); }
		}

		public string CassandraTypeName
		{
			//get { return "org.apache.cassandra.db.marshal.UTF8Type"; }
			get { return "UTF8Type"; }
		}

		public DbType DbType
		{
			get { return DbType.String; }
		}
	}
}
