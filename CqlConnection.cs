using System;
using System.Data;
using Apache.Cassandra.Cql.Internal;
using System.Data.Common;

namespace Apache.Cassandra.Cql
{
	public class CqlConnection : DbConnection
	{
		private ActualCqlConnection _ActualConnection;
		private ConnectionState _ConnectionState;
		private CqlConnectionConfiguration _Config;
		private string _CurrentKeyspace;

		internal ActualCqlConnection ActualConnection {
			get { return _ActualConnection; }
		}

		public CqlConnection()
		{
			_ConnectionState = ConnectionState.Closed;
			_Config = new CqlConnectionConfiguration();
			_CurrentKeyspace = "";
			_ActualConnection = null;
		}
		
		protected override DbTransaction BeginDbTransaction(IsolationLevel il)
		{
			throw new CqlException("transactions are not supported");
		}

		public override void ChangeDatabase (string databaseName)
		{
			EnsureConnected();
			_CurrentKeyspace = CqlConnectionConfiguration.EnsureKeyspaceNameIsValid(databaseName);
		}

		public override void Close ()
		{
			if (_ConnectionState == ConnectionState.Closed)
				return;

			// TODO: connection pool
			// TODO: when in connection pool, do not disconnect, just return it to the pool!
			_ActualConnection.DisassociateFrom(this);
			_ActualConnection.Disconnect();
			_ActualConnection = null;
		}

		protected override DbCommand CreateDbCommand ()
		{
			EnsureConnected();
			return new CqlCommand() { Connection = this };
		}

		public override void Open ()
		{
			// TODO: connection pool

			if (_ConnectionState != ConnectionState.Closed)
				throw new CqlException("already connected");

			_ActualConnection = new ActualCqlConnection(_Config);
			_ActualConnection.Connect();
			_ActualConnection.AssociateWith(this);
			_ConnectionState = ConnectionState.Open;
		}

		public override string ConnectionString {
			get {
				return _Config.ConnectionString;
			}
			set {
				EnsureNotConnected();
				_Config = CqlConnectionConfiguration.FromConnectionString(value);
			}
		}

		public override string Database {
			get {
				EnsureConnected();
				return _CurrentKeyspace;
			}
		}

		public override ConnectionState State {
			get {
				return _ConnectionState;
			}
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

		public override string DataSource
		{
			get { return _Config.Host + ":" + _Config.Port; }
		}

		public override string ServerVersion
		{
			// TODO: get cassandra version from Cassandra.Client
			get { return "unknown"; }
		}
	}
}

