using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using NetworkService.Models;
using NetworkService.ViewModel;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for ChartDataView.xaml
    /// </summary>
    public partial class ChartDataView : UserControl
    {
        public Dictionary<Road, Dictionary<DateTime, int>> ChartRoadsData { get; set; }

        public ChartDataViewModel Vm { get; set; }

        public Dictionary<int, TextBox> ListOfTbs = new Dictionary<int, TextBox>();

        public Dictionary<int, System.Windows.Shapes.Rectangle> ListOfRects = new Dictionary<int, System.Windows.Shapes.Rectangle>();

        public ChartDataView()
        {
            InitializeComponent();
            ChartRoadsData = new Dictionary<Road, Dictionary<DateTime, int>>();
            this.DataContext = new NetworkService.ViewModel.ChartDataViewModel();
            Vm = (ChartDataViewModel)(this.DataContext);

            ListOfTbs.Add(0, Tb1);
            ListOfTbs.Add(1, Tb2);
            ListOfTbs.Add(2, Tb3);
            ListOfTbs.Add(3, Tb4);
            ListOfTbs.Add(4, Tb5);
            ListOfTbs.Add(5, Tb6);
            ListOfTbs.Add(6, Tb7);
            ListOfTbs.Add(7, Tb8);
            ListOfTbs.Add(8, Tb9);
            ListOfTbs.Add(9, Tb10);

            ListOfRects.Add(0, R1);
            ListOfRects.Add(1, R2);
            ListOfRects.Add(2, R3);
            ListOfRects.Add(3, R4);
            ListOfRects.Add(4, R5);
            ListOfRects.Add(5, R6);
            ListOfRects.Add(6, R7);
            ListOfRects.Add(7, R8);
            ListOfRects.Add(8, R9);
            ListOfRects.Add(9, R10);

            NotifiedVms.Instance.RegisterAction(ShowChartAction);
        }

        private void ShowChartAction()
        {
            Dispatcher.Invoke(() =>                                                 // Multiple threads use calls the same action at the same time
            {                                                                       // And here we need to update the UI so we do it in a separate thread
                if (Vm.SelectedRoad != null)
                {
                    Road road = Vm.SelectedRoad;

                    // Path for our log file
                    string path = "log.txt";

                    if (!ChartRoadsData.ContainsKey(road))
                    {
                        ChartRoadsData.Add(road, new Dictionary<DateTime, int>());
                    }
                    else
                    {
                        ChartRoadsData[road].Clear();
                    }

                    string[] allLines;

                    try
                    {
                        if (File.Exists(path))
                        {
                            allLines = System.IO.File.ReadAllLines(path);

                            for (var i = allLines.Length - 1; i > 0; i--)
                            {
                                string line = allLines[i];

                                if (IsValidString(allLines[i]))
                                {
                                    if (Int32.TryParse(line.Split(',')[1], out int n))
                                    {
                                        int id = Int32.Parse(line.Split(',')[1]);

                                        if (road.Id == id)
                                        {
                                            string date = line.Split(',')[0];

                                            if (DateTime.Compare(Convert.ToDateTime(date), DateTime.Today) < 0)
                                            {
                                                continue;
                                            }


                                            int value = -1;

                                            if (Int32.TryParse(line.Split(',')[2].Split(':')[1], out int q))
                                            {
                                                value = Int32.Parse(line.Split(',')[2].Split(':')[1]);
                                            }

                                            if (ChartRoadsData[road].Count == 10)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                ChartRoadsData[road].Add(Convert.ToDateTime(date), value);
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
                    catch (Exception ex)
                    {

                    }

                    int idx = 0;

                    foreach (KeyValuePair<int, TextBox> el in ListOfTbs)
                    {
                        el.Value.Text = "";
                    }

                    foreach (KeyValuePair<int, Rectangle> el in ListOfRects)
                    {
                        el.Value.Height = 0;
                    }

                    foreach (var el in ChartRoadsData[road])
                    {
                        ListOfTbs[idx].Text = el.Key.ToString();
                        ListOfRects[idx].Height = el.Value / 50;
                        idx++;
                    }
                }
            });
        }

        private void ShowChart (object sender, RoutedEventArgs e)
        {
            ShowChartAction();
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
    }
}
