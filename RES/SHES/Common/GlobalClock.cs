using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GlobalClock
    {
        private GlobalClock instance;
        private object padlock = new object();
        private int counter = 0;
        private GlobalClock()
        {
            counter = 0;
        }

        public GlobalClock Instance
        {
            get
            {
                lock(padlock)
                {
                    if(instance == null)
                    {
                        instance = new GlobalClock();
                    }
                    return instance;
                }
            }
        }

        public DateTime GetCurrentTime()
        {
            return DateTime.Now.AddSeconds(counter);
        }

        private async void RacunajVreme()
        {
            while(true)
            {
                //ucitati iz xml-a vreme, ovo moze i u konstruktoru da bi se ubrzalo, da ne cita stalno iz xml-a
                int n = 5;  //ovo procitati iz xml-a
                counter += n;

                await Task.Delay(1000);
            }
        }
    }
}
