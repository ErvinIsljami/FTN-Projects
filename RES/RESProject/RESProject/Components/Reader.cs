using RESProject.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RESProject.Components
{

    public sealed class Reader
    {
        private static object padlock = new object();
        private static Reader instance = null;
        static Reader()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Reader();
                }
            }
        }
        public static Reader Instance()
        {
            lock(padlock)
            {
                if(instance == null)
                {
                    instance = new Reader();
                }
                return instance;
            }
        }

        public bool ReadInterval(Codes code, DateTime startInterval, DateTime endInterval)
        {
            int dataSet = (int)code % 5 + 1;
            string path = "DataSet" + dataSet.ToString() + ".xml";
            if(!File.Exists(path))
            {
                Logger.Instance().LogEvent("Reader", "Could not open file to read. File not found.");
                return false;
            }
            if(startInterval == null || endInterval == null)
            {
                return false;
            }
            if((int)code > 10)
            {
                return false;
            }
            CodeMaps cm = new CodeMaps();
            XDocument document = XDocument.Load(path);
            string parentName = cm.GetNameForCode(code);
            XElement root = document.Element("Codes");
            XElement parent = root.Element(parentName);

            foreach(XElement atr in parent.Descendants())
            {
                DateTime attributeTime = new DateTime();
                attributeTime = DateTime.Parse(atr.Attribute("time").Value);
                if(startInterval < attributeTime && endInterval > attributeTime)
                {
                    Logger.Instance().LogEvent("Reader", "Reader printed entry");
                    Console.WriteLine(atr.Attribute("method").Value + " " + atr.Attribute("value").Value + " " + atr.Attribute("time").Value);
                }
            }



            Logger.Instance().LogEvent("Reader", "Reader finished reading");
            //Console.ReadLine();
            // Console.WriteLine("Value:" + reader.GetAttribute("value") + "," + reader.GetAttribute("DateTime").ToString());
            return true;
        }
    }
}

