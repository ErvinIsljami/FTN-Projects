using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCompute
{
    public class DllWork : FileWork
    {
        public DllWork() { }

        public string CopyDllToContainerFolder(int numOfContainers, string packetName, int startingContainerIdx, string rootDirectoryPath, string containersPartialDirectoryPath)
        {
            string executingDllSourcePath;
            FileInfo[] allDlls = ReturnAllDlls(packetName, rootDirectoryPath, out executingDllSourcePath);

            if (!String.IsNullOrWhiteSpace(executingDllSourcePath))
            {
                int cnt = 0;
                int i = startingContainerIdx;

                DirectoryInfo containerDirectoryInfo = new DirectoryInfo($"{containersPartialDirectoryPath}{i}");
                if (containerDirectoryInfo.GetFiles().ToArray().Length > 0)
                {
                    foreach (var f in containerDirectoryInfo.GetFiles().ToArray())
                    {
                        if (File.Exists(f.FullName))
                        {
                            File.Delete(f.FullName);
                        }
                    }
                }

                while (cnt < numOfContainers)
                {
                    foreach (FileInfo dll in allDlls)
                    {
                        File.Copy(dll.FullName, $@"{containersPartialDirectoryPath}{i}\{Path.GetFileName(dll.Name)}", true);
                        //File.Copy(dllSourcePath, $@"{containersPartialDirectoryPath}{i}\{Path.GetFileName(dllSourcePath)}", true);
                    }
                    cnt++;
                    i = ((i + 1) == 4) ? 0 : i + 1;
                }
                return $@"{containersPartialDirectoryPath}?\{Path.GetFileName(executingDllSourcePath)}";
            }
            return "";
        }

        public FileInfo[] ReturnDlls(string packetName, string rootDirectoryPath)
        {
            if (CheckIfRootDirectoryContainsPackets(rootDirectoryPath))
            {
                DirectoryInfo rootDirectoryInfo = new DirectoryInfo(rootDirectoryPath);
                DirectoryInfo[] subDirectories = rootDirectoryInfo.GetDirectories(packetName);

                string filter = "*.dll";
                FileInfo[] listOfFiles = subDirectories[0].GetFiles(filter).ToArray();
                return listOfFiles;
            }
            return null;
        }

        public FileInfo[] ReturnAllDlls(string packetName, string rootDirectoryPath, out string executingDllFileName)
        {
            FileInfo[] listOfFiles = ReturnDlls(packetName, rootDirectoryPath);
            executingDllFileName = (listOfFiles.Length > 4) ? "" : ReturnExecutingDllFileName(listOfFiles);
            return listOfFiles;
        }

        public string ReturnExecutingDllFileName(FileInfo[] listOfFiles)
        {
            string result = "";
            foreach (FileInfo file in listOfFiles)
            {
                if (file.Name != "RoleEnvironment.dll" )
                {
                    result = file.FullName;
                    break;
                }
            }
            return result;
        }
        
        public void CopyDllToContainerFolder(string source, string destination)
        {
            if (File.Exists(source))
            {
                File.Copy(source, destination);
            }
        }

        public void CopyAllDllsToNewContainerFolder(string source, string destination)
        {
            DirectoryInfo destinationDirectory = new DirectoryInfo(Path.GetDirectoryName(destination));
            if (destinationDirectory.GetFiles().ToArray().Length > 0)
            {
                foreach (var f in destinationDirectory.GetFiles().ToArray())
                {
                    if (File.Exists(f.FullName))
                    {
                        File.Delete(f.FullName);
                    }
                }
            }

            DirectoryInfo sourceDirectory = new DirectoryInfo(Path.GetDirectoryName(source));
            FileInfo[] sourceFileNames = sourceDirectory.GetFiles("*.dll").ToArray();
            foreach (var f in sourceFileNames)
            {
                File.Copy(f.FullName, Path.Combine(destinationDirectory.FullName, f.Name), true);
            }
        }
    }
}
