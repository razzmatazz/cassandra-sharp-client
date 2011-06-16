using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Apache.Cassandra.Cql.Internal.Marshal;

namespace Apache.Cassandra.Cql
{
	public class CqlParameter: DbParameter
	{
		private string _Name;
		private object _Value;
		private DataRowVersion _SourceVersion = DataRowVersion.Default;
		private IMarshaller _MarshalledType;
		private CqlParameterCollection _Collection;

		public CqlParameter()
		{

		}

		public CqlParameter(string name, object value)
		{
			ParameterName = name;
			Value = value;
		}

		internal void BindToCollection(CqlParameterCollection collection)
		{
			if (_Collection != null)
				throw new InvalidOperationException("parameter is already bound to parameter collection");

			_Collection = collection;
		}

		internal string MarshallToCqlParamValue()
		{
			EnsureValid();
			return _MarshalledType.MarshallToCqlParamValue(_Value);
		}

		private void EnsureValid()
		{
			if (string.IsNullOrEmpty(_Name))
				throw new CqlException("parameter has no name set");

			if (_Value == null)
				throw new CqlException(string.Format("parameter {0} has no value set", _Name));
		}

		public override DbType DbType
		{
			get
			{
				return TypeResolver.GetMarshalledTypeForObject(_Value).DbType;
			}
			set
			{
				throw new NotSupportedException("CqlParameter.DbType cannot be set on cassandra parameters");
			}
		}

		public override ParameterDirection Direction
		{
			get
			{
				return ParameterDirection.Output;
			}
			set
			{
				if (value != ParameterDirection.Output)
					throw new NotSupportedException("CqlParameter.Direction can only be output");
			}
		}

		public override bool IsNullable
		{
			get
			{
				return false;
			}
			set
			{
				if (value != false)
					throw new NotSupportedException("CqlParameter.IsNullable cannot be true, cassandra cql client does not support nullable params");
			}
		}

		public override string ParameterName
		{
			get
			{
				return _Name;
			}
			set
			{
				// TODO: validate param name

				if (string.IsNullOrEmpty(value) || value.Length < 2 || value[0] != '@')
					throw new CqlException("parameter name must start with @ and not be an empty string");

				if (_Collection != null)
					_Collection.OnBoundParameterNameChange(this, _Name, value);

				_Name = value;
			}
		}

		public override void ResetDbType()
		{
			// nop: we don't allow changing DbType for param
		}

		public override int Size
		{
			get
			{
				return TypeResolver.GetMarshalledTypeForObject(_Value).Marshall(_Value).Length;
			}
			set
			{
				throw new NotSupportedException("cassandra cql client does not support changing size of CqlParameter value");
			}
		}

		public override string SourceColumn
		{
			get
			{
				throw new NotSupportedException("cassandra cql client does not support setting IDbParameter.SourceColumn");
			}
			set
			{
				throw new NotSupportedException("cassandra cql client does not support setting IDbParameter.SourceColumn");
			}
		}

		public override bool SourceColumnNullMapping
		{
			get
			{
				// TODO: is this right?
				return false;
			}
			set
			{
				throw new NotSupportedException("cassandra cql client does not support setting IDbParameter.SourceColumnNullMapping");
			}
		}

		public override DataRowVersion SourceVersion
		{
			get
			{
				return _SourceVersion;
			}
			set
			{
				_SourceVersion = value;
			}
		}

		public override object Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_MarshalledType = TypeResolver.GetMarshalledTypeForObject(value);
				if (_MarshalledType == null)
					throw new ArgumentException("cassandra cql client does not support parameter value type of " + value.GetType().Name);

				_Value = value;
			}
		}
	}
}
