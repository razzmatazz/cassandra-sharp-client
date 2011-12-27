using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apache.Cassandra.Cql.Marshal
{
	public class LongMarshaller: IMarshaller
	{
		public static long FromBytes(byte[] bytes)
		{
			return (long)new LongMarshaller().Unmarshall(bytes);
		}

		public static byte[] ToBytes(long value)
		{
			return new LongMarshaller().Marshall(value);
		}

		public object ToValue(object obj)
		{
			throw new NotImplementedException();
		}

		public byte[] Marshall(object obj)
		{
			long l = (long)obj;

			Func<long, int, byte> nthByte = (ln, i) => (byte)((ln >> i * 8) & 0xff);

			return Enumerable.Range(0, 8).Select(i => nthByte(l, i)).Reverse().ToArray();
		}

		public object Unmarshall(byte[] bytes)
		{
			long l = 0;

			foreach(byte b in bytes)
			{
				l <<= 8;
				l += b;
			}

			return l;
		}

		public Type MarshalledType
		{
			get { throw new NotImplementedException(); }
		}

		public System.Data.DbType DbType
		{
			get { throw new NotImplementedException(); }
		}

		public string CassandraTypeName
		{
			get { return "LongType"; }
		}
	}
}
