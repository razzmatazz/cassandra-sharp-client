using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Apache.Cassandra.Cql
{
	class CqlConnectionConfiguration
	{
		public readonly ushort DEFAULT_CASSANDRA_PORT = 9160;

		public string Host { get; set; }
		public ushort Port { get; set; }
		public string DefaultKeyspace { get; set; }
		public string ConnectionString
		{
			get
			{
				return string.Format("Source={0}:{1}{2}", Host, Port, DefaultKeyspace == "" ? "": ";Keyspace=" + DefaultKeyspace);
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public int Timeout { get; set; }
		
		public CqlConnectionConfiguration()
		{
			Host = "";
			Port = DEFAULT_CASSANDRA_PORT;
			DefaultKeyspace = "";
			Timeout = 0;
		}
		
		private void ParseEndpoints(DbConnectionStringBuilder builder)
		{
			object val;
			builder.TryGetValue("Source", out val);
			if (val == null)
				throw new CqlException("no \"Source\" param specified in connection string");

			string source = val as string;
			if (source.Contains(":"))
			{
				var parts = source.Split(':');

				ushort port;
				if (!ushort.TryParse(parts[1], out port))
					throw new CqlException("cannot parse Source param: port number is not parsable");

				Host = parts[0];
				Port = port;
			}
			else
			{
				Host = source;
				Port = DEFAULT_CASSANDRA_PORT;
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
				throw new CqlException("keyspace name \"" + keyspace + "\" is not valid");
			
			return keyspace;
		}
		
		public static CqlConnectionConfiguration FromConnectionString(string connString)
		{
			var builder = new DbConnectionStringBuilder();
			builder.ConnectionString = connString;

			var config = new CqlConnectionConfiguration();
			config.ParseDefaultKeyspace(builder);
			config.ParseEndpoints(builder);
			
			return config;
		}
	}
}

