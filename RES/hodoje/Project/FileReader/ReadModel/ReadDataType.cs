using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileReader.ReadModel
{
    [XmlType(TypeName = "STAVKA")]
    public class ReadDataType
    {
        [XmlElement("SAT")]
        public string Hour { get; set; }
        [XmlElement("LOAD")]
        public string Load { get; set; }
        [XmlElement("OBLAST")]
        public string GeoAreaId { get; set; }
    }
}
