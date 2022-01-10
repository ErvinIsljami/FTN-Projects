using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public sealed class GlobalClock
    {
        private static GlobalClock instance;
        private static object padlock = new object();
        private int counter = 0;
        public int SpeedConst { get; set; }
        private DateTime startingTime;
        public int Second
        { 
            get
            {
                return 1000 / SpeedConst;
            }
        }
        private GlobalClock()
        {
            counter = 0;
            SpeedConst = 2;
            startingTime = DateTime.Now;
            Task.Factory.StartNew(() => RacunajVreme());
        }
        
        public static GlobalClock Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GlobalClock();
                    }
                    return instance;
                }
            }
        }

        public DateTime GetCurrentTime()
        {
            return startingTime.AddSeconds(counter);
        }

        private async void RacunajVreme()
        {
            while (true)
            {
                counter += SpeedConst;

                await Task.Delay(1000);
            }
        }
    }
}
