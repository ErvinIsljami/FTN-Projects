using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using static PZ3_NetworkService.MyCommand;

namespace PZ3_NetworkService.ViewModel
{
    public class DataChartViewModel : ValidationBase
    {
        public GeometryGroup GeometryData { get; set; } = new GeometryGroup();
        public List<string> ValveList { get; set; }
        private double height1;
        private double height2;
        private string content1;
        private string content2;
        private int selectedIndex = 0;
        private bool valid = false;
        public MyICommand<TextBlock> ShowCommand { get; set; }
        public MyICommand<Grid> LoadCommand { get; set; }
        public List<Label> Labels { get; set; } = new List<Label>(25);

        public double Height1
        {
            get { return height1; }
            set { height1 = value; OnPropertyChanged("Height1"); }

        }
        public double Height2
        {
            get { return height2; }
            set { height2 = value; OnPropertyChanged("Height2"); }

        }
        
        public string Content1
        {
            get { return content1; }
            set { content1 = value; OnPropertyChanged("Content1"); }

        }

        public string Content2
        {
            get { return content2; }
            set { content2 = value; OnPropertyChanged("Content2"); }

        }

        public int SelectedIndex
        {
            get => selectedIndex;
            set { selectedIndex = value; OnPropertyChanged("SelectedIndex"); }
        }

        public DataChartViewModel()
        {
            ValveList = DataBase.Valve_MainStorage.Values.Select(x=>x.Name).ToList();
            ValveList.Insert(0, "Select Object");
            ShowCommand = new MyICommand<TextBlock>(OnShow);
            LoadCommand = new MyICommand<Grid>(OnLoad);
            ChangeHeight();
        }

        private void ChangeHeight()
        {
            double a = DataBase.IACount;
            double b = DataBase.IBCount;
            double factor = 300 / (a + b);
            Height1 = a * factor;
            Height2 = b * factor;
            Content1 = "IA\n" + Math.Round((a / (a + b) * 100), 2).ToString() + " %";
            Content2 = "IB\n" + Math.Round((b / (a + b) * 100), 2).ToString() + " %";
        }

        protected override void ValidateSelf()
        {
            if (SelectedIndex == 0)
                valid = false;
            else
                valid = true;
        }

        private void OnLoad(Grid MainGrid)
        {
            for(int i=1;i<26;++i)
            {
              Labels.Add((Label)MainGrid.FindName("L" + i.ToString()));
            }
        }

        private void OnShow(TextBlock block)
        {
            this.Validate();
            if(valid)
            {
                block.Text = "";
                DrawDiagram();
            }
            else
            {
                block.Text = "Must Select Object!";
            }

        }

        private void DrawDiagram()
        {
            int counter = 0;
            ValveModel val = null;
            List<Point> list = new List<Point>();
            List<string> dates = new List<string>();
            Task.Delay(1000).ContinueWith(_ => 
            {
                Application.Current.Dispatcher.Invoke(() =>
                {                           
                    if (SelectedIndex != 0)
                    {
                        string obj = ValveList[SelectedIndex];

                        foreach (var item in DataBase.Valve_MainStorage.Values)
                        {
                            if (item.Name == obj)
                            {
                                val = item;
                                break;
                            }
                        }

                        string path = "../../../../log.txt";

                        GeometryData.Children.Clear();

                        string[] allLines;

                        try
                        {
                            if (File.Exists(path))
                            {
                                allLines = System.IO.File.ReadAllLines(path);
                                allLines.Reverse();

                                for (var i = allLines.Length - 1; i > 0; i--)
                                {
                                    string line = allLines[i];

                                    if (allLines[i] != null)
                                    {
                                        if (Int32.TryParse(line.Split(',')[2], out int n))
                                        {
                                            string name = line.Split(',')[3];

                                            if (val.Name == name)
                                            {
                                                string date = line.Split(',')[0];

                                                if (DateTime.Compare(Convert.ToDateTime(date), DateTime.Today) < 0)
                                                {
                                                    continue;
                                                }

                                               
                                                int value = -1;

                                                if (Int32.TryParse(line.Split(',')[4], out int q))
                                                {
                                                    value = Int32.Parse(line.Split(',')[4]);
                                                }

                                                if (GeometryData.Children.Count == 24)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Point p = new Point(counter, value);
                                                    if (++counter == 26)
                                                    {
                                                        GeometryData.Children.RemoveAt(GeometryData.Children.Count - 1);
                                                        dates.RemoveAt(dates.Count - 1);
                                                        counter = 0;
                                                    }
                                                    list.Add(p);
                                                    dates.Add(date);
                                                }
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
                            }
                        }
                        catch {}

                        for (int i = 0; i < list.Count; i++)
                        {
                            list[i] = new Point(30 * list[i].X, 22.5 * list[i].Y);
                        }

                        Point start = list[0];
                        for (int i = 1; i < list.Count; ++i)
                        {
                            GeometryData.Children.Add(new LineGeometry(start, list[i]));
                            Labels[i-1].ToolTip = Convert.ToDateTime(dates[i]).ToLongTimeString();
                            start = list[i];
                        }
                    }
                });
                DrawDiagram();
            });
            

            
        }



    }
}
