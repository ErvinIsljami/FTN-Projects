using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService
{
	public static class Logger
	{
		private static string filename = "log.txt";

		public static void Log(string entry)
		{
			File.AppendAllText(filename, $"{entry}{Environment.NewLine}");
		}

		public static string[] LoadLog()
		{
			if (File.Exists(filename))
			{
				// It might happen that Log method is called while trying to read the log,
				// that's why we try until we read it
				string[] lines;
				while (true)
				{
					try
					{
						lines = File.ReadAllLines(filename);
						break;
					}
					catch (Exception e)
					{
						continue;
					}
				}
				return lines;
			}
			else
			{
				return new string[0];
			}
		}
	}
}
