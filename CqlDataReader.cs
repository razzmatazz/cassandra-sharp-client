using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Apache.Cassandra.Cql.Internal;
using Apache.Cassandra.Cql.Internal.Marshal;

namespace Apache.Cassandra.Cql
{
	public class CqlDataReader: IDataReader
	{
		private ActualCqlConnection _Connection;
		private CqlResult _CqlResult;
		private bool _Closed;
		private int _RowIter;
		private ColumnFamilyMetadata _Cf;

		private CqlRow CurrentRow
		{
			get
			{
				EnsureNotAtTheEnd();
				return _CqlResult.Rows[_RowIter];
			}
		}

		internal CqlDataReader(CqlResult cqlResult, ActualCqlConnection connection, ColumnFamilyMetadata md)
		{
			_CqlResult = cqlResult;
			_Connection = connection;
			_RowIter = -1;
			_Cf = md;

			// TODO support super column families
			if (md.FamilyType == ColumnFamilyMetadata.ColumnFamilyType.Super)
				throw new NotImplementedException("super column families are not supported yet");

			if (!_CqlResult.__isset.rows || _CqlResult.Type != CqlResultType.ROWS)
				throw new CqlException("non-scalar query has returned a result of non-ROWS type");
		}

		public void Close()
		{
			_Closed = true;
			// nop
		}

		private void EnsureNotClosed()
		{
			if (_Closed)
				throw new InvalidOperationException("CqlDataReader is closed");
		}

		private void EnsureNotAtTheEnd()
		{
			EnsureNotClosed();

			if (_RowIter == _CqlResult.Rows.Count)
				throw new InvalidOperationException("CqlDataReader is at the end of result set");
		}

		public int Depth
		{
			get {
				// TODO: as Cassandra support hierarchical queries, this would be nice to expose on Super type CF
				EnsureNotClosed();
				return 0;
			}
		}

		public DataTable GetSchemaTable()
		{
			EnsureNotClosed();

			// TODO: write CqlDataReader.GetSchemaTable
			throw new NotImplementedException();
		}

		public bool IsClosed
		{
			get { return _Closed; }
		}

		public bool NextResult()
		{
			EnsureNotClosed();

			if (_RowIter == _CqlResult.Rows.Count - 1)
				return false;

			_RowIter += 1;
			return true;
		}

		public bool Read()
		{
			EnsureNotAtTheEnd();
			return true;
		}

		public int RecordsAffected
		{
			get {
				EnsureNotClosed();

				throw new NotImplementedException();
			}
		}

		public void Dispose()
		{
			Close();
		}

		public int FieldCount
		{
			get { return CurrentRow.Columns.Count; }
		}

		public bool GetBoolean(int i)
		{
			// TODO: add more heuristics here
			return Convert.ToBoolean(GetValue(i));
		}

		public byte GetByte(int i)
		{
			if (_Cf.ColumnValueType == TypeResolver.CassandraType.Bytes)
				return CurrentRow.Columns[i].Value[0];
			else
			{
				return Convert.ToByte(GetValue(i));
			}
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			var colValue = CurrentRow.Columns[i].Value;
			long len = Math.Max(length, colValue.Length);
			Array.Copy(colValue, 0, buffer, bufferoffset, len);
			return len;
		}

		public char GetChar(int i)
		{
			return GetString(i)[0];
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			string str = GetString(i);
			long len = Math.Max(str.Length, length);
			str.CopyTo(0, buffer, bufferoffset, length);
			return len;
		}

		public IDataReader GetData(int i)
		{
			EnsureNotAtTheEnd();

			// TODO: support for multicolumns
			throw new NotImplementedException();
		}

		public string GetDataTypeName(int i)
		{
			return _Cf.ColumnValueMarshaller.CassandraTypeName;
		}

		public DateTime GetDateTime(int i)
		{
			// TODO: handle TimeUUID properly
			// TODO: which is the most populare datetime format?
			return DateTime.Parse(GetString(i));
		}

		public decimal GetDecimal(int i)
		{
			return Convert.ToDecimal(GetValue(i));
		}

		public double GetDouble(int i)
		{
			return (float)Convert.ToDouble(GetValue(i));
		}

		public Type GetFieldType(int i)
		{
			return _Cf.ColumnValueMarshaller.MarshalledType;
		}

		public float GetFloat(int i)
		{
			return (float)GetDouble(i);
		}

		public Guid GetGuid(int i)
		{
			if (_Cf.ColumnValueType == TypeResolver.CassandraType.LexicalUUID
					|| _Cf.ColumnValueType == TypeResolver.CassandraType.TimeUUID
					|| _Cf.ColumnValueType == TypeResolver.CassandraType.UUID
				)
			{
				return new Guid(CurrentRow.Columns[i].Value);
			}
			else {
				return new Guid(GetString(i));
			};
		}

		public short GetInt16(int i)
		{
			return Convert.ToInt16(GetValue(i));
		}

		public int GetInt32(int i)
		{
			return Convert.ToInt32(GetValue(i));
		}

		public long GetInt64(int i)
		{
			return Convert.ToInt64(GetValue(i));
		}

		public string GetName(int i)
		{
			return Convert.ToString(_Cf.ColumnNameMarshaller.Unmarshall(CurrentRow.Columns[i].Name));
		}

		public int GetOrdinal(string name)
		{
			return _GetColumnNumByName(_Cf.ColumnNameMarshaller.Marshall(name));
		}

		public string GetString(int i)
		{
			return Convert.ToString(GetValue(i));
		}

		public object GetValue(int i)
		{
			var marshaller = (i == 0) ? _Cf.KeyMarshaller : _Cf.ColumnValueMarshaller;
			return marshaller.Unmarshall(CurrentRow.Columns[i].Value);
		}

		public int GetValues(object[] values)
		{
			object[] ourValues = CurrentRow.Columns.Select(c => _Cf.ColumnValueMarshaller.Unmarshall(c.Value)).ToArray();

			int len = Math.Min(values.Length, ourValues.Length);
			Array.Copy(ourValues, values, len);
			return len;
		}

		public bool IsDBNull(int i)
		{
			EnsureNotAtTheEnd();
			return false; 			// TODO: columns are never DBNull, dunno if this is right?
		}

		public object this[string name]
		{
			get
			{
				return _Cf.ColumnValueMarshaller.Unmarshall(_GetColumnByName(_Cf.ColumnNameMarshaller.Marshall(name)).Value);
			}
		}

		public object this[int i]
		{
			get
			{
				return _Cf.ColumnValueMarshaller.Unmarshall(CurrentRow.Columns[i].Value);
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

			throw new CqlException("column in a row could not be resolved by name");
		}
	}
}
