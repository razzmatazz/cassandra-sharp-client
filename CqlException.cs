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
	}
}

