using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RESProject.Classes;
namespace RESProject.Components
{
    public sealed class DumpingBuffer
    {
        private static int CDID = 0;
        private static Dictionary<int, CollectionDescription> CDList = new Dictionary<int, CollectionDescription>();
        private static Dictionary<int, CollectionDescription> CDListDelayed = new Dictionary<int, CollectionDescription>();
        private static int[] counters = { 0, 0, 0, 0, 0 };
        private static Dictionary<int, bool> flags = new Dictionary<int, bool>();
        private static DeltaCD DCD = new DeltaCD();
        private static DumpingBuffer instance = null;
        private static readonly object padlock = new object();
        private static int recieveCounter = 0;
        static DumpingBuffer()
        {
            flags = new Dictionary<int, bool>() { { 1, false }, { 2, false }, { 3, false }, { 4, false }, { 5, false } };
            CDList = new Dictionary<int, CollectionDescription>() { { 1, null }, { 2, null }, { 3, null }, { 4, null }, { 5, null } };
            CDListDelayed = new Dictionary<int, CollectionDescription>() { { 1, null }, { 2, null }, { 3, null }, { 4, null }, { 5, null } };
        }
        public static DumpingBuffer Instance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new DumpingBuffer();
                }
                return instance;
            }
        }
        public static bool WriteToHistory(int code, double value)
        {
            bool retval = true;
            if (code < 1 || code > 10)
            {
                return false;
            }
            if(value > 1000 || value < 0)
            {
                return false;
            }
            int dataSet = code % 5 + 1;
            int index = code / 5;
            int index2 = 0;
            if (index == 0)
                index2 = 1;
            

            if (CDList[dataSet] == null)
            {
                //treba napraviti novi collection description zato sto je prvi put stigla vrednost za taj CD
                CDID++;
                Guid g = new Guid();
                CDList[dataSet] = new CollectionDescription(Guid.NewGuid().GetHashCode(), dataSet);
                Logger.Instance().LogEvent("DumpingBuffer", string.Format("New CollectionDescription({0}) added.", dataSet));
                CDList[dataSet].DumpingPropertyCollection.Add(null);
                CDList[dataSet].DumpingPropertyCollection.Add(null);
            }
            
            if (CDList[dataSet].DumpingPropertyCollection[index] != null)
            {
                if (CDList[dataSet].DumpingPropertyCollection[index].Code == (Codes)code)
                {
                    if (CDList[dataSet].DumpingPropertyCollection[index2] == null)
                    {
                        CDList[dataSet].DumpingPropertyCollection[index].DumpingValue = value;   //updatuj vrednost ako je stigla nova vrednost a nije popunjen ceo collection
                        Logger.Instance().LogEvent("DumpingBuffer", string.Format("Updated {0} to a new value({1})", code, value));
                        retval = true;
                    }
                    else
                    {
                        
                        retval = DelaySending(code, value);
                    }
                }
            }
            else //null je, prvi put je dodat pa ga ubacujemo u cd
            {
                CDList[dataSet].DumpingPropertyCollection[index] = new DumpingProperty(code, value);
                Logger.Instance().LogEvent("DumpingBuffer", string.Format("Added new value to CollectionDescription({0})", code));
                counters[CDList[dataSet].DataSet - 1]++;
                retval = true;
            }

            
            recieveCounter++;
            if(recieveCounter % 10 == 0)
            {
                retval = SendToHistorical();
            }
            Logger.Instance().LogEvent("DumpingBuffer", "Writing to history finished");
            return retval;
        }

        public static bool DelaySending(int code, double value)
        {
            if (code < 1 || code > 10)
            {
                Logger.Instance().LogEvent("DumpingBuffer", "Value not valid");
                return false;
            }
            if (value > 1000 || value < 0)
            {
                Logger.Instance().LogEvent("DumpingBuffer", "Value not valid");
                return false;
            }

            bool retVal = true;
            int dataSet = code % 5 + 1;
            //ako se nalazi vec u kolekciji i treba update-ovati njegovu vrednost
            if (CDListDelayed[dataSet] == null)
            {
                //treba napraviti novi collection description zato sto je prvi put stigla vrednost za taj CD
                CDID++;
                CDListDelayed[dataSet] = new CollectionDescription(CDID, dataSet);
                CDListDelayed[dataSet].DumpingPropertyCollection.Add(null);
                CDListDelayed[dataSet].DumpingPropertyCollection.Add(null);
                Logger.Instance().LogEvent("DumpingBuffer", string.Format("New CollectionDescription({0}) added to delayed collection.", dataSet));
            }

            if (CDListDelayed[dataSet].DumpingPropertyCollection[0] != null)
            {
                if (CDListDelayed[dataSet].DumpingPropertyCollection[0].Code == (Codes)code)
                {
                    if (CDListDelayed[dataSet].DumpingPropertyCollection[1] == null)
                    {
                        CDListDelayed[dataSet].DumpingPropertyCollection[0].DumpingValue = value;   //updatuj vrednost ako je stigla nova vrednost a nije popunjen ceo collection
                        Logger.Instance().LogEvent("DumpingBuffer", string.Format("Updated {0} to a new value({1}) in delayed collection", code, value));
                        retVal = true;
                    }
                }
            }
            else //null je, prvi put je dodat pa ga ubacujemo u cd
            {
                CDListDelayed[dataSet].DumpingPropertyCollection[0] = new DumpingProperty(code, value);
                Logger.Instance().LogEvent("DumpingBuffer", string.Format("Added new value to CollectionDescription({0}) in delayed collection", code));
                retVal = true;
            }

            if (CDListDelayed[dataSet].DumpingPropertyCollection[1] != null)
            {
                if (CDListDelayed[dataSet].DumpingPropertyCollection[1].Code == (Codes)code)
                {
                    if (CDListDelayed[dataSet].DumpingPropertyCollection[0] == null)
                    {
                        CDListDelayed[dataSet].DumpingPropertyCollection[1].DumpingValue = value;
                        Logger.Instance().LogEvent("DumpingBuffer", string.Format("Updated {0} to a new value({1}) in delayed collection", code, value));
                        retVal = true;
                    }
                }
            }
            else
            {
                Logger.Instance().LogEvent("DumpingBuffer", string.Format("Added new value to CollectionDescription({0}) in delayed collection", code));
                CDListDelayed[dataSet].DumpingPropertyCollection[1] = new DumpingProperty(code, value);
                retVal = true;
            }


            return retVal;
        }

        public static bool SendToHistorical()
        {
            DeltaCD deltaCD = new DeltaCD();
            deltaCD.TransactionID = Guid.NewGuid().ToString();
            foreach(CollectionDescription cd in CDList.Values)
            {
                if (cd != null)
                {
                    if (flags[cd.DataSet] == false)
                    {
                        if (counters[cd.DataSet - 1] == 2)
                        {
                            deltaCD.Add.Add(cd);
                            flags[cd.DataSet] = true;
                        }
                    }
                    else
                    {
                        if (counters[cd.DataSet - 1] == 2)
                        {
                            deltaCD.Update.Add(cd);
                        }
                    }
                }
            }

            //pozvati metodu iz historicala da upise ovo u fajl
            Historical.Instance().WriteToXML(deltaCD);
            //preuzima one sto smo odlozili i sad opet nastavlja sa radom
            CDList = null;
            for(int i = 0; i < 5; i++)
            {
                counters[i] = 0;
            }
            foreach(CollectionDescription cd in CDListDelayed.Values)
            {
                if(cd != null)
                    if(cd.DumpingPropertyCollection != null)
                        counters[cd.DataSet-1] = cd.DumpingPropertyCollection.Count();
            }
            CDList = null;
            CDList = CDListDelayed;
            CDListDelayed = new Dictionary<int, CollectionDescription>() { { 1, null }, { 2, null }, { 3, null }, { 4, null }, { 5, null } };
            return true;
        }
    }
}