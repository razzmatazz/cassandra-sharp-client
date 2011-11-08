using System;
using System.Collections.Generic;
using System.Text;

namespace Apache.Cassandra.Cql.Marshal.Helpers
{
    /// <summary>
    /// GuidGenerator from managed fusion/fluentcassandra
    /// <remarks>http://github.com/managedfusion/managedfusion/raw/master/Source/GuidGenerator.cs
    /// It was modified by me to use DateTimePrecise
    /// </remarks>
    /// </summary>
	public static class GuidGenerator
	{
		// guid version types
		private enum GuidVersion : byte
		{
			TimeBased = 0x01,
			Reserved = 0x02,
			NameBased = 0x03,
			Random = 0x04
		}

		// number of bytes in guid
		private const int ByteArraySize = 16;

		// multiplex variant info
		private const int VariantByte = 8;
        private const int VariantByteMask = 0x3f;
        private const int VariantByteShift = 0x80;

		// multiplex version info
        private const int VersionByte = 7;
        private const int VersionByteMask = 0x0f;
        private const int VersionByteShift = 4;

		// indexes within the uuid array for certain boundaries
		private static readonly byte TimestampByte = 0;
		private static readonly byte GuidClockSequenceByte = 8;
		private static readonly byte NodeByte = 10;

		// offset to move from 1/1/0001, which is 0-time for .NET, to gregorian 0-time of 10/15/1582
		private static readonly DateTime GregorianCalendarStart = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);

		// random node that is 16 bytes
		private static readonly byte[] RandomNode;

		private static Random _random = new Random();
        private static DateTimePrecise _dateTimeSeed;

		static GuidGenerator()
		{
			RandomNode = new byte[6];
			_random.NextBytes(RandomNode);
            _dateTimeSeed = new DateTimePrecise(10);
		}

        /// <summary>
        /// Generate a Time based Guid using Datetime.UtcNow
        /// </summary>
        /// <returns>a GUID</returns>
		public static Guid GenerateTimeBasedGuid()
		{
			return GenerateTimeBasedGuid(_dateTimeSeed.UtcNow.Ticks, RandomNode);
		}
        /// <summary>
        /// Generate a Time based Guid using given Datetime (converted into UtcNow first)
        /// </summary>
        /// <returns>a GUID</returns>
		public static Guid GenerateTimeBasedGuid(DateTime dateTime)
		{
			return GenerateTimeBasedGuid(dateTime.Ticks, RandomNode);
		}

        /// <summary>
        /// Generate a Time based Guid using given DatetimePrecise (converted into UtcNow first)
        /// </summary>
        /// <returns>a GUID</returns>
        public static Guid GenerateTimeBasedGuid(DateTimePrecise dateTime)
        {
            return GenerateTimeBasedGuid(dateTime.UtcNow.Ticks, RandomNode);
        }

        /// <summary>
        /// Generate a Time based Guid using given Datetime (converted into UtcNow first)
        /// </summary>
        /// <returns>a GUID</returns>
		public static Guid GenerateTimeBasedGuid(long dateTimeTicks, byte[] node)
		{
            long ticks = dateTimeTicks - GregorianCalendarStart.Ticks;

			byte[] guid = new byte[ByteArraySize];
			byte[] clockSequenceBytes = BitConverter.GetBytes(Convert.ToInt16(Environment.TickCount % Int16.MaxValue));
			byte[] timestamp = BitConverter.GetBytes(ticks);

			// copy node
			Array.Copy(node, 0, guid, NodeByte, node.Length);

			// copy clock sequence
			Array.Copy(clockSequenceBytes, 0, guid, GuidClockSequenceByte, clockSequenceBytes.Length);

			// copy timestamp
			Array.Copy(timestamp, 0, guid, TimestampByte, timestamp.Length);

			// set the variant
			guid[VariantByte] &= (byte)VariantByteMask;
			guid[VariantByte] |= (byte)VariantByteShift;

			// set the version
			guid[VersionByte] &= (byte)VersionByteMask;
			guid[VersionByte] |= (byte)((byte)GuidVersion.TimeBased << VersionByteShift);

			return new Guid(guid);
		}
	}
}
