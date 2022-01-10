using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService
{
    public static class Loger
    {
        public static string Path = "../../../../log.txt";
        public static void InitializeLoger()
        {
            if (!File.Exists(Path))
                File.Create(Path);
            else
                File.WriteAllText(Path,"");
        }

        public static void Log(string message)
        {
            File.AppendAllText(Path, message);
        }
    }
}
