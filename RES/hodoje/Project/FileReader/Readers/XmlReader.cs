using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Entities.Models;
using FileReader.Interfaces;
using FileReader.ReadModel;

namespace FileReader.Readers
{
    public class XmlReader : IReader
    {
        public List<PowerConsumptionData> Read(string fileName, out string errorMessage)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<ReadDataType>), new XmlRootAttribute("PROGNOZIRANI_LOAD"));
            List<ReadDataType> listOfData = new List<ReadDataType>();
            List<PowerConsumptionData> returnList = new List<PowerConsumptionData>();
            errorMessage = "";

            if (!String.IsNullOrWhiteSpace(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    try
                    {
                        listOfData = xs.Deserialize(sr) as List<ReadDataType>;
                    }
                    catch (Exception)
                    {
                        listOfData = null;
                    }

                }

                if (listOfData != null)
                {
                    if (listOfData.Count == 24)
                    {
                        if (listOfData.GroupBy(data => data.Hour).Select(x => x.First()).ToList().Count == 24)
                        {
                            bool everyHourSet = true;
                            int missingHour = -1;

                            for (int i = 1; i <= 24; i++)
                            {
                                if (!listOfData.Any(x => x.Hour == i.ToString()))
                                {
                                    everyHourSet = false;
                                    missingHour = i;
                                    break;
                                }
                            }

                            if (everyHourSet)
                            {
                                int hour;
                                double consumption;
                                foreach (ReadDataType data in listOfData)
                                {
                                    Int32.TryParse(data.Hour, out hour);
                                    Double.TryParse(data.Load, out consumption);

                                    if (hour == 24)
                                    {
                                        hour = 0;
                                    }

                                    returnList.Add(new PowerConsumptionData
                                    {
                                        Timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                            DateTime.Now.Day,
                                            hour, 0, 0),
                                        Consumption = consumption,
                                        GeoAreaId = data.GeoAreaId
                                    });
                                }
                            }
                            else
                            {
                                errorMessage =
                                    $"Time: {DateTime.Now}, Message: 'Data for hour '{missingHour}' is missing.'";
                            }
                        }
                        else
                        {
                            errorMessage = $"Time: {DateTime.Now}, Message: 'There aren't 24 distinct values.'";
                        }
                    }
                    else
                    {
                        errorMessage = $"Time: {DateTime.Now}, Message: 'There aren't 24 values.'";
                    }
                }
                else
                {
                    errorMessage = $"Time: {DateTime.Now}, Message: 'Either missing proper root or some other error.'";
                }
            }
            
            return returnList;
        }
    }
}
