using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Apache.Cassandra.Cql.Internal
{
	class Util
	{
		public static byte[] CompressQueryString(string queryString, Compression compression)
		{
			byte[] queryAsBytes = Encoding.UTF8.GetBytes(queryString);

			switch (compression)
			{
				case Compression.GZIP:
					using (var memOutStream = new MemoryStream())
					{
						using (var deflateStream = new DeflateStream(memOutStream, CompressionMode.Compress))
						{
							deflateStream.Write(queryAsBytes, 0, queryAsBytes.Length);
							deflateStream.Flush();
							return memOutStream.ToArray();
						}
					}
				case Compression.NONE:
					return queryAsBytes;
				default:
					throw new NotImplementedException("unknown compression method");
			}
			
		}
	}
}
