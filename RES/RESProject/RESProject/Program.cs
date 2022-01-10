using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RESProject.Classes;
using RESProject.Components;

namespace RESProject
{
    class Program
    {
        
        private static Writer writer = new Writer();
        private static Task autoWriteToBuffer = null;
        private static Logger logger = Logger.Instance();
        static void Main(string[] args)
        {
            Reader reader = new Reader();
            Meni();
            Console.WriteLine("Pritisnite enter za izlazak iz programa");
           
            Console.ReadLine();
        }
        static void Meni()
        {
            int izbor = 0;
            do
            {
                
                Console.WriteLine("Izaberite opciju: ");
                Console.WriteLine("1. Manuelno upisivanje u buffer.");
                Console.WriteLine("2. Automatsko upisivanje u buffer.");
                Console.WriteLine("3. Ispis vrednosti iz XML fajla po vremenskom intervalu.");
                Console.WriteLine("4. Izlaz.");
                try
                {
                    izbor = Int32.Parse(Console.ReadLine());
                }
                catch(Exception e)
                {
                    throw new Exception("Input not valid.");
                }
                if(izbor > 4 || izbor < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Probajte ponovo. Pritisnite enter za nazad");
                    Console.ReadLine();
                    continue;
                }
                ExecuteCommand(izbor);
            } while (izbor != 4);
        }
        static void ExecuteCommand(int mode)
        {
            if (mode == 1)
            {
                Console.Clear();

                try
                {
                    writer.ManualWriteToHistory();
                    Logger.Instance().LogEvent("Program", "Initiated ManualWriteToHistory");
                }
                catch (Exception e)
                {
                    Logger.Instance().LogEvent("Program", "ManualWriteToHistory error. Message: " + e.Message);
                }
                Logger.Instance().LogEvent("Program", "ManualWriteToHistory finished.");
            }
            else if (mode == 2)
            {
                if(autoWriteToBuffer != null && autoWriteToBuffer.Status == TaskStatus.RanToCompletion)
                {
                    Console.Clear();
                    Console.WriteLine("Automatsko upisivanje je vec pokrenuto.");
                    Console.WriteLine("Kliknite enter za nazad.");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Console.Clear();
                    autoWriteToBuffer = new Task(() => writer.WriteToDumpingBuffer()); 
                    autoWriteToBuffer.Start();
                    Logger.Instance().LogEvent("Program", "Initiated autoWriteToBuffer.");
                    Console.WriteLine("Pokrenuto je automatsko upisivanje.");
                    Console.WriteLine("Pritisnite enter za nazad.");
                    Console.ReadLine();
                }
            }
            else if(mode == 3)
            {
                Console.WriteLine("Unesite kod za koji zelite da citate vrednosti.");
                Codes code = (Codes)(Int32.Parse(Console.ReadLine()) - 1);
                Console.WriteLine("Unesite pocetno vreme u formatu HH:mm");
                DateTime startInterval = DateTime.Parse(Console.ReadLine());
                Console.WriteLine("Unesite krajnje vreme u formatu HH:mm");
                DateTime endInterval = DateTime.Parse(Console.ReadLine());

                Reader.Instance().ReadInterval(code, startInterval, endInterval);

            }
            
        }
    }
}