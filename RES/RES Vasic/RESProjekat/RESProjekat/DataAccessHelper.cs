using RESProjekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProjekat
{
    public sealed class DataAccessHelper
    {
        private static DataAccessHelper instance = null;
        private static readonly object padlock = new object();
        private ObservableCollection<Transformator> transformatori;
        private ObservableCollection<Grupa> grupe;
        private Dictionary<string, ObservableCollection<Transformator>> grupisanaBaza;

        DataAccessHelper()
        {
            transformatori = new ObservableCollection<Transformator>();
            grupe = new ObservableCollection<Grupa>();
            grupisanaBaza = new Dictionary<string, ObservableCollection<Transformator>>();


            //ucitaj iz baze
            Transformator t1 = new Transformator("TS-101", ETipJedinice.SOLAR, 4234, 1000, 10000, ETipKontrole.LOCAL, 300);
            Transformator t2 = new Transformator("TS-102", ETipJedinice.SOLAR, 4234, 1000, 10000, ETipKontrole.LOCAL, 300);
            Transformator t3 = new Transformator("TS-103", ETipJedinice.SOLAR, 4234, 1000, 10000, ETipKontrole.LOCAL, 300);
            Grupa g1 = new Grupa("GR-1");
            Grupa g2 = new Grupa("GR-2");

            Transformatori.Add(t1);
            Transformatori.Add(t2);
            Transformatori.Add(t3);

            Grupe.Add(g1);
            Grupe.Add(g2);

            g1.BrojJedinica = 2;
            g1.MaxProizvodnja = t1.Max + t2.Max;
            g1.TrenutnaProizvodnja = t1.TrenutnaAktivnaSnaga + t2.TrenutnaAktivnaSnaga;

            g2.BrojJedinica = 1;
            g2.MaxProizvodnja = t3.Max;
            g2.TrenutnaProizvodnja = t3.TrenutnaAktivnaSnaga;

            GrupisanaBaza[g1.Kod] = new ObservableCollection<Transformator>();
            GrupisanaBaza[g2.Kod] = new ObservableCollection<Transformator>();

            GrupisanaBaza[g1.Kod].Add(t1);
            GrupisanaBaza[g1.Kod].Add(t2);
            GrupisanaBaza[g2.Kod].Add(t3);
            

        }
        public static DataAccessHelper Instance
        {
            get
            {
                lock(padlock)
                {
                    if (instance == null)
                        instance = new DataAccessHelper();

                    return instance;

                }
            }
        }

        public ObservableCollection<Transformator> Transformatori { get => transformatori; }
        public ObservableCollection<Grupa> Grupe { get => grupe;  }
        public Dictionary<string, ObservableCollection<Transformator>> GrupisanaBaza { get => grupisanaBaza; }

        public bool DodajTransformator(Transformator t, string grupa)
        {
            if(Transformatori.Any(x => x.Kod == t.Kod))
                return false;

            if (!Grupe.Any(x=> x.Kod == grupa))
                return false;

            Transformatori.Add(t);
            GrupisanaBaza[grupa].Add(t);


            return true;
        }

        public bool DodajGrupu(Grupa g)
        {
            if (Grupe.Any(x => x.Kod == g.Kod))
                return false;

            Grupe.Add(g);
            GrupisanaBaza[g.Kod] = new ObservableCollection<Transformator>();
            return true;
        }

    }
}

