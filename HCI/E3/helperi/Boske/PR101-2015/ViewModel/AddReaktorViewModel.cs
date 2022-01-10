using PR101_2015.Commands;
using PR101_2015.Model;
using PR101_2015.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PR101_2015.ViewModel
{
    class AddReaktorViewModel
    {
        public AddReaktorViewModel()
        {
            AddReaktor2Command = new AddReaktor2Command(this);
            AddedReaktor = new Reaktor();
        }
      
        public Window Window { get; set; }

        public Reaktor AddedReaktor { get; set; }
        public String TextBox1Value { get; set; }
        public String TextBox2Value { get; set; }
        public String ComboBoxValue { get; set; }

        public void AddReaktor2()
        {
           
               // if (validate())
                {
                    //if(int.TryParse(textBox1.Text, out value)

                    MainViewModel.Reaktori.Add(new Reaktor(Int32.Parse(TextBox1Value), TextBox2Value, ComboBoxValue));
                    MainViewModel.ReaktoriListView.Add(new Reaktor(Int32.Parse(TextBox1Value), TextBox2Value, ComboBoxValue.ToString()));
                    MainViewModel.ReaktoriFilterLista.Add(new Reaktor(Int32.Parse(TextBox1Value), TextBox2Value, ComboBoxValue.ToString()));
                    MainViewModel.br++;
                }
                //else
                {
                    //MessageBox.Show("Polja nisu dobro popunjena!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            
        }

        public ICommand AddReaktor2Command
        {
            get;
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
