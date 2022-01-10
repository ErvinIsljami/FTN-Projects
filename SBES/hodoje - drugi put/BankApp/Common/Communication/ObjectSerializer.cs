using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common.Communication
{
	public static class ObjectSerializer
	{
		public static byte[] ObjectToByteArray(Object obj)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}
	}
}
