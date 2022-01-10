using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CloudCompute
{
    public class XmlWork : FileWork
    {
        public XmlWork() { }

        public int ReturnNumberOfContainersForWork(string packetName, string rootDirectoryPath)
        {
            try
            {
                if (CheckIfRootDirectoryContainsPackets(rootDirectoryPath))
                {
                    DirectoryInfo rootDirectoryInfo = new DirectoryInfo(rootDirectoryPath);
                    DirectoryInfo[] subDirectories = rootDirectoryInfo.GetDirectories(packetName);

                    // The "Create" event is triggered when the folder is created, not when the files are finished copying
                    string filter = "*.xml";
                    FileInfo[] listOfFiles;

                    // Since there is not event when the file is finished it's creation, we check a few times until we get a valid result
                    while (true)
                    {
                        listOfFiles = subDirectories[0].GetFiles(filter).ToArray();
                        if (listOfFiles.Length > 0)
                        {
                            break;
                        }
                    }

                    string configFilename = ReturnConfigFileName(listOfFiles);
                    int numOfInstances = ParseConfigFileForNumOfInstances(configFilename);
                    return numOfInstances;
                }
            }
            catch (Exception)
            {
                return -1;
            }
            return -1;
        }

        public string ReturnConfigFileName(FileInfo[] listOfFiles)
        {
            string result = "";
            foreach (FileInfo file in listOfFiles)
            {
                if (file.Name.Split('.')[1] == "xml")
                {
                    result = file.FullName;
                    break;
                }
            }
            return result;
        }

        public bool IsValidValue(int value)
        {
            return (value > 0 && value <= 4);
        }

        public int ParseConfigFileForNumOfInstances(string path)
        {
            // Tries to read until it is able to read (until it's freed from another process).
            while (true)
            {
                if (IsFileReady(path))
                {
                    string s = File.ReadAllText(path);
                    XmlDocument xmld = new XmlDocument();
                    xmld.LoadXml(s);

                    string xpath = "ConfigData/Instances";
                    var nodes = xmld.SelectNodes(xpath);
                    int value = 0;
                    foreach (XmlNode node in nodes)
                    {
                        Int32.TryParse(node.InnerText, out value);
                    }
                    return IsValidValue(value) ? value : -1;
                }
            }
        }
    }
}
