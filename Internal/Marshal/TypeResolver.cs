using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apache.Cassandra.Cql.Internal.Marshal
{
	static class TypeResolver
	{
		public enum CassandraType
		{
			Ascii,
			Bytes,
			Composite,
			CounterColumn,
			DynamicComposite,
			Integer,
			LexicalUUID,
			LocalByPartitioner,
			Long,
			TimeUUID,
			UTF8,
			UUID
		}

		public class TypeDescription
		{
			public CassandraType Type { get; set; }
			public IMarshaller Marshaller { get; set; }
		}

		private static TypeDescription[] TypeDescriptions = {
			new TypeDescription() {
				Type = CassandraType.Ascii,
				Marshaller = new AsciiTypeMarshaller()
			},
			new TypeDescription() {
				Type = CassandraType.UTF8,
				Marshaller = new Utf8TypeMarshaller()
			},
			new TypeDescription() {
				Type = CassandraType.Bytes,
				Marshaller = new BytesType()
			}
		};

		public static IMarshaller GetMarshallerForName(string name)
		{
			return GetTypeDescriptionForName(name).Marshaller;
		}

		public static TypeDescription GetTypeDescriptionForName(string name)
		{
			return TypeDescriptions.Where(t => t.Marshaller.CassandraTypeName == name).First();
		}
	}
}
