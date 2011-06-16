using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Apache.Cassandra.Cql
{
	public class CqlParameterCollection: DbParameterCollection
	{
		private IDictionary<string, CqlParameter> _ParameterByName;
		private List<CqlParameter> _Parameters;

		public CqlParameterCollection()
		{
			_ParameterByName = new Dictionary<string, CqlParameter>();
			_Parameters = new List<CqlParameter>();
		}

		internal void EnsureParameterCollectionIsComplete()
		{
			if (_ParameterByName.Count != _Parameters.Count)
				throw new CqlException("some of parameter in parameter collection don't have ParameterName set");

			var firstParamWoValue = _Parameters.FirstOrDefault(p => p.Value == null);
			if (firstParamWoValue != null)
				throw new CqlException(string.Format("parameter {0} has no value set", firstParamWoValue.ParameterName));
		}

		internal void OnBoundParameterNameChange(CqlParameter param, string previousName, string newName)
		{
			if (_ParameterByName.ContainsKey(newName))
				throw new ArgumentException("parameter with name {0} already exists in CqlParameterCollection", newName);

			if (!string.IsNullOrEmpty(previousName))
			{
				_ParameterByName.Remove(previousName);
			}

			_ParameterByName[newName] = param;
		}

		public override int Add(object value)
		{
			var param = (CqlParameter)value;
			param.BindToCollection(this);

			_Parameters.Add(param);
			if (!string.IsNullOrEmpty(param.ParameterName))
				_ParameterByName[param.ParameterName] = param;

			return _Parameters.Count - 1;
		}

		public override void AddRange(Array values)
		{
			foreach (var val in values)
				Add(val);
		}

		public override void Clear()
		{
			_Parameters.Clear();
			_ParameterByName.Clear();
		}

		public override bool Contains(string value)
		{
			return _ParameterByName.ContainsKey(value);
		}

		public override bool Contains(object value)
		{
			return _Parameters.Contains((CqlParameter)value);
		}

		public override void CopyTo(Array array, int index)
		{
			((System.Collections.ICollection)_Parameters).CopyTo(array, index);
		}

		public override int Count
		{
			get { return _Parameters.Count; }
		}

		public override System.Collections.IEnumerator GetEnumerator()
		{
			return ((System.Collections.ICollection)_Parameters).GetEnumerator();
		}

		protected override DbParameter GetParameter(string parameterName)
		{
			return _ParameterByName[parameterName];
		}

		protected override DbParameter GetParameter(int index)
		{
			return _Parameters[index];
		}

		public override int IndexOf(string parameterName)
		{
			for(int i = 0; i < _Parameters.Count; i++)
				if (_Parameters[i].ParameterName == parameterName)
					return i;

			return -1;
		}

		public override int IndexOf(object value)
		{
			var param = (CqlParameter)value;

			for (int i = 0; i < _Parameters.Count; i++)
				if (_Parameters[i] == value)
					return i;

			return -1;			
		}

		public override void Insert(int index, object value)
		{
			_Parameters.Insert(index, (CqlParameter)value);
		}

		public override bool IsFixedSize
		{
			get { return false; }
		}

		public override bool IsReadOnly
		{
			get { return false; }
		}

		public override bool IsSynchronized
		{
			get { return false; }
		}

		public override void Remove(object value)
		{
			var idx = IndexOf(value);
			if (idx != -1)
				throw new ArgumentOutOfRangeException("no CqlParamter with given value was found in CqlParameterCollection");

			RemoveAt(idx);
		}

		public override void RemoveAt(string parameterName)
		{
			var param = _ParameterByName[parameterName];
			_ParameterByName.Remove(parameterName);
			_Parameters.Remove(param);
		}

		public override void RemoveAt(int index)
		{
			var param = _Parameters[index];

			_Parameters.RemoveAt(index);

			if (!string.IsNullOrEmpty(param.ParameterName))
				_ParameterByName.Remove(param.ParameterName);
		}

		protected override void SetParameter(string parameterName, DbParameter value)
		{
			value.ParameterName = parameterName;
			SetParameter(_Parameters.Count, value);
		}

		protected override void SetParameter(int index, DbParameter value)
		{
			var parameterName = value.ParameterName;
			if (string.IsNullOrEmpty(value.ParameterName))
				throw new ArgumentException("invalid parameter name: must be not null or empty");

			if (_ParameterByName.ContainsKey(parameterName))
				throw new ArgumentException(string.Format("parameter {0} already has been set on this CqlParameterCollection", parameterName));

			CqlParameter param = (value is CqlParameter) ? (CqlParameter)value : new CqlParameter(parameterName, value.Value);
			param.BindToCollection(this);
			_ParameterByName[parameterName] = param;
			_Parameters.Insert(index, param);
		}

		public override object SyncRoot
		{
			get { return this; }
		}
	}
}
