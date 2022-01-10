using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Common.Model
{
	public class LC
	{
		string guid;
		List<Generator> generators;

		public LC()
		{

		}

		public LC(string guid, List<Generator> generators)
		{
			this.guid = guid;
			this.generators = generators;
		}

		public string Guid { get => guid; set => guid = value; }
		[XmlArray("Generators")]
		public List<Generator> Generators { get => generators; set => generators = value; }
	}
}
