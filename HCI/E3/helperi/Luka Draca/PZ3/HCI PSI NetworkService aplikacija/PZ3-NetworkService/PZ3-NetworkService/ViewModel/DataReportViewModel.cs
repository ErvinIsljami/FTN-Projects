using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using static PZ3_NetworkService.MyCommand;

namespace PZ3_NetworkService.ViewModel
{
    public class DataReportViewModel : BindableBase
    {
        private string rText;
        public string RText
        {
            get => rText;
            set
            {
                rText = value;
                OnPropertyChanged("RText");
            }
        }

        public MyICommand<TextBlock> ShowCommand { get; set; }
        
        public DataReportViewModel()
        {
            ShowCommand = new MyICommand<TextBlock>(OnShow);
        }

        private void OnShow(TextBlock tb)
        {
            tb.Text = "";
            string tempReport="";
            string text = DataBase.logtext;
            
            string[] lines = text.Split('\n');
            if (lines.Length != 0 && lines[0] != "")
            {
                int cObjects = 2 * DataBase.Valve_MainStorage.Values.Count();
                int day = DateTime.Now.Day;

                for (int i = 0; i < cObjects; ++i)
                {
                    tempReport += Environment.NewLine + "-- Object_" + i.ToString() + " --" + Environment.NewLine;
                    for (int j = 0; j < lines.Length; ++j)
                    {
                        string[] line = lines[j].Split(',');
                        if (lines[j]!="" && line[2] == i.ToString() && Convert.ToDateTime(line[0]).Day == day)
                        {
                            if(line.Contains("ADD \r") || line.Contains("DELETE \r"))
                            {
                                if (line[5] == "ADD \r")
                                {
                                    tempReport += "\t--> " + line[0] + " ADD: Object" + Environment.NewLine;
                                }
                                else if (line[5] == "DELETE \r")
                                {
                                    tempReport += "\t--> " + line[0] + " DELETE: Object " + Environment.NewLine;
                                }
                            }
                            else
                            {
                                tempReport += "\t--> " + line[0] + " Changed Value: " + line[4] + Environment.NewLine;
                            }
                            
                        }
                    }
                }
                tb.Text=tempReport;
            }
            else
            {
                tb.Foreground = Brushes.Red;
                tb.FontSize = 16;
                tb.Text = $"Log file is Empty!";
            }

        }


    }
}
