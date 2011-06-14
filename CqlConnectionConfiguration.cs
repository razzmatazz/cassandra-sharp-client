using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Apache.Cassandra.Cql
{
	class CassandraConnectionConfiguration
	{
		public string Host { get; set; }
		public ushort Port { get; set; }
		public string DefaultKeyspace { get; set; }
		public string ConnectionString { get; set; }
		public int Timeout { get; set; }
		
		public CassandraConnectionConfiguration()
		{
			Host = "";
			Port = 7160;
			DefaultKeyspace = "";
			ConnectionString = "";
			Timeout = 0;
		}
		
		private void ParseEndpoints(DbConnectionStringBuilder builder)
		{
			object val;
			builder.TryGetValue("Source", out val);
			if (val == null)
				throw new CassandraException("no \"Source\" param specified in connection string");

			string source = val as string;
			if (source.Contains(":"))
			{
				var parts = source.Split(':');

				ushort port;
				if (!ushort.TryParse(parts[1], out port))
					throw new CassandraException("cannot parse Source param: port number is not parsable");

				Host = parts[0];
				Port = Port;
			}
			else
			{
				Host = source;
				Port = 7160;
			}
		}
		
		private void ParseDefaultKeyspace(DbConnectionStringBuilder builder)
		{
			object val;
			if (builder.TryGetValue("Keyspace", out val))
			{
				DefaultKeyspace = EnsureKeyspaceNameIsValid(val as string);
			}
			else if (builder.TryGetValue("Database", out val))
			{
				DefaultKeyspace = EnsureKeyspaceNameIsValid(val as string);
			}
		}
		
		public static string EnsureKeyspaceNameIsValid(string keyspace)
		{
			// TODO: a more elaborate checking is needed here
			if (keyspace == "")
				throw new CassandraException("keyspace name \"" + keyspace + "\" is not valid");
			
			return keyspace;
		}
		
		public static CassandraConnectionConfiguration FromConnectionString(string connString)
		{
			var builder = new DbConnectionStringBuilder();
			builder.ConnectionString = connString;
			
			var config = new CassandraConnectionConfiguration();
			config.ParseDefaultKeyspace(builder);
			config.ParseEndpoints(builder);
			
			return config;
		}
	}
}

