using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Windows.Shapes;
using AssetManagementSystem.DB;
using AssetManagementSystem.Services;
using AssetManagementSystem.Views;
using Common;

namespace AssetManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<AMSDevice> DevicesCollection { get; set; }
        AMSDbContext dbContext = new AMSDbContext();
        public MainWindow()
        {
            InitializeComponent();
            DevicesCollection = new ObservableCollection<AMSDevice>();
            LCToAMSServiceHost svc1 = new LCToAMSServiceHost();
            LDToAMSServiceHost svc2 = new LDToAMSServiceHost();
            svc1.OpenService();
            svc2.OpenService();
            data_grid.ItemsSource = DevicesCollection;
            cmb_box_alarm.ItemsSource = new List<string>() { "Po satima", "Po promenama" };
            dbContext.SaveChanges();
        }

        private void btn_show_statistics_Click(object sender, RoutedEventArgs e)
        {
            if(data_grid.SelectedItem == null)
            {
                MessageBox.Show("Please select device from list");
                return;
            }
            DateTime from;
            DateTime to;
            long fromUnix = 0;
            long toUnix = 0;

            if(!DateTime.TryParse(txt_box_from.Text, out from))
            {
                MessageBox.Show("From date not valid");
                return;
            }
            else
            {
                var dateTime = from;
                var dateTimeOffset = new DateTimeOffset(dateTime);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                fromUnix = unixDateTime;
            }

            if(!DateTime.TryParse(txt_box_to.Text, out to))
            {
                MessageBox.Show("To date not valid");
                return;
            }
            else
            {
                var dateTime = to;
                var dateTimeOffset = new DateTimeOffset(dateTime);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                toUnix = unixDateTime;
            }

            StatisticsWindow statWindow = new StatisticsWindow((AMSDevice)data_grid.SelectedItem, fromUnix, toUnix);
            statWindow.Show();

        }

        private void data_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btn_alarm_Click(object sender, RoutedEventArgs e)
        {
            if(radio_btn_all.IsChecked == true)
            {
                var listOfDevices = dbContext.Devices.ToList();
                int limit;
                Int32.TryParse(txt_box_limit.Text, out limit);
                if(cmb_box_alarm.SelectedItem.ToString() == "Po satima")
                {
                    foreach(var device in listOfDevices)
                    {
                        var listOfChangesets = dbContext.Measurements.Where(x => x.AMSDeviceId == device.Id).OrderBy(a => a.TimeStamp).ToList();
                        long min = listOfChangesets[0].TimeStamp;
                        long max = listOfChangesets[listOfChangesets.Count - 1].TimeStamp;

                        DateTimeConverter converter = new DateTimeConverter();
                        var startWorking = converter.ConvertFromUnix(min);
                        var stopedWorking = converter.ConvertFromUnix(max);
                        device.IsAlarm = (stopedWorking - startWorking).TotalHours > limit;
                    }
                }
                else
                {
                    foreach (var device in listOfDevices)
                    {
                        var listOfChangesets = dbContext.Measurements.Where(x => x.AMSDeviceId == device.Id).OrderBy(a => a.TimeStamp).ToList();
                        
                        device.IsAlarm = listOfChangesets.Count > limit;
                    }
                }

                AlarmWindow alarmWindow = new AlarmWindow(listOfDevices);
                alarmWindow.Show();
            }
            else if(radio_btn_contr.IsChecked == true)
            {
                var listOfDevices = dbContext.Devices.Where(x => x.AMSLCId.ToString() == txt_box_contr_name.Text);
                int limit;
                Int32.TryParse(txt_box_limit.Text, out limit);
                if (cmb_box_alarm.SelectedItem.ToString() == "Po satima")
                {
                    foreach (var device in listOfDevices)
                    {
                        var listOfChangesets = dbContext.Measurements.Where(x => x.AMSDeviceId == device.Id).OrderBy(a => a.TimeStamp).ToList();
                        long min = listOfChangesets[0].TimeStamp;
                        long max = listOfChangesets[listOfChangesets.Count - 1].TimeStamp;

                        DateTimeConverter converter = new DateTimeConverter();
                        var startWorking = converter.ConvertFromUnix(min);
                        var stopedWorking = converter.ConvertFromUnix(max);
                        device.IsAlarm = (stopedWorking - startWorking).TotalHours > limit;
                    }
                }
                else
                {
                    foreach (var device in listOfDevices)
                    {
                        var listOfChangesets = dbContext.Measurements.Where(x => x.AMSDeviceId == device.Id).OrderBy(a => a.TimeStamp).ToList();

                        device.IsAlarm = listOfChangesets.Count > limit;
                    }
                }
                AlarmWindow alarmWindow = new AlarmWindow(listOfDevices.ToList());
                alarmWindow.Show();
            }

            
        }
        

        private void btn_plot_Click(object sender, RoutedEventArgs e)
        {
            if (data_grid.SelectedItem == null)
            {
                MessageBox.Show("Please select device from list");
                return;
            }
            DateTime from;
            DateTime to;
            long fromUnix = 0;
            long toUnix = 0;

            if (!DateTime.TryParse(txt_box_from_Copy.Text, out from))
            {
                MessageBox.Show("From date not valid");
                return;
            }
            else
            {
                var dateTime = from;
                var dateTimeOffset = new DateTimeOffset(dateTime);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                fromUnix = unixDateTime;
            }

            if (!DateTime.TryParse(txt_box_to_Copy.Text, out to))
            {
                MessageBox.Show("To date not valid");
                return;
            }
            else
            {
                var dateTime = to;
                var dateTimeOffset = new DateTimeOffset(dateTime);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                toUnix = unixDateTime;
            }

            ChartWindow chartWindow = new ChartWindow(((AMSDevice)data_grid.SelectedItem).Id, fromUnix, toUnix);
            chartWindow.Show();
        }
    }
}
