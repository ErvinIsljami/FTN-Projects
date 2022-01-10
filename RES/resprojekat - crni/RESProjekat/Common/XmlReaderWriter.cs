using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Common
{
	public class XmlReaderWriter
	{
		public void SerializeObject<T>(T serializableObject, string filename)
		{
			if (serializableObject == null)
			{
				return;
			}

			try
			{
				// XmlDocument is an object that represents an XML element
				XmlDocument xmlDocument = new XmlDocument();
				// XmlSerializer is an object that represents a serializer that will serialize an object based on it's type
				XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());

				// MemoryStream is an object that represents a byte array stream to RAM
				using (MemoryStream stream = new MemoryStream())
				{
					serializer.Serialize(stream, serializableObject);
					stream.Position = 0;
					xmlDocument.Load(stream);
					xmlDocument.Save(filename);
				}
			}
			catch (Exception e)
			{

			}
		}

		public T DeserializeObject<T>(string filename)
		{
			if (String.IsNullOrEmpty(filename))
			{
				return default(T);
			}

			// Object that will hold the read object
			T objectOut = default(T);

			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				// Turns XML code into a string
				string xmlString = xmlDocument.OuterXml;

				using (StringReader read = new StringReader(xmlString))
				{
					// Get the type of the object for the serializer
					Type outType = typeof(T);
					XmlSerializer serializer = new XmlSerializer(outType);
					using (XmlReader reader = new XmlTextReader(read))
					{
						objectOut = (T)serializer.Deserialize(reader);
					}
				}
			}
			catch (Exception e)
			{

			}

			return objectOut;
		}
	}
}
