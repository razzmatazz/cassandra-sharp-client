using System;
using System.Data;
using Apache.Cassandra.Cql.Internal;
using System.Data.Common;

namespace Apache.Cassandra.Cql
{
	public class CqlCommand: DbCommand
	{
		private CqlConnection _Connection;
		private Statement _Statement;
		private bool _DesignTimeVisible;
		private CqlParameterCollection _DbParams;

		public CqlCommand()
		{
			_DesignTimeVisible = true;
			_DbParams = new CqlParameterCollection();
		}

		public CqlCommand(CqlConnection connection)
			: this()
		{
			_Connection = connection;
		}

		public CqlCommand(string commandText)
			: this()
		{
			_Statement = new Statement(commandText);
		}

		public CqlCommand(string commandText, CqlConnection connection)
			: this(commandText)
		{
			_Connection = connection;
		}

		public override void Cancel()
		{
			throw new NotSupportedException("Cassandra client does not support canceling on a query");
		}

		public override string CommandText
		{
			get
			{
				return _Statement.CommandText;
			}
			set
			{
				_Statement = new Statement(value);
			}
		}

		public override int CommandTimeout
		{
			// TODO: what about CommandTimeout
			get 
			{ 
				throw new NotImplementedException(); 
			}
			  set 
			{ 
				throw new NotImplementedException(); 
			}
		}


		public override CommandType CommandType
		{
			get
			{
				return CommandType.Text;
			}
			set
			{
				if (value != CommandType.Text)
					throw new CqlException("CqlCommmand.CommandType can only be CommandType.Text");
			}
		}

		protected override DbParameter CreateDbParameter()
		{
			// TODO: implement CreateDbParameter
 			throw new NotImplementedException();
		}

		protected override DbParameterCollection  DbParameterCollection
		{
			get { return _DbParams; }
		}

		public override bool DesignTimeVisible
		{
			get { return _DesignTimeVisible; }
			set { _DesignTimeVisible = value; }
		}

		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
		{
			EnsureWeHaveCommandTextAndConnection();
			return _Connection.ActualConnection.ExecuteStatementWithReader(_Connection, _Statement, behavior, _DbParams);
		}

		public override int  ExecuteNonQuery()
		{
			EnsureWeHaveCommandTextAndConnection();
			return _Connection.ActualConnection.ExecuteNonQueryStatement(_Connection, _Statement, _DbParams);
		}

		public override object  ExecuteScalar()
		{
			EnsureWeHaveCommandTextAndConnection();

			using (var reader = _Connection.ActualConnection.ExecuteStatementWithReader(_Connection, _Statement, CommandBehavior.SingleRow, _DbParams))
			{
				if (!reader.NextResult())
					throw new CqlException("no records returned for the query to extract scalar from");

				if (reader.FieldCount == 0)
					throw new CqlException("first row from results of the query has no columns returned");

				return reader[0];
			}
		}

		public override void  Prepare()
		{
 			throw new NotImplementedException();
		}

		public override UpdateRowSource  UpdatedRowSource
		{
			// TODO: check what is UpdatedRowSource actually
			  get 
			{ 
				throw new NotImplementedException(); 
			}
			  set 
			{ 
				throw new NotImplementedException(); 
			}
		}

		private void EnsureWeHaveCommandTextAndConnection()
		{
			if (_Statement == null)
				throw new CqlException("CommandText is not set on CqlCommand");

			if (_Connection == null || _Connection.State == ConnectionState.Closed)
				throw new InvalidOperationException("connection is not set on CqlCommand");
		}

		protected override DbConnection DbConnection
		{
			get
			{
				return _Connection;
			}
			set
			{
				_Connection = (CqlConnection)value;
			}
		}

		protected override DbTransaction DbTransaction
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException("cassandra cql client does not support exceptions");
			}
		}
	}

}

