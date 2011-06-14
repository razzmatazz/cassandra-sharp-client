using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Apache.Cassandra.Cql
{
	class CassandraConnectionConfiguration
	{
		public IList<string> EndPoints { get; set; }
		public string DefaultKeyspace { get; set; }
		public string ConnectionString { get; set; }
		
		public CassandraConnectionConfiguration()
		{
			EndPoints = new List<string>();
			DefaultKeyspace = "";
			ConnectionString = "";
		}
		
		private void ParseEndpoints(DbConnectionStringBuilder builder)
		{
			object val;
			builder.TryGetValue("Source", out val);
			
			string endpointString = (val as string) ?? "";
			string[] endpointStringList = endpointString.Split(',');
			
			var endpointList = new List<string>();
			foreach(var str in endpointStringList)
			{
				var trimmedStr = str.Trim();
				if (trimmedStr != "")
					endpointList.Add(trimmedStr);
			}
			
			if (endpointList.Count == 0)
				throw new CassandraException("no endpoints set in the \"Source\" param for connection or the param is missing altogether");
		}
		
		private void ParseDefaultKeyspace(DbConnectionStringBuilder builder)
		{
			object val;
			if (builder.TryGetValue("Initial Catalog", out val))
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

