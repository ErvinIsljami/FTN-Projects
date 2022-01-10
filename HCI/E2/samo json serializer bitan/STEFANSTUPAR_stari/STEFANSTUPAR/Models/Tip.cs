using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEFANSTUPAR.Models
{
    public class Tip : INotifyPropertyChanged
    {
        private string oznaka = "";
        private string naziv = "";
        private string opis = "";
        private string slika = "";

        public Tip(String o, String n, String op, String s)
        {
            oznaka = o;
            naziv = n;
            opis = op;
            slika = s;
        }

        public Tip()
        {

        }

        public void setAll(Tip t)
        {
            oznaka = t.oznaka;
            naziv = t.naziv;
            opis = t.opis;
            slika = t.slika;
        }

        public string Oznaka
        {
            get { return oznaka; }
            set
            {
                if (value != oznaka)
                {
                    oznaka = value;
                    OnPropertyChanged("Oznaka");
                }
            }
        }

        public string Naziv
        {
            get
            {
                return naziv;
            }
            set
            {
                if (value != naziv)
                {
                    naziv = value;
                    OnPropertyChanged("Naziv");
                }
            }
        }

        public string Opis
        {
            get
            {
                return opis;
            }
            set
            {
                if (value != opis)
                {
                    opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }

        public String Slika
        {
            get
            {
                return slika;
            }
            set
            {
                if (value != slika)
                    slika = value;
                OnPropertyChanged("Slika");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

        public static implicit operator Tip(string v)
        {
            throw new NotImplementedException();
        }

        public static ObservableCollection<Tip> tipovi = new ObservableCollection<Tip>();

        public static ObservableCollection<Etiketa> Tipovi
        {
            get;
            set;
        }

        public static Tip temp = new Tip();

        public static Tip Temp
        {
            get;
            set;
        }
    }
}
