using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CloudCompute
{
    public abstract class FileWork
    {
        public bool CheckIfRootDirectoryContainsPackets(string rootDirectoryPath)
        {
            try
            {
                Directory.CreateDirectory(rootDirectoryPath);
                DirectoryInfo rootDirectoryInfo = new DirectoryInfo(rootDirectoryPath);
                DirectoryInfo[] subDirectories = rootDirectoryInfo.GetDirectories();

                if (subDirectories.Length == 0)
                {
                    return false;
                }
                else
                {
                    foreach (var subDirectory in subDirectories)
                    {
                        if (subDirectory.Name.Contains("Packet"))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsFileReady(string filename)
        {
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return inputStream.Length > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
