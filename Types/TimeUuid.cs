using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.Cassandra.Cql.Marshal.Helpers;

namespace Apache.Cassandra.Client.Types
{
	public struct TimeUuid : IFormattable, IComparable, IComparable<TimeUuid>, IEquatable<TimeUuid>
	{
		private static DateTimePrecise _DateTimePrecise = new DateTimePrecise(10);
		private static readonly DateTime GregorianCalendarStart = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);

		private Guid _Guid;

		public Guid Guid
		{
			get
			{
				return _Guid;
			}
		}

		public TimeUuid(byte[] bytes)
		{
			_Guid = new Guid(bytes);

			EnsureVersionIs1();
		}

		public TimeUuid(Guid guid)
		{
			_Guid = guid;

			EnsureVersionIs1();
		}

		public TimeUuid(string guid)
		{
			_Guid = new Guid(guid);

			EnsureVersionIs1();
		}

		public TimeUuid(DateTimePrecise datetimePrecise)
		{
			_Guid = GuidGenerator.GenerateTimeBasedGuid(datetimePrecise);
		}

		public TimeUuid(DateTime datetime)
		{
			_Guid = GuidGenerator.GenerateTimeBasedGuid(datetime.ToUniversalTime()); 
		}

		public TimeUuid(long ticks)
		{
			_Guid = GuidGenerator.GenerateTimeBasedGuid(ticks);
		}

		public static TimeUuid NewTimeUUID()
		{
			lock (_DateTimePrecise)
			{
				return new TimeUuid(_DateTimePrecise);
			}
		}

		private void EnsureVersionIs1()
		{
			var bytes = _Guid.ToByteArray();

			var version = (bytes[7] >> 4) & 0xf;
			if (version != 1)
				throw new Exception(string.Format("expecting time-based UUID (v1), but v{0} was found", version));
		}

		public long Ticks
		{
			get
			{
				var bytes = _Guid.ToByteArray();

				// remove the version nible
				bytes[7] &= 0x0f;

				long ticks = BitConverter.ToInt64(bytes, 0);
				ticks += GregorianCalendarStart.Ticks;

				return ticks;
			}
		}

		public DateTime DateTime
		{
			get
			{
				return new DateTime(Ticks, DateTimeKind.Utc);
			}
		}

		public override bool Equals(object obj)
		{
			return (obj is TimeUuid) && _Guid == ((TimeUuid)obj)._Guid;
		}

		public override string ToString()
		{
			return _Guid.ToString();
		}

		public override int GetHashCode()
		{
			return _Guid.GetHashCode();
		}

		public byte[] ToByteArray()
		{
			return _Guid.ToByteArray();
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return _Guid.ToString(format, formatProvider);
		}

		public bool Equals(TimeUuid other)
		{
			return _Guid.Equals(other._Guid);
		}

		int IComparable.CompareTo(object obj)
		{
			return Ticks.CompareTo(((TimeUuid)obj).Ticks);
		}

		int IComparable<TimeUuid>.CompareTo(TimeUuid other)
		{
			return Ticks.CompareTo(other.Ticks);
		}


		public static bool operator !=(TimeUuid a, TimeUuid b)
		{
			return !a.Equals(b);
		}

		public static bool operator ==(TimeUuid a, TimeUuid b)
		{
			return a.Equals(b);
		}
	}
}
