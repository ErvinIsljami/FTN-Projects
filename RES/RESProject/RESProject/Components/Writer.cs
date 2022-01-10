using RESProject.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RESProject.Components
{
    public class Writer
    {
        public Writer()
        {

        }
        
        async public void WriteToDumpingBuffer() //treba implementirati kao Task(asynch)
        {
            int code;
            double value;

            while (true)
            {
                Random r = new Random();
                code = r.Next(10);
                value = r.Next(100, 100000) /100;
                //TODO 2
                DumpingBuffer.WriteToHistory(code, value);
                Logger.Instance().LogEvent("Writer", string.Format("write to dumping buffer with code: {0} and value {1}", code, value));
                //Console.ReadLine();
                await Task.Delay(2000);
            }
        }
        public bool ManualWriteToHistory() //treba implementirati da salje direktno historical componenti
        {
            double value = -1;
            Console.WriteLine("Writing Manualy to History");
            Console.WriteLine("Select Code: ");
            Console.WriteLine("1.  CODE_ANALOG");
            Console.WriteLine("2.  CODE_CUSTOM");
            Console.WriteLine("3.  CODE_SINGLENODE");
            Console.WriteLine("4.  CODE_CONSUMER");
            Console.WriteLine("5.  CODE_MOTION");
            Console.WriteLine("6.  CODE_DIGITAL");
            Console.WriteLine("7.  CODE_LIMITSET");
            Console.WriteLine("8.  CODE_MULTIPLENODE");
            Console.WriteLine("9.  CODE_SOURCE");
            Console.WriteLine("10. CODE_SENSOR");
            int code;
            try
            {
                code = Int32.Parse(Console.ReadLine()) - 1;
            }
            catch (Exception e)
            {
                Logger.Instance().LogEvent("Writer", "error while parsing.");
                throw new Exception("Could not parse the input");
            }
            Console.WriteLine("Enter value: ");
            value = Double.Parse(Console.ReadLine());

            return WriteToFile(code, value);
        }
        public bool WriteToFile(int code, double value)
        {
            
            if (code > 9 || code < 0)
            {
                //Logger.Instance().LogEvent("Writer", "Manual writing to history aborted! CODE ERROR");
                return false;
            }
            
            Historical.Instance().ManualWriting((Codes)(code), value);
            Logger.Instance().LogEvent("Writer", "Manual writing finished");
            return true;
        }
    }
} 
