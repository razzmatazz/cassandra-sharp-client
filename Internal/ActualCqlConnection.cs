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
		private KeyspaceMetadata _CurrentKeyspace;
		private Compression _Compression;
		private CqlConnection _AssociatedConnection;

		public ActualCqlConnection(CqlConnectionConfiguration config)
		{
			_Config = config;
			_CurrentKeyspace = null;

			// TODO: Compression.GZIP does not work!
			_Compression = Compression.NONE;
		}

		public void AssociateWith(CqlConnection connection)
		{
			if (_AssociatedConnection != null)
				throw new InvalidOperationException("already associated with an existing CqlConnection");

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

			SetKeyspace(_Config.DefaultKeyspace);
		}

		public void SetKeyspace(string keyspace)
		{
			if (string.IsNullOrEmpty(keyspace))
			{
				_CurrentKeyspace = null;
			}
			else
			{
				_CurrentKeyspace = new KeyspaceMetadata(this, keyspace);
				ExecuteClientCommand(cl => cl.set_keyspace(_CurrentKeyspace.KeyspaceName));
			}
		}

		public void EnsureKeyspaceSelectedFor(Statement st)
		{
			if (st.RequiresKeyspaceSelected && _CurrentKeyspace == null)
				throw new CqlException("no keyspace is selected for connection");
		}

		public CqlDataReader ExecuteStatementWithReader(CqlConnection connection, Statement st, CommandBehavior cmdBehaviour, CqlParameterCollection stParams)
		{
			EnsureAssociatedWith(connection);
			EnsureKeyspaceSelectedFor(st);

			// TODO: what about CommandBehaviour.SingleRow ? should we support this, throw an exception or nop?

			if ((cmdBehaviour & CommandBehavior.KeyInfo) == CommandBehavior.KeyInfo)
				throw new CqlException("CommandBehavior.KeyInfo is not supported by cassandra client");

			if (!st.IsSelectStatement)
				throw new CqlException("IDataReader can only be executed on queries of the SELECT type");

			var cfMetadata = _CurrentKeyspace.GetColumnFamilyMetadata(st.ColumnFamilyName);

			// CommandBehavior.SchemaOnly means the user only wants CqlCommand.GetSchemaTable(), don't execute the actual query
			if ((cmdBehaviour & CommandBehavior.SchemaOnly) == CommandBehavior.SchemaOnly)
				return new CqlDataReader(null, _AssociatedConnection, cfMetadata, cmdBehaviour);
			
			// TODO: support the CommandBehaviour.SingleRow option when querying
			CqlResult result = ExecuteClientCommandWithResult(
				cl => cl.execute_cql_query(Util.CompressQueryString(st.MakeCqlToExecute(stParams), _Compression), _Compression)
			);

			// resolve column family
			if (cfMetadata == null)
			{
				throw new CqlException(string.Format(
					"metadata for column family \"{0}\" was not found in the active keyspace (\"{1}\")",
					st.ColumnFamilyName,
					_CurrentKeyspace.KeyspaceName
				));
			}

			return new CqlDataReader(result, _AssociatedConnection, cfMetadata, cmdBehaviour);
		}

		
		public int ExecuteNonQueryStatement(CqlConnection connection, Statement st, CqlParameterCollection stParams)
		{
			EnsureAssociatedWith(connection);
			EnsureKeyspaceSelectedFor(st);

			if (!st.IsNonQueryStatement)
				throw new InvalidOperationException("CommandText is not a non-query cassandra statement");

			// treat the USE query statement specially
			if (st.Type == Statement.QueryType.Use)
			{
				SetKeyspace(st.Keyspace);
				return 0;
			}
			else
			{
				CqlResult result = ExecuteClientCommandWithResult(
					cl => cl.execute_cql_query(Util.CompressQueryString(st.MakeCqlToExecute(stParams), _Compression), _Compression)
				);

				if (!result.__isset.type)
					throw new CqlException("query results from cassandra has no type set");

				if (result.Type != CqlResultType.INT && result.Type != CqlResultType.VOID)
					throw new InvalidOperationException("non-query statement should've returned INT or VOID");

				return result.Type == CqlResultType.INT ? result.Num : 0;
			}
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
				_CurrentKeyspace = null;
			}
		}

		public void Dispose()
		{
			CloseConnection();
		}

	}
}
