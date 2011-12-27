using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apache.Cassandra.Client.Types
{
	public class TimeUuidRange
	{
		public TimeUuid FromUuid { get; set; }
		public TimeUuid ToUuid { get; set; }

		public TimeUuidRange(TimeUuid from, TimeUuid to)
		{
			FromUuid = from;
			ToUuid = to;

			if (FromUuid.Ticks > ToUuid.DateTime.Ticks)
				throw new Exception("From > To");
		}

		public TimeUuidRange(DateTime from, DateTime to)
		{
			FromUuid = new TimeUuid(from);
			ToUuid = new TimeUuid(to);

			if (FromUuid.Ticks > ToUuid.DateTime.Ticks)
				throw new Exception("From > To");
		}

		public static TimeUuidRange From(DateTime from)
		{
			return From(new TimeUuid(from));
		}

		public static TimeUuidRange From(TimeUuid fromUuid)
		{
			return new TimeUuidRange(fromUuid, new TimeUuid(DateTime.Now.AddYears(1)));
		}

		public static TimeUuidRange To(DateTime to)
		{
			return To(new TimeUuid(to));
		}

		public static TimeUuidRange To(TimeUuid toUuid)
		{
			return new TimeUuidRange(new TimeUuid(DateTime.Now.AddYears(-1)), toUuid);
		}
	}
}
