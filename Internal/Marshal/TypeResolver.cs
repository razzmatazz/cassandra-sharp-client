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
				Marshaller = new AsciiType()
			},
			new TypeDescription() {
				Type = CassandraType.UTF8,
				Marshaller = new Utf8Type()
			},
			new TypeDescription() {
				Type = CassandraType.Bytes,
				Marshaller = new BytesType()
			}
		};

		public static IMarshaller GetMarshalledTypeForObject(object value)
		{
			return TypeDescriptions.Where(td => td.Marshaller.MarshalledType == value.GetType()).Select(td => td.Marshaller).FirstOrDefault();
		}

		public static bool IsNativeTypeMarshalled(Type type)
		{
			return TypeDescriptions.Any(td => td.Marshaller.MarshalledType == type);
		}

		public static IMarshaller GetMarshalledTypeForName(string name)
		{
			return GetTypeDescriptionForName(name).Marshaller;
		}

		public static TypeDescription GetTypeDescriptionForName(string name)
		{
			return TypeDescriptions.Where(t => t.Marshaller.CassandraTypeName == name).First();
		}
	}
}
