using RESProject.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RESProject.Components
{
    public sealed class Historical
    {
        private static List<HistoricalDescription> ListDescription { get; set; }
        private static readonly object padlock = new object();
        private static Historical instance = null;
        private static Dictionary<int, bool> xmlCreators;
        public Historical()
        {
            ListDescription = new List<HistoricalDescription>();
            xmlCreators = new Dictionary<int, bool> { { 1, false }, { 2, false }, { 3, false }, { 4, false }, { 5, false } };

        }
        static Historical()
        {
            ListDescription = new List<HistoricalDescription>();
            xmlCreators = new Dictionary<int, bool> { { 1, false }, { 2, false }, { 3, false }, { 4, false }, { 5, false } };
        }
        public static Historical Instance()
        {
            lock(padlock)
            {
                if (instance == null)
                {
                    instance = new Historical();
                }

                return instance;
            }
        }

        public bool WriteToXML(DeltaCD dcd)
        {
            if(dcd == null)
            {
                Logger.Instance().LogEvent("Historical", "Delta cd is not valid.");
                return false;
            }
            foreach(CollectionDescription cd in dcd.Add)
            {
                if (!AutomaticAdd(cd))
                {
                    Logger.Instance().LogEvent("Historical", "Automatic add not valid.");
                    return false;
                }
            }
            foreach (CollectionDescription cd in dcd.Update)
            {
                if (!AutomaticUpdate(cd))
                {
                    Logger.Instance().LogEvent("Historical", "Automatic update not valid.");
                    return false;
                }
            }
            foreach (CollectionDescription cd in dcd.Delete)
            {
                if(!AutomaticDelete(cd))
                {
                    Logger.Instance().LogEvent("Historical", "Automatic delete not valid.");
                    return false;
                }
            }

            for (int i = 0; i < 54; i++)
            {
                int a = 9;
                a--;
            }

            return true;
        }
        public bool ManualWriting(Codes code, double value)
        {
            int dataSet = (int)code % 5 + 1;
            string path = "DataSet" + dataSet.ToString() + ".xml";
            CodeMaps maps = new CodeMaps();
            if(!File.Exists(path))
            {
                if (!CreateXmlDocument(dataSet, path))
                {
                    Logger.Instance().LogEvent("Historical", "Xml couldnt be created.");
                    return false;
                }
            }
            if((int)code > 10 || (int)code < 0)
            {
                Logger.Instance().LogEvent("Historical", "Code out of range.");
                return false;
            }
            //LINQ to XML
            XDocument document = XDocument.Load(path);
            string name = maps.GetNameForCode(code);
            
            DateTime timeOfEntry = DateTime.Now;
            string t = timeOfEntry.ToString("HH:mm");
            
            document.Element("Codes").Element(name).Add(new XElement("entry",   new XAttribute("method", "manual"), 
                                                                                new XAttribute("value", value.ToString("#.##")), 
                                                                                new XAttribute("time", t),
                                                                                new XAttribute("id", "-1")));
            //List<XElement> GetAllEntriesForCode(string code);

            document.Save(path);
            Logger.Instance().LogEvent("Historical", "Manual writing finished.");
            return true;
        }
        public bool CreateXmlDocument(int dataSet, string path)
        {
            if(dataSet > 5 || dataSet < 1)
            {
                return false;
            }
            if(!Uri.IsWellFormedUriString(path,UriKind.RelativeOrAbsolute))
            {
                return false;
            }

            if(File.Exists(path))
            {
                return false;
            }

            using (FileStream fs = new FileStream(path, FileMode.CreateNew))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.GetEncoding("ISO-8859-1");
                settings.NewLineChars = Environment.NewLine;
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(fs, settings))
                {
                    Codes code1 = (Codes)dataSet - 1;
                    Codes code2 = (Codes)(dataSet + 5 - 1);
                    CodeMaps maps = new CodeMaps();
                    writer.WriteStartDocument(true);

                    writer.WriteStartElement("Codes");
                    writer.WriteStartElement(string.Format(maps.GetNameForCode(code1)));
                    writer.WriteFullEndElement();
                    writer.WriteStartElement(string.Format(maps.GetNameForCode(code2)));
                    writer.WriteFullEndElement();
                    writer.WriteFullEndElement();

                    writer.WriteEndDocument();
                    writer.Close();
                }
                fs.Close();
            }
            
            Logger.Instance().LogEvent("Historical", "File " + path + " created.");
            return true;
        }
        public bool IsOutOfDeadband(DumpingProperty dp)
        {
            if(dp == null)
            {
                return false;
            }
            if(dp.Code == Codes.CODE_DIGITAL)
            {
                return true;
            }
            int dataSet = (int)dp.Code % 5 + 1;
            string path = "DataSet" + dataSet.ToString() + ".xml";
            if(!File.Exists(path))
            {
                return false;
            }

            CodeMaps cm = new CodeMaps();
            XDocument document = XDocument.Load(path);
            string parentName = cm.GetNameForCode(dp.Code);
            XElement root = document.Element("Codes");
            XElement parent = root.Element(parentName);
            bool isOutOfRange = true;

            foreach (XElement element in parent.Descendants())
            {
                double value = Double.Parse(element.Attribute("value").Value);
                double deadBand = dp.DumpingValue * 0.02;
                if (value < dp.DumpingValue + deadBand && value > dp.DumpingValue - deadBand)
                {
                    isOutOfRange = false;
                    break;
                }
            }
            
            return isOutOfRange;
        }
        public bool AutomaticAdd(CollectionDescription cd)
        {
            if (cd == null)
                return false;
            string path = "DataSet" + cd.DataSet.ToString() + ".xml";
            CodeMaps map = new CodeMaps();
           
            if (!File.Exists(path))
            {
                if (!CreateXmlDocument(cd.DataSet, path))
                {
                    return false;
                }
            }
            if (cd.DumpingPropertyCollection.Count != 2)
            {
                return false;
            }
            XDocument document = XDocument.Load(path);
            string element1Name = map.GetNameForCode(cd.DumpingPropertyCollection[0].Code);
            string element2Name = map.GetNameForCode(cd.DumpingPropertyCollection[1].Code);
            DateTime timeOfEntry = DateTime.Now;
            string time = timeOfEntry.ToString("HH:mm");
            if (cd.ID / 100 == 0)
                return true;
            if(IsOutOfDeadband(cd.DumpingPropertyCollection[0]))
            {
                document = XDocument.Load(path);
                document.Element("Codes").Element(element1Name).Add(new XElement("entry", new XAttribute("id", cd.ID.ToString()),
                                                                                        new XAttribute("method", "automatic"),
                                                                                        new XAttribute("code", element1Name),
                                                                                        new XAttribute("value", cd.DumpingPropertyCollection[0].DumpingValue.ToString("#.##")),
                                                                                        new XAttribute("time", time)));
                document.Save(path);
                
            }


            if (IsOutOfDeadband(cd.DumpingPropertyCollection[1]))
            {
                document = XDocument.Load(path);
                document.Element("Codes").Element(element2Name).Add(new XElement("entry", new XAttribute("id", cd.ID.ToString()),
                                                                                            new XAttribute("method", "automatic"),
                                                                                            new XAttribute("code", element2Name),
                                                                                            new XAttribute("value", cd.DumpingPropertyCollection[1].DumpingValue.ToString("#.##")),
                                                                                            new XAttribute("time", time)));

                document.Save(path);
            }


            return true;
        }
        public bool AutomaticUpdate(CollectionDescription cd)
        {
            if (cd == null)
                return false;
            string path = "DataSet" + cd.DataSet.ToString() + ".xml";
            CodeMaps map = new CodeMaps();
            if (!File.Exists(path))
            {
                return false;
            }
            if (cd.DumpingPropertyCollection.Count != 2)
            {
                return false;
            }

            XDocument document = XDocument.Load(path);
            DateTime timeOfEntry = DateTime.Now;
            string time = timeOfEntry.ToString("HH:mm");
            if (cd.ID / 100 == 0)
                return true;
            if (CheckIfUpdatable(cd.DumpingPropertyCollection[0].Code, cd.ID))
            {
                if (IsOutOfDeadband(cd.DumpingPropertyCollection[0]))
                {

                    document = XDocument.Load(path);
                    string element1Name = map.GetNameForCode(cd.DumpingPropertyCollection[0].Code);
                    document.Element("Codes").Element(element1Name).Add(new XElement("entry", new XAttribute("id", cd.ID.ToString()),
                                                                                           new XAttribute("method", "automatic"),
                                                                                           new XAttribute("code", element1Name),
                                                                                           new XAttribute("value", cd.DumpingPropertyCollection[0].DumpingValue.ToString("#.##")),
                                                                                           new XAttribute("time", time)));
                    document.Save(path);
                }
            }
            if(CheckIfUpdatable(cd.DumpingPropertyCollection[1].Code, cd.ID))
            {
                if (IsOutOfDeadband(cd.DumpingPropertyCollection[1]))
                {

                    document = XDocument.Load(path);
                    string element2Name = map.GetNameForCode(cd.DumpingPropertyCollection[1].Code);
                    document.Element("Codes").Element(element2Name).Add(new XElement("entry", new XAttribute("id", cd.ID.ToString()),
                                                                                                new XAttribute("method", "automatic"),
                                                                                                new XAttribute("code", element2Name),
                                                                                                new XAttribute("value", cd.DumpingPropertyCollection[1].DumpingValue.ToString("#.##")),
                                                                                                new XAttribute("time", time)));

                    document.Save(path);
                }
            }
            


            return true;
        }
        public bool AutomaticDelete(CollectionDescription cd)
        {
            if(cd == null)
                return false;

            string path = "DataSet" + cd.DataSet.ToString() + ".xml";
            CodeMaps map = new CodeMaps();
            bool ret1 = true;
            bool ret2 = true;
            if (!File.Exists(path))
            {
                return false;
            }
            if (cd.DumpingPropertyCollection.Count != 2)
            {
                return false;
            }

            XDocument document = XDocument.Load(path);
            string name = map.GetNameForCode(cd.DumpingPropertyCollection[0].Code);
            XElement element = document.Element("Codes").Element(name);
            XElement temp = null;
            foreach (XElement e in element.Elements())
            {
                if(Double.Parse(e.Attribute("value").Value) == cd.DumpingPropertyCollection[0].DumpingValue)
                {
                    temp = e;
                    break;
                }
            }
            if (temp != null)
                temp.Remove();
            else
                ret1 = false;

            string name2 = map.GetNameForCode(cd.DumpingPropertyCollection[1].Code);
            XElement element2 = document.Element("Codes").Element(name2);
            XElement temp2 = null;
            foreach (XElement e in element2.Elements())
            {
                if (Double.Parse(e.Attribute("value").Value) == cd.DumpingPropertyCollection[1].DumpingValue)
                {
                    temp2 = e;
                    break;
                }
            }
            if (temp2 != null)
                temp2.Remove();
            else
                ret2 = false;
            
            return ret1 && ret2;
        }
        public bool CheckIfUpdatable(Codes code, int id)
        {
            if ((int)code > 10 || (int)code < 0)
                return false;

            string path = "DataSet" + ((int)code % 5 + 1) + ".xml";
            bool found = false;
            CodeMaps map = new CodeMaps();
            if (!File.Exists(path))
            {
                return false;
            }

            XDocument document = XDocument.Load(path);
            string element1Name = map.GetNameForCode(code);

            foreach (XElement e in document.Element("Codes").Element(element1Name).Elements())
            {
                if (e.Attribute("method").Value == "automatic")
                {
                    found = true;
                    break;
                }
            }
            bool unique = true;

            foreach (XElement e in document.Element("Codes").Element(element1Name).Elements())
            {
                if (Int32.Parse(e.Attribute("id").Value) == id)
                {
                    unique = false;
                    break;
                }
            }


            return found && unique;
        }
    }
}
