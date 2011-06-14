using System;
using System.Data.Common;

namespace Apache.Cassandra.Cql
{
	public class CassandraException: DbException
	{
		public CassandraException (string msg)
			: base(msg)
		{
		}
	}
}

