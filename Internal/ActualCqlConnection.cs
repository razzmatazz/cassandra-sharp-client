using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.Cassandra.Cql;
using Thrift.Transport;
using Thrift.Protocol;
using System.Data;
using System.Data.Common;

namespace Apache.Cassandra.Cql.Internal
{
	class ActualCqlConnection: IDisposable
	{
		private CqlConnectionConfiguration _Config;
		private Apache.Cassandra.Cassandra.Client _Client;
		private Compression _Compression;
		private CqlConnection _AssociatedConnection;
		private bool _KeyspaceMightBeOutOfSync = false;

		public ActualCqlConnection(CqlConnectionConfiguration config)
		{
			_Config = config;

			// TODO: Compression.GZIP does not work!
			_Compression = Compression.NONE;
		}

		public void AssociateWith(CqlConnection connection)
		{
			if (_AssociatedConnection != null)
				throw new InvalidOperationException("already associated with an existing CqlConnection");

			// make sure we are on correct keyspace
			if (_KeyspaceMightBeOutOfSync)
			{
				if (!string.IsNullOrEmpty(_Config.DefaultKeyspace))
				{
					ExecuteClientCommand(cl => cl.set_keyspace(_Config.DefaultKeyspace));
				}
			}

			_AssociatedConnection = connection;
		}

		public void DisassociateFrom(CqlConnection connection)
		{
			if (_AssociatedConnection != connection)
				throw new InvalidOperationException("not associated with the given CqlConnection");

			_AssociatedConnection = null;
		}

		public void Connect()
		{
			if (_Client != null)
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

			_Client = new Cassandra.Client(protocol);
			_Client.InputProtocol.Transport.Open();
			if (!_Client.InputProtocol.Transport.Equals(_Client.OutputProtocol.Transport))
				_Client.OutputProtocol.Transport.Open();
		}

		public CqlDataReader ExecuteStatementWithReader(CqlConnection connection, StatementList st, CommandBehavior cmdBehaviour, CqlParameterCollection stParams)
		{
			EnsureAssociatedWith(connection);

			// TODO: what about CommandBehaviour.SingleRow ? should we support this, throw an exception or nop?

			if ((cmdBehaviour & CommandBehavior.KeyInfo) == CommandBehavior.KeyInfo)
				throw new CqlException("CommandBehavior.KeyInfo is not supported by cassandra client");

			// CommandBehavior.SchemaOnly means the user only wants CqlCommand.GetSchemaTable(), don't execute the actual query
			if ((cmdBehaviour & CommandBehavior.SchemaOnly) == CommandBehavior.SchemaOnly)
				return new CqlDataReader(null, _AssociatedConnection, cmdBehaviour);
			
			// TODO: support the CommandBehaviour.SingleRow option when querying
			_KeyspaceMightBeOutOfSync |= st.CouldChangeKeyspace;
			CqlResult result = ExecuteClientCommandWithResult(
				cl => cl.execute_cql_query(Util.CompressQueryString(st.MakeCqlToExecute(stParams), _Compression), _Compression)
			);

			if (result.__isset.type && result.Type != CqlResultType.ROWS)
				throw new CqlException("Query string has returned non-ROWS result set and ROWS were expected for CqlDataReader");

			return new CqlDataReader(result, _AssociatedConnection, cmdBehaviour);
		}

		public int ExecuteNonQueryStatements(CqlConnection connection, StatementList st, CqlParameterCollection stParams)
		{
			EnsureAssociatedWith(connection);

			_KeyspaceMightBeOutOfSync |= st.CouldChangeKeyspace;
			CqlResult result = ExecuteClientCommandWithResult(
				cl => cl.execute_cql_query(Util.CompressQueryString(st.MakeCqlToExecute(stParams), _Compression), _Compression)
			);

			if (!result.__isset.type)
				throw new CqlException("query result from cassandra has no type set");

			return result.Type == CqlResultType.INT ? result.Num : 0;
		}

		private void EnsureAssociatedWith(CqlConnection connection)
		{
			if (connection != _AssociatedConnection)
				throw new InvalidOperationException("not associated with the given CqlConnection");
		}

		public void ExecuteClientCommand(Action<Cassandra.Client> actionOnClient)
		{
			ExecuteClientCommandWithResult(cl => { actionOnClient(cl); return 0; });
		}

		public TResult ExecuteClientCommandWithResult<TResult>(Func<Cassandra.Client, TResult> actionOnClient)
		{
			if (_Client == null)
				throw new InvalidOperationException("not connected to cassandra instance");

			try
			{
				return actionOnClient(_Client);
			}
			// TODO: wrap each cassandra/thrift exception
			catch (InvalidRequestException ire)
			{
				throw new CqlException(ire.Why, ire);
			}
			catch (DbException ex)
			{
				throw ex;	// throw DbException's as is
			}
			catch (Exception ex)
			{
				throw new CqlException(ex.Message, ex);
			}
		}

		public void Disconnect()
		{
			if (_Client == null)
				throw new InvalidOperationException("connection already disconnected");

			CloseConnection();
		}

		private void CloseConnection()
		{
			if (_Client != null)
			{
				_Client.InputProtocol.Transport.Close();
				_Client.OutputProtocol.Transport.Close();

				_Client = null;
			}
		}

		public void Dispose()
		{
			CloseConnection();
		}

	}
}
