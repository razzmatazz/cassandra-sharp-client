using System;
using System.Data;
using Thrift.Protocol;
using Thrift.Transport;

namespace Apache.Cassandra.Cql
{
	public class CassandraConnection: IDbConnection
	{
		private Apache.Cassandra.Cassandra.Client _Client;

		private ConnectionState _ConnectionState;
		private CassandraConnectionConfiguration _Config;
		private string _CurrentKeyspace;
		private int _ConnectionTimeout; // TODO: not used for now

		public CassandraConnection()
		{
			_ConnectionState = ConnectionState.Closed;
			_Config = new CassandraConnectionConfiguration();
			_CurrentKeyspace = "";
			_ConnectionTimeout = 0;
		}
		
		public IDbTransaction BeginTransaction()
		{
			EnsureConnected();
			throw new CassandraException("transactions are not supported");
		}

		public IDbTransaction BeginTransaction (IsolationLevel il)
		{
			return BeginTransaction();
		}

		public void ChangeDatabase (string databaseName)
		{
			EnsureConnected();
			_CurrentKeyspace = CassandraConnectionConfiguration.EnsureKeyspaceNameIsValid(databaseName);
		}

		public void Close ()
		{
			// TODO: close connection
		}

		public IDbCommand CreateCommand ()
		{
			EnsureConnected();
			return new CassandraCommand(this);
		}

		public void Open ()
		{
			if (_ConnectionState != ConnectionState.Closed)
				throw new CassandraException("already connected");

			TTransport transport = null;
            if (_Config.Timeout == 0)
            {
                transport = new TSocket(_Config.Host, _Config.Port);
            }
            else
            {
                transport = new TSocket(_Config.Host, _Config.Port, _Config.Timeout);
            }
            TProtocol protocol = new TBinaryProtocol(transport);
            _Client = new Cassandra.Client(protocol);

			// TODO
		}

		public string ConnectionString {
			get {
				return _Config.ConnectionString;
			}
			set {
				EnsureNotConnected();
				_Config = CassandraConnectionConfiguration.FromConnectionString(value);
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
				throw new CassandraException("connection is closed");
		}

		private void EnsureNotConnected()
		{
			if (_ConnectionState != ConnectionState.Closed)
				throw new CassandraException("connection is open");
		}
	}
}

