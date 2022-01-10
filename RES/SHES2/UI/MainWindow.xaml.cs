using Common;
using Common.Communication;
using Common.SHES_Components;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }

            field = value;

            OnPropertyChanged(propertyName);
        }

        private SHES shes;

        public SHES SHES 
        {   
            get
            {
                return shes;
            }
            set
            {
                SetField(ref shes, value);
            }
        }

        private string powerString;

        private SHESToComponentsQueues queues;
        public string PowerString
        {
            get
            {
                return powerString;
            }
            set
            {
                SetField(ref powerString, value);
            }
        }

        public MainWindow()
        {
            queues = new SHESToComponentsQueues();
            SHES = new SHES(queues);
            ShesDbContext dbContext = new ShesDbContext();
            SHES.Batteries.List.AddRange(dbContext.Battery.ToList());
            shes.Consumers.List.AddRange(dbContext.Consumers.ToList());
            shes.SolarPanels.List.AddRange(dbContext.SolarPanels.ToList());

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtBox_solarPanelName.Text))
            {
                MessageBox.Show("Pls enter valid name for solar panel.");
                return;
            }
            int power;
            if(!int.TryParse(txtBox_solarPanelPower.Text, out power))
            {
                MessageBox.Show("Please enter valid power.");
                return;
            }

            ShesDbContext context = new ShesDbContext();
            SolarPanel sp = new SolarPanel(power, txtBox_solarPanelName.Text);
            context.SolarPanels.Add(sp);
            context.SaveChanges();
            MessageBox.Show("Success");
            txtBox_solarPanelName.Text = "";
            txtBox_solarPanelPower.Text = "";
            shes.SolarPanels.List.Add(sp);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBox_BatteryName.Text))
            {
                MessageBox.Show("Pls enter valid name for battery.");
                return;
            }
            int power;
            if (!int.TryParse(txtBox_BatteryPower.Text, out power))
            {
                MessageBox.Show("Please enter valid power.");
                return;
            }
            int capacity = 0;
            if(!int.TryParse(txtBox_BatteryCapacity.Text, out capacity))
            {
                MessageBox.Show("Please enter valid capacity.");
                return;
            }

            Battery b = new Battery(power, capacity, txtBox_BatteryName.Text);
            ShesDbContext context = new ShesDbContext();
            context.Battery.Add(b);
            context.SaveChanges();
            MessageBox.Show("Success");
            txtBox_BatteryPower.Text = "";
            txtBox_BatteryCapacity.Text = "";
            txtBox_BatteryName.Text = "";
            SHES.Batteries.List.Add(b);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBox_ConsumerName.Text))
            {
                MessageBox.Show("Pls enter valid name for consumer.");
                return;
            }

            int power;

            if (!int.TryParse(txtBox_ConsumerPower.Text, out power))
            {
                MessageBox.Show("Please enter valid power.");
                return;
            }

            ShesDbContext context = new ShesDbContext();
            Consumer c = new Consumer(power, txtBox_ConsumerName.Text);
            context.Consumers.Add(c);
            context.SaveChanges();
            SHES.Consumers.List.Add(c);
            MessageBox.Show("Success");
            txtBox_ConsumerPower.Text = "";
            txtBox_ConsumerName.Text = "";
        }

        private void button_Simulate_Click(object sender, RoutedEventArgs e)
        {
            SHES.SolarPanels.SunPower = slider_sunPower.Value / 100;
            Task.Factory.StartNew( () => Simulate());
            button_Simulate.IsEnabled = false;
        }

        public async void Simulate()
        {
            while (true)
            {
                try
                {
                    double diff = SHES.GetDiff();
                    Dispatcher.Invoke(() =>
                    {
                        txtBox_Power.Text = "" + SHES.CurrentPower;
                    });

                    Dispatcher.Invoke(() =>
                    {
                        txtBox_Diff.Text = "" + diff;
                    });
                }
                catch(Exception e)
                {

                }

                await Task.Delay(500);
            }
        }

        private void slider_sunPower_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SHES.SolarPanels.SunPower = slider_sunPower.Value / 100;
            Response r = new Response(shes.SolarPanels.SunPower, "");
            queues.SolarReponses.Enqueue(r);
        }
    }
}
