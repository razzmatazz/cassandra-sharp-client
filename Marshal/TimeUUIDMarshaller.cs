using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Apache.Cassandra.Cql.Marshal.Helpers;

namespace Apache.Cassandra.Cql.Marshal
{
	public class TimeUUIDMarshaller : IMarshaller
	{
		public object ToValue(object obj)
		{
			throw new NotImplementedException();
		}

		public byte[] Marshall(object obj)
		{
            // TODO: this is probably wrong - see Unmarshall and use hex representation - bytearray format is somewhat strange
			return ((Guid)obj).ToByteArray();
		}

		public object Unmarshall(byte[] bytes)
		{
            var obj =  new Guid(BitConverter.ToString(bytes).Replace("-", string.Empty));
            return obj;
		}

		public Type MarshalledType
		{
			get { return typeof(Guid); }
		}

		public DbType DbType
		{
			get { return DbType.Guid; }
		}

		public string CassandraTypeName
		{
			//get { return "org.apache.cassandra.db.marshal.TimeUUIDType"; }
			get { return "TimeUUIDType"; }
		}

	}
}
