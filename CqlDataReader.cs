using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Apache.Cassandra.Cql.Internal;
using Apache.Cassandra.Cql.Marshal;
using System.Data.Common;

namespace Apache.Cassandra.Cql
{
	/// <summary>
	/// Provides IDbDataReader view over CqlCommand results.
	/// 
	/// Default assumptions for types are:
	///		key validator:						UTF8Type
	///		comparator (column name type)		UTF8Type
	///		value validator (column value type)	UTF8Type
	/// 
	///		TODO: support for super column families
	/// </summary>
	public class CqlDataReader : DbDataReader
	{
		private CqlConnection _Connection;
		private CqlResult _CqlResult;
		private bool _Closed;
		private int _RowIter;
		private CommandBehavior _CmdBehaviour;
		private IMarshaller _ColumnNameMarshaller;
		private IMarshaller _ColumnValueMarshaller;
		private IMarshaller _KeyMarshaller;

		private CqlRow CurrentRow
		{
			get
			{
				EnsureNotAtTheEnd();
				return _CqlResult.Rows[_RowIter];
			}
		}

		internal CqlDataReader(CqlResult cqlResult, CqlConnection connection, CommandBehavior cmdBehaviour)
		{
			_CqlResult = cqlResult;
			_Connection = connection;
			_RowIter = -1;
			_CmdBehaviour = cmdBehaviour;

			ResolveMarshallersFromSchema(cqlResult.Schema);

			if ((cmdBehaviour & CommandBehavior.SchemaOnly) == 0)
			{
                // TODO: is this really what we should do here?! what happens with SqlDataReader ?
				if (!_CqlResult.__isset.rows || _CqlResult.Type != CqlResultType.ROWS)
					throw new CqlException("non-scalar query has returned a result of non-ROWS type");
			}
		}

		private void ResolveMarshallersFromSchema(CqlMetadata cqlMetadata)
		{
			_KeyMarshaller = TypeResolver.GetMarshallerForCassandraType(TypeResolver.CassandraType.UTF8);
			_ColumnNameMarshaller = TypeResolver.GetMarshalledTypeForName(cqlMetadata.Default_name_type);
			_ColumnValueMarshaller = TypeResolver.GetMarshallerForCassandraType(TypeResolver.CassandraType.UTF8);
		}

		public override void Close()
		{
			if (!_Closed)
			{
				_Closed = true;

				if ((_CmdBehaviour & CommandBehavior.CloseConnection) == CommandBehavior.CloseConnection)
					_Connection.Close();
			}
		}

		private void EnsureNotClosed()
		{
			if (_Closed)
				throw new InvalidOperationException("CqlDataReader is closed");
		}

		private void EnsureNotAtTheEnd()
		{
			EnsureNotClosed();

			if (_CqlResult == null
					|| _RowIter == _CqlResult.Rows.Count
					|| (_RowIter == 1 && ((_CmdBehaviour & CommandBehavior.SingleRow) == CommandBehavior.SingleRow)))
			{
				throw new InvalidOperationException("CqlDataReader is at the end of result set");
			}
		}

		public override int Depth
		{
			get {
				// TODO: as Cassandra support hierarchical queries, this would be nice to expose on Super type CF
				EnsureNotClosed();
				return 0;
			}
		}

		public override DataTable GetSchemaTable()
		{
			EnsureNotClosed();

			// TODO: write CqlDataReader.GetSchemaTable
			throw new NotImplementedException();
		}

		public override bool IsClosed
		{
			get { return _Closed; }
		}

		public override bool NextResult()
		{
			EnsureNotClosed();

			if (_CqlResult == null)
				return false;

			if (_RowIter == _CqlResult.Rows.Count - 1)
				return false;

			if (_RowIter == 1 && ((_CmdBehaviour & CommandBehavior.SingleRow) == CommandBehavior.SingleRow))
				return false;

			_RowIter += 1;
			return true;
		}

		public override bool Read()
		{
			EnsureNotAtTheEnd();
			return true;
		}

		public override int RecordsAffected
		{
			get {
				EnsureNotClosed();

				// cassandra select queries never affect data store
				return 0;
			}
		}

