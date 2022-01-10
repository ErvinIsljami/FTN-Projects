using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NetworkService.Models;

namespace NetworkService.ViewModel
{
    public class ReportViewModel : BindableBase, INotify
    {
        public MyICommand ShowCommand { get; set; }

        private string _reportText;

        public ReportViewModel()
        {
            ShowCommand = new MyICommand(OnShow);

            NotifiedVms.Instance.Register(this);
        }

        public string ReportText
        {
            get { return _reportText; }
            set
            {
                if (_reportText != value)
                {
                    _reportText = value;
                    OnPropertyChanged("ReportText");
                }
            }
        }

        private void OnShow()
        {
            // Path for our log file
            string path = "log.txt";

            // Path for our temp file where we will write down log lines that matter (with IDs that are not in deleted roads)
            string tempFile = Path.GetTempFileName();

            // Reset report text
            ReportText = "";

            // Report dictionary that holds a list of strings for each object in Roads where we will store log lines for each object
            Dictionary<int, List<string>> objectLogs = new Dictionary<int, List<string>>();

            try
            {
                // This part will do a check of the log file, omit log data for each object that doesn't exist anymore and override the file
                if (File.Exists(path))
                {
                    string[] readAllLines = System.IO.File.ReadAllLines(path);
                    List<string> writeAllLines = new List<string>();

                    for (int idx = 0; idx < readAllLines.Length; idx++)
                    {
                        string line = readAllLines[idx];

                        if (IsValidString(line))
                        {
                            if (Int32.TryParse(line.Split(',')[1], out int i))
                            {
                                int id = Int32.Parse(line.Split(',')[1]);

                                // If the ID isn't in the deleted roads, than that line will be useful and won't be deleted
                                //if (!RoadsObs.Instance.DeletedRoads.ToList().Exists(o => o.Id == id) &&
                                //    RoadsObs.Instance.Roads.ToList().Exists(o => o.Id == id))
                                //    writeAllLines[idx] = line;
                                if (RoadsObs.Instance.Roads.ToList().Exists(o => o.Id == id))
                                {
                                    writeAllLines.Add(line);
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    System.IO.File.WriteAllLines(tempFile, writeAllLines);

                    // Slower code
                    //using (var sr = new StreamReader(path))
                    //{
                    //    using (var sw = new StreamWriter(tempFile))
                    //    {
                    //        string line;

                    //        while ((line = sr.ReadLine()) != null)
                    //        {
                    //            if (IsValidString(line))
                    //            {
                    //                if (Int32.TryParse(line.Split(',')[1], out int i))
                    //                {
                    //                    int id = Int32.Parse(line.Split(',')[1]);

                    //                    // If the ID isn't in the deleted roads, than that line will be useful and won't be deleted
                    //                    if(!RoadsObs.Instance.DeletedRoads.ToList().Exists(o => o.Id == id))
                    //                        sw.WriteLine(line);
                    //                }
                    //                else
                    //                {
                    //                    continue;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    // We delete the log file
                    File.Delete(path);

                    // We move the temp file where we saved the useful log data and specify the same log file name
                    File.Move(tempFile, path);
                }

                // This part changes the ReportText
                if (File.Exists(path))
                {
                    string[] allLines = System.IO.File.ReadAllLines(path);

                    for (int idx = 0; idx < allLines.Length; idx++)
                    {
                        string line = allLines[idx];

                        if (IsValidString(line))
                        {
                            // Another validation where we check if there will be an "int" that represents some ID
                            if (Int32.TryParse(line.Split(',')[1], out int i))
                            {
                                // Extract date from the string
                                string date = line.Split(',')[0];

                                // Another validation where we check if date from the log file falls in todays' date
                                if (DateTime.Compare(Convert.ToDateTime(date), DateTime.Today) < 0)
                                {
                                    continue;
                                }

                                // Extract the ID from the string
                                int id = Int32.Parse(line.Split(',')[1]);

                                // Extract the value from the string
                                string value = line.Split(',')[2].Split(':')[1];

                                // Final report line that will look something like this: "16.01.2018. 18:54:41,23,Value:13204"
                                string finalWritting = $"\t{date}, CHANGED STATE: {value}";

                                // We check in report dictionary if it contains an ID extracted from a log line
                                if (!objectLogs.ContainsKey(id))
                                {
                                    // If it doesn't contain the ID then we check if the ID is contained in the Roads collection
                                    if (RoadsObs.Instance.Roads.ToDictionary(o => o.Id, o => o.ToString())
                                        .ContainsKey(id))
                                    {
                                        // If it does contain the ID then that means that there is an object with that id in Roads collection,
                                        // so we have to add it to the report dictionary
                                        objectLogs.Add(id, new List<string>());

                                        // And we add the report line to the list of string for that object in report dictionary
                                        objectLogs[id].Add(finalWritting);
                                    }
                                }
                                else
                                {
                                    // If report dictionary contains the ID, then we just add the report line to the existing list of strings for the particular object
                                    objectLogs[id].Add(finalWritting);
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    // Slower code
                    //using (var sr = new StreamReader(path))
                    //{
                    //    string line;

                    //    while ((line = sr.ReadLine()) != null)
                    //    {
                    //        // Valid string looks something like this: "16.01.2018. 18:54:41,23,Value:13204"
                    //        if (IsValidString(line))
                    //        {
                    //            // Another validation where we check if there will be an "int" that represents some ID
                    //            if (Int32.TryParse(line.Split(',')[1], out int i))
                    //            {
                    //                // Extract date from the string
                    //                string date = line.Split(',')[0];

                    //                // Another validation where we check if date from the log file falls in todays' date
                    //                if (DateTime.Compare(Convert.ToDateTime(date), DateTime.Today) < 0)
                    //                {
                    //                    continue;
                    //                }

                    //                // Extract the ID from the string
                    //                int id = Int32.Parse(line.Split(',')[1]);

                    //                // Extract the value from the string
                    //                string value = line.Split(',')[2].Split(':')[1];

                    //                // Final report line that will look something like this: "16.01.2018. 18:54:41,23,Value:13204"
                    //                string finalWritting = $"\t{date}, CHANGED STATE: {value}";

                    //                // We check in report dictionary if it contains an ID extracted from a log line
                    //                if (!objectLogs.ContainsKey(id))
                    //                {
                    //                    // If it doesn't contain the ID then we check if the ID is contained in the Roads collection
                    //                    if (RoadsObs.Instance.Roads.ToDictionary(o => o.Id, o => o.ToString())
                    //                        .ContainsKey(id))
                    //                    {
                    //                        // If it does contain the ID then that means that there is an object with that id in Roads collection,
                    //                        // so we have to add it to the report dictionary
                    //                        objectLogs.Add(id, new List<string>());

                    //                        // And we add the report line to the list of string for that object in report dictionary
                    //                        objectLogs[id].Add(finalWritting);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    // If report dictionary contains the ID, then we just add the report line to the existing list of strings for the particular object
                    //                    objectLogs[id].Add(finalWritting);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                continue;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //}

                    // Just a check so the "DAILY REPORT" doesn't show in report if there are no items
                    if (objectLogs.Count != 0)
                    {
                        ReportText += "DAILY REPORT: \n";
                    }

                    // OrderBy creates a query that returns items in an ordered order
                    // We just assign the result to the same Dictionary
                    objectLogs = objectLogs.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);

                    // Iterate through each object in report dictionary
                    foreach (KeyValuePair<int, List<string>> obj in objectLogs)
                    {
                        // Write down the name of the object
                        ReportText += $"-Object {obj.Key}: \n\n";

                        // Iterate through the list of strings of the current object in report dictionary 
                        foreach (string str in obj.Value)
                        {
                            ReportText += str + '\n';
                        }

                        // Separation line
                        ReportText += "------------------------------------------------------------------------------------------------------------------\n";
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private bool IsValidString(string input)
        {
            bool result = true;

            if (input.Split(',').Length == 0 && input.Split(',').Length != 3)
            {
                result = false;
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                result = false;
            }
            return result;
        }

        public void Notify(Road changedRoad)
        {
            //throw new NotImplementedException();
        }
    }
}
