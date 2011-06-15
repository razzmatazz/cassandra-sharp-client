using System;
using System.Data.Common;

namespace Apache.Cassandra.Cql
{
	public class CqlException : DbException
	{
		public CqlException (string msg)
			: base(msg)
		{
		}

		public CqlException(string msg, Exception inner)
			: base(msg, inner)
		{
		}
	}
}

