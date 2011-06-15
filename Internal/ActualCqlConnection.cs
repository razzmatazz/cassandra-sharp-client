using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.Cassandra.Cql;
using Thrift.Transport;
using Thrift.Protocol;
using System.Data;

namespace Apache.Cassandra.Cql.Internal
{
	class ActualCqlConnection: IDisposable
	{
		private CqlConnectionConfiguration _Config;
		public Apache.Cassandra.Cassandra.Client Client { get; private set; }
		private IKeyspaceMetadata _CurrentKeyspace;
		private Compression _Compression;
		private string _ActualKeyspaceSelected;

		public ActualCqlConnection(CqlConnectionConfiguration config)
		{
			_Config = config;
			_CurrentKeyspace = new NullKeyspaceMetadata();

			// TODO: Compression.NONE does not work!
			_Compression = Compression.NONE;
			_ActualKeyspaceSelected = "";
		}

		public void Connect()
		{
			if (Client != null)
				throw new InvalidOperationException("connection already open");

			TSocket socket;
			if (_Config.Timeout == 0)
			{
				socket = new TSocket(_Config.Host, _Config.Port);
			}
			else
			{
				socket = new TSocket(_Config.Host, _Config.Port, _Config.Timeout);
			}

			TTransport transport = new TFramedTransport(socket);

			TProtocol protocol = new TBinaryProtocol(transport);

			Client = new Cassandra.Client(protocol);
			Client.InputProtocol.Transport.Open();
			if (!Client.InputProtocol.Transport.Equals(Client.OutputProtocol.Transport))
				Client.OutputProtocol.Transport.Open();

			SetKeyspace(_Config.DefaultKeyspace);
		}

		public void SetKeyspace(string keyspace)
		{
			if (_ActualKeyspaceSelected != keyspace)
			{
				if (string.IsNullOrEmpty(keyspace))
				{
					_CurrentKeyspace = new NullKeyspaceMetadata();
				}
				else
				{
					_CurrentKeyspace = new KeyspaceMetadata(this, keyspace);
				}
			}
		}

		public void EnsureKeyspaceSelected()
		{
			if (_ActualKeyspaceSelected != _CurrentKeyspace.KeyspaceName)
			{
				// TODO: how to change to "null" keyspace?
				if (_CurrentKeyspace.KeyspaceName != "")
					ExecuteClientCommand(cl => cl.set_keyspace(_CurrentKeyspace.KeyspaceName));

				_ActualKeyspaceSelected = _CurrentKeyspace.KeyspaceName;
			}
		}

		public IDataReader ExecuteQueryWithReader(string commandText)
		{
			EnsureKeyspaceSelected();

			CqlResult result = ExecuteClientCommandWithResult(
				cl => cl.execute_cql_query(Util.CompressQueryString(commandText, _Compression), _Compression)
			);

			// resolve column family
			var cfName = new QueryAnalyzer(commandText).ColumnFamilyName;
			var cfMetadata = _CurrentKeyspace.GetColumnFamilyMetadata(cfName);
			if (cfMetadata == null)
				throw new CqlException("metadata for column family \"" + cfName + "\" was not found in the active keyspace (" + _CurrentKeyspace.KeyspaceName + ")");

			return new CqlDataReader(result, this, cfMetadata);
		}

		public void ExecuteClientCommand(Action<Cassandra.Client> actionOnClient)
		{
			try
			{
				actionOnClient(Client);
			}
			// TODO: check each exception
			catch (InvalidRequestException ire)
			{
				throw new CqlException(ire.Why, ire);
			}
			catch (Exception ex)
			{
				throw new CqlException(ex.Message, ex);
			}
		}

		public TResult ExecuteClientCommandWithResult<TResult>(Func<Cassandra.Client, TResult> actionOnClient)
		{
			try
			{
				return actionOnClient(Client);
			}
			// TODO: check each exception
			catch (InvalidRequestException ire)
			{
				throw new CqlException(ire.Why, ire);
			}
			catch (Exception ex)
			{
				throw new CqlException(ex.Message, ex);
			}
		}

		public void Disconnect()
		{
			if (Client == null)
				throw new InvalidOperationException("connection already disconnected");

			CloseConnection();
		}

		private void CloseConnection()
		{
			if (Client != null)
			{
				Client.InputProtocol.Transport.Close();
				Client.OutputProtocol.Transport.Close();

				Client = null;
				_CurrentKeyspace = new NullKeyspaceMetadata();
				_ActualKeyspaceSelected = "";
			}
		}

		public void Dispose()
		{
			CloseConnection();
		}
	}
}
