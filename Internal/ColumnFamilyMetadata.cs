using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.Cassandra.Cql.Internal.Marshal;

namespace Apache.Cassandra.Cql.Internal
{
	class ColumnFamilyMetadata
	{
		public enum ColumnFamilyType {
			Standard,
			Super
		}

		private CfDef _Cf;
		public string Name { get; private set; }
		public string Comment { get; private set; }
		public ColumnFamilyType FamilyType { get; private set; }

		public TypeResolver.CassandraType KeyType { get; private set; }
		public IMarshaller KeyMarshaller { get; private set; }

		public TypeResolver.CassandraType ColumnNameType { get; private set; }
		public IMarshaller ColumnNameMarshaller { get; private set; }

		public TypeResolver.CassandraType ColumnValueType { get; private set; }
		public IMarshaller ColumnValueMarshaller { get; private set; }

		public ColumnFamilyMetadata(CfDef cf)
		{
			_Cf = cf;

			Name = cf.Name;
			Comment = cf.Comment;

			if (cf.Column_type == "Standard")
			{
				FamilyType = ColumnFamilyType.Standard;
			}
			else if (cf.Column_type == "Super")
			{
				FamilyType = ColumnFamilyType.Super;
			}
			else
			{
				throw new NotImplementedException("column family type \"" + cf.Column_type + "\" is unsupported");
			}

			var td = TypeResolver.GetTypeDescriptionForName(_Cf.Key_validation_class);
			KeyType = td.Type;
			KeyMarshaller = td.Marshaller;

			td = TypeResolver.GetTypeDescriptionForName(_Cf.Comparator_type);
			ColumnNameType = td.Type;
			ColumnNameMarshaller = td.Marshaller;

			td = TypeResolver.GetTypeDescriptionForName(_Cf.Default_validation_class);
			ColumnValueType = td.Type;
			ColumnValueMarshaller = td.Marshaller;
		}

	}
}