		public override int FieldCount
		{
			get { return CurrentRow.Columns.Count; }
		}

		public override bool GetBoolean(int i)
		{
			// TODO: add more heuristics to GetBoolean
			return Convert.ToBoolean(GetValue(i));
		}

		public override byte GetByte(int i)
		{
			return CurrentRow.Columns[i].Value[0];
		}

		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			var colValue = CurrentRow.Columns[i].Value;
			long len = Math.Max(length, colValue.Length);
			Array.Copy(colValue, 0, buffer, bufferoffset, len);
			return len;
		}

		public override char GetChar(int i)
		{
			return GetString(i)[0];
		}

		public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			string str = GetString(i);
			long len = Math.Max(str.Length, length);
			str.CopyTo(0, buffer, bufferoffset, length);
			return len;
		}

		public override string GetDataTypeName(int i)
		{
			// TODO: this might lie, but we don't have enough metadata to resolve this
			return new BytesType().CassandraTypeName;
		}

		public override DateTime GetDateTime(int i)
		{
			// TODO: handle TimeUUID properly
			// TODO: which is the most populare datetime format?
			return DateTime.Parse(GetString(i));
		}

		public override decimal GetDecimal(int i)
		{
			return Convert.ToDecimal(GetValue(i));
		}

		public override double GetDouble(int i)
		{
			return (float)Convert.ToDouble(GetValue(i));
		}

		public override Type GetFieldType(int i)
		{
			// TODO: this might lie but we don't have enough data 
			return typeof(byte[]);
		}

		public override float GetFloat(int i)
		{
			return (float)GetDouble(i);
		}

		public override Guid GetGuid(int i)
		{
			return new Guid((byte[])GetValue(i));
		}

		public override short GetInt16(int i)
		{
			return Convert.ToInt16(GetValue(i));
		}

		public override int GetInt32(int i)
		{
			return Convert.ToInt32(GetValue(i));
		}

		public override long GetInt64(int i)
		{
			return Convert.ToInt64(GetValue(i));
		}

		public override string GetName(int i)
		{
			if (i == 0)
				return "KEY";
			else
				return _ColumnNameMarshaller.Unmarshall(CurrentRow.Columns[i].Name).ToString();
		}

		public override int GetOrdinal(string name)
		{
			return _GetColumnNumByName(_ColumnNameMarshaller.Marshall(name));
		}

		public override string GetString(int i)
		{
			return Convert.ToString(GetValue(i));
		}

		public override object GetValue(int i)
		{
			byte[] colValue = CurrentRow.Columns[i].Value;

			return _ColumnValueMarshaller.Unmarshall(colValue);
		}

		public override int GetValues(object[] values)
		{
			object[] ourValues = CurrentRow.Columns.Select(c => _ColumnValueMarshaller.Unmarshall(c.Value)).ToArray();

			int len = Math.Min(values.Length, ourValues.Length);
			Array.Copy(ourValues, values, len);
			return len;
		}

		public override bool IsDBNull(int i)
		{
			EnsureNotAtTheEnd();
			return false; 			// TODO: columns are never DBNull, dunno if this is right?
		}

		public override object this[string name]
		{
			get
			{
				return _ColumnValueMarshaller.Unmarshall(_GetColumnByName(_ColumnNameMarshaller.Marshall(name)).Value);
			}
		}

		public override object this[int i]
		{
			get
			{
				return _ColumnValueMarshaller.Unmarshall(CurrentRow.Columns[i].Value);
			}
		}

		private Column _GetColumnByName(byte[] name)
		{
			return CurrentRow.Columns[_GetColumnNumByName(name)];
		}

		private int _GetColumnNumByName(byte[] name)
		{
			var cols = CurrentRow.Columns;

			for (int i = 0; i < cols.Count; i++)
			{
				// TODO: some say SequenceEqual is bad for performance
				if (cols[i].Name.SequenceEqual(name))
					return i;
			}

			throw new IndexOutOfRangeException("No column with the specified name was found");
		}

		public override System.Collections.IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public override bool HasRows
		{
			get { throw new NotImplementedException(); }
		}
	}
}
