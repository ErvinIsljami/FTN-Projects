using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window_Save.xaml
    /// </summary>
    public partial class Window_Save : Window
    {
        public Window_Save()
        {
            InitializeComponent();
        }

        private void Click_DA(object sender, RoutedEventArgs e)
        {
            
            
            TextRange t = new TextRange(((MainWindow)Application.Current.MainWindow).rtbEditor.Document.ContentStart, ((MainWindow)Application.Current.MainWindow).rtbEditor.Document.ContentEnd);

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Rtf Files(*.rtf)|*.rtf|Text Files(*.txt)|*.txt|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                //File.WriteAllText(dialog.FileName, fileText);
                FileStream file = new FileStream(dialog.FileName, FileMode.Create);
                t.Save(file, System.Windows.DataFormats.Rtf);
                file.Close();
            }
            
            this.Close();
        }

        private void Click_NE(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
