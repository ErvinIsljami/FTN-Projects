using AssetManagementSystem.DB;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AssetManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow(AMSDevice selectedDevice, long from, long to)
        {
            InitializeComponent();

            AMSDbContext dbContext = new AMSDbContext();
            var listOfMeasurements = dbContext.Measurements.Where(x => x.AMSDeviceId == selectedDevice.Id && x.TimeStamp >= from && x.TimeStamp <= to);
            var sortedList = listOfMeasurements.ToList();
            sortedList.OrderBy(x => x.TimeStamp);
            
            txt_box_measurements.Text = "";

            DateTime dateTimeLowest = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTimeLowest = dateTimeLowest.AddSeconds(sortedList[0].TimeStamp);

            DateTime dateTimeHighest = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTimeHighest = dateTimeHighest.AddSeconds(sortedList[sortedList.Count - 1].TimeStamp);


            txt_box_measurements.Text = $"Number of working hours for device: {selectedDevice.DeviceName} is {(dateTimeHighest - dateTimeLowest).TotalHours}\n";

            foreach (var item in sortedList)
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                dateTime = dateTime.AddSeconds(item.TimeStamp);
                txt_box_measurements.Text += $"Item id: {item.AMSDeviceId}, changed value to {item.Value} in {dateTime}" + "\n";                
            }




        }
    }
}
