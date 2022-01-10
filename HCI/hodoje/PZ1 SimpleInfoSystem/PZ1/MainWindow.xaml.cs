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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;    // Namespace koji sadrzi "BindingList"
using Classes;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataIO serializer = new DataIO();

        public static BindingList<Classes.Kalashnikov> Kalashnikovs { get; set; }
        // Provides a generic collection that supports data binding.
        public MainWindow()
        {
            Kalashnikovs = serializer.DeSerializeObject<BindingList<Classes.Kalashnikov>>("kalashnikovs.xml");
            if (Kalashnikovs == null)
            {
                Kalashnikovs = new BindingList<Classes.Kalashnikov>();

            }
            DataContext = this;     // Na nivou prozora i nasledjuje se u child kontrolama
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddWindow newWindow = new AddWindow();
            newWindow.ShowDialog();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            Kalashnikovs.RemoveAt(MainGrid.SelectedIndex);
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            serializer.SerializeObject<BindingList<Classes.Kalashnikov>>(Kalashnikovs, "kalashnikovs.xml");
        }

        private void Read_OnClick(object sender, RoutedEventArgs e)
        {
            ReadWindow newWindow = new ReadWindow((Kalashnikov)MainGrid.SelectedItem);
            newWindow.ShowDialog();
        }

        private void Change_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeWindow newWindow = new ChangeWindow((Kalashnikov)MainGrid.SelectedItem);
            newWindow.ShowDialog();
            MainGrid.Items.Refresh();
        }
    }
}
