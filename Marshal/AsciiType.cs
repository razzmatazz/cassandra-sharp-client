using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Apache.Cassandra.Cql.Marshal
{
	public class AsciiType : IMarshaller
	{
		public static string ToAscii(object obj)
		{
			// TODO limit string to ASCII chars
			return Convert.ToString(obj);
		}

		public object ToValue(object obj)
		{
			return ToAscii(obj);
		}

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
			//get { return "org.apache.cassandra.db.marshal.AsciiType"; }
			get { return "AsciiType"; }
		}

		public DbType DbType
		{
			get { return DbType.AnsiString; }
		}
	}
}
