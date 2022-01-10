using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEFANSTUPAR.Models
{
    public class Lokal : INotifyPropertyChanged
    {
        private string oznaka = "";
        private string naziv = "";
        private string opis = "";
        private Tip tip = new Tip();
        private string alkohol = "";
        private bool hendikepirani = false;
        private bool pusenje = false;
        private bool rezervacije = false;
        private string cena = "";
        private string kapacitet = "";
        private DateTime datum = DateTime.Today;
        private ObservableCollection<Etiketa> etikete = new ObservableCollection<Etiketa>();
        private string slika = "";

        private double x = -1;
        private double y = -1;
        private string mesto = "";

        public string Oznaka
        {
            get
            {
                return oznaka;
            }
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

        public Tip Tip
        {
            get
            {
                return tip;
            }
            set
            {
                if (value != tip)
                {
                    tip = value;
                    OnPropertyChanged("Tip");
                }
            }
        }

        public string Alkohol
        {
            get
            {
                return alkohol;
            }
            set
            {
                if (value != alkohol)
                {
                    alkohol = value;
                    OnPropertyChanged("Alkohol");
                }
            }
        }

        public bool Hendikepirani
        {
            get
            {
                return hendikepirani;
            }
            set
            {
                if (value != hendikepirani)
                {
                    hendikepirani = value;
                    OnPropertyChanged("Hendikepirani");
                }
            }
        }

        public bool Pusenje
        {
            get
            {
                return pusenje;
            }
            set
            {
                if (value != pusenje)
                {
                    pusenje = value;
                    OnPropertyChanged("Pusenje");
                }
            }
        }

        public bool Rezervacije
        {
            get
            {
                return rezervacije;
            }
            set
            {
                if (value != rezervacije)
                {
                    rezervacije = value;
                    OnPropertyChanged("Rezervacije");
                }
            }
        }

        public string Cena
        {
            get
            {
                return cena;
            }
            set
            {
                if (value != cena)
                {
                    cena = value;
                    OnPropertyChanged("Cena");
                }
            }
        }

        public string Kapacitet
        {
            get
            {
                return kapacitet;
            }
            set
            {
                if (value != kapacitet)
                {
                    cena = value;
                    OnPropertyChanged("Kapacitet");
                }
            }
        }

        public DateTime Datum
        {
            get
            {
                return datum;
            }
            set
            {
                if (value != datum)
                {
                    datum = value;
                    OnPropertyChanged("Datum");
                }
            }
        }

        public ObservableCollection<Etiketa> Etikete
        {
            get
            {
                return etikete;
            }
            set
            {
                if (value != etikete)
                {
                    etikete = value;
                    OnPropertyChanged("Etikete");
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

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (value != x)
                    x = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (value != y)
                    y = value;
                OnPropertyChanged("Y");
            }
        }


        public string Mesto
        {
            get
            {
                return mesto;
            }
            set
            {
                if (value != mesto)
                {
                    mesto = value;
                    OnPropertyChanged("Mesto");
                }
            }
        }


        public Lokal(string o, string n, string op, Tip t, string al, bool h, bool p, string c, string k, DateTime d, ObservableCollection<Etiketa> l, string sl, bool r)
        {

            this.oznaka = o;
            this.naziv = n;
            this.opis = op;
            this.tip = t;
            this.alkohol = al;
            this.hendikepirani = h;
            this.pusenje = p;
            this.cena = c;
            this.kapacitet = k;
            this.datum = d;
            this.etikete = l;
            this.slika = sl;
            
            this.rezervacije = r;


        }


        public void setAll(Lokal l)
        {


            oznaka = l.oznaka;
            naziv = l.naziv;
            opis = l.opis;
            tip = l.tip;
            alkohol = l.alkohol;
            hendikepirani = l.hendikepirani;
            pusenje = l.pusenje;
            rezervacije = l.rezervacije;
            cena = l.cena;
            kapacitet = l.kapacitet;
            datum = l.datum;
            etikete = l.etikete;
            x = l.x;
            y = l.y;
            slika = l.slika;
            mesto = l.mesto;

        }


        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

        public bool addEtiketa(Etiketa e)
        {
            foreach (Etiketa e1 in etikete)
            {
                if (e1.Oznaka == e.Oznaka)
                {
                    etikete.Remove(e);
                    return true;
                }
            }

            return false;
        }

        public static ObservableCollection<Lokal> lokali = new ObservableCollection<Lokal>();

        public static ObservableCollection<Etiketa> Lokali
        {
            get;
            set;
        }

    }
}
