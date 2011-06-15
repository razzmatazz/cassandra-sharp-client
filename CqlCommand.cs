using System;
using System.Data;

namespace Apache.Cassandra.Cql
{
	public class CqlCommand: IDbCommand
	{
		private string _CommandText;
		private CqlConnection _Connection;

		public CqlCommand()
		{

		}

		public CqlCommand(string commandText)
		{
			_CommandText = commandText;
		}

		public CqlCommand(string commandText, CqlConnection connection)
		{
			_CommandText = commandText;
			_Connection = connection;
		}

		private void EnsureWeHaveQueryAndConnection()
		{
			if (_CommandText == null)
				throw new CqlException("CommandText is not set on command");

			if (_Connection == null)
				throw new CqlException("connection is not set on command");
		}

		public void Cancel ()
		{
			// TODO: cancel is supported or not?
			// nop
		}

		public IDbDataParameter CreateParameter ()
		{
			throw new NotImplementedException ();
		}

		public int ExecuteNonQuery ()
		{
			throw new NotImplementedException ();
		}

		public IDataReader ExecuteReader()
		{
			EnsureWeHaveQueryAndConnection();

			return _Connection.ActualConnection.ExecuteQueryWithReader(_CommandText);
		}

		public IDataReader ExecuteReader (CommandBehavior behavior)
		{
			throw new NotImplementedException ();
		}

		public object ExecuteScalar ()
		{
			throw new NotImplementedException ();
		}

		public void Prepare ()
		{
			throw new NotImplementedException ();
		}

		public string CommandText {
			get {
				return _CommandText;
			}
			set {
				_CommandText = value;
			}
		}

		public int CommandTimeout {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public CommandType CommandType {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public IDbConnection Connection {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public IDataParameterCollection Parameters {
			get {
				throw new NotImplementedException ();
			}
		}

		public IDbTransaction Transaction {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public UpdateRowSource UpdatedRowSource {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}
	}
}

