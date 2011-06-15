using System;
using System.Data;
using Apache.Cassandra.Cql.Internal;

namespace Apache.Cassandra.Cql
{
	public class CqlConnection : IDbConnection
	{
		private ActualCqlConnection _ActualConnection;
		private ConnectionState _ConnectionState;
		private CqlConnectionConfiguration _Config;
		private string _CurrentKeyspace;
		private int _ConnectionTimeout; // TODO: not used for now

		internal ActualCqlConnection ActualConnection {
			get { return _ActualConnection; }
		}

		public CqlConnection()
		{
			_ConnectionState = ConnectionState.Closed;
			_Config = new CqlConnectionConfiguration();
			_CurrentKeyspace = "";
			_ConnectionTimeout = 0;
			_ActualConnection = null;
		}
		
		public IDbTransaction BeginTransaction()
		{
			EnsureConnected();
			throw new CqlException("transactions are not supported");
		}

		public IDbTransaction BeginTransaction (IsolationLevel il)
		{
			return BeginTransaction();
		}

		public void ChangeDatabase (string databaseName)
		{
			EnsureConnected();
			_CurrentKeyspace = CqlConnectionConfiguration.EnsureKeyspaceNameIsValid(databaseName);
		}

		public void Close ()
		{
			if (_ConnectionState == ConnectionState.Closed)
				return;

			// TODO: connection pool
			_ActualConnection.Disconnect();
			_ActualConnection = null;
		}

		public IDbCommand CreateCommand ()
		{
			EnsureConnected();
			return new CqlCommand() { Connection = this };
		}

		public void Open ()
		{
			// TODO: connection pool

			if (_ConnectionState != ConnectionState.Closed)
				throw new CqlException("already connected");

			_ActualConnection = new ActualCqlConnection(_Config);
			_ActualConnection.Connect();
			_ConnectionState = ConnectionState.Open;
		}

		public string ConnectionString {
			get {
				return _Config.ConnectionString;
			}
			set {
				EnsureNotConnected();
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
				EnsureConnected();
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

		private void EnsureConnected()
		{
			if (_ConnectionState == ConnectionState.Closed)
				throw new CqlException("connection is closed");
		}

		private void EnsureNotConnected()
		{
			if (_ConnectionState != ConnectionState.Closed)
				throw new CqlException("connection is open");
		}
	}
}

