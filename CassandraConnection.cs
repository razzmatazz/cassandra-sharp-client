using System;
using System.Data;

namespace Apache.Cassandra.Client
{
	public class CqlConnection: IDbConnection
	{
		private ConnectionState _ConnectionState;
		private CqlConnectionConfiguration _Config;
		private string _CurrentKeyspace;
		private int _ConnectionTimeout; // TODO: not used for now
		
		public CqlConnection()
		{
			_ConnectionState = ConnectionState.Closed;
			_Config = new CqlConnectionConfiguration();
			_CurrentKeyspace = "";
			_ConnectionTimeout = 0;
		}
		
		public IDbTransaction BeginTransaction ()
		{
			throw new CqlException("transactions are not supported");
		}

		public IDbTransaction BeginTransaction (IsolationLevel il)
		{
			return BeginTransaction();
		}

		public void ChangeDatabase (string databaseName)
		{
			if (_ConnectionState == ConnectionState.Closed)
				throw new CqlException("connection is closed");
			
			_CurrentKeyspace = CqlConnectionConfiguration.EnsureKeyspaceNameIsValid(databaseName);
		}

		public void Close ()
		{
			// TODO: close connection
		}

		public IDbCommand CreateCommand ()
		{
			// TODO: create command
			throw new NotImplementedException ();
		}

		public void Open ()
		{
			// TODO: open connection
			throw new NotImplementedException ();
		}

		public string ConnectionString {
			get {
				return _Config.ConnectionString;
			}
			set {
				if (_ConnectionState != ConnectionState.Closed)
					throw new CqlException("cannot set ConnectionString while connection is not closed");
				
				_Config = CqlConnectionConfiguration.FromConnectionString(value);
			}
		}

		public int ConnectionTimeout {
			get {
				return _ConnectionTimeout;
			}
		}

		public string Database {
			get {
				if (_ConnectionState == ConnectionState.Closed)
					throw new CqlException("connection is closed");
				
				return _CurrentKeyspace;
			}
		}

		public ConnectionState State {
			get {
				return _ConnectionState;
			}
		}

		public void Dispose ()
		{
			Close();
		}
	}
}

