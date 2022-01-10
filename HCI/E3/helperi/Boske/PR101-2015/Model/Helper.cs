using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PR101_2015.Model
{
    public static class Helper
    {

        public static BindingList<Reaktor> Reaktori { get; set; } //povezna lista //za tabelu
        public static BindingList<Reaktor> ReaktoriListView { get; set; } //za listview posto ne treba da nestane iz tabele
        public static BindingList<Reaktor> ReaktoriFilterLista { get; set; } //za filter posebno da ne bi izgubili prave podatke prilikom filriranja
        public static int br = 0; //brojac reaktora u rijecniku
        public static List<string> tipovi = new List<string>() { "FUZIONI", "TERMALNI", "TIP3" };
        public static DateTime datum = new DateTime();

        public static Dictionary<int, Reaktor> ReakDiksn = new Dictionary<int, Reaktor>();  //zbog prihvatanja objekata iz servera,key=rb servera
        public static Dictionary<string, Reaktor> CanvasiDiksn = new Dictionary<string, Reaktor>();      // key = ime canvasa u koji je dodat objekat
        public static Dictionary<string, Canvas> aktuelniCanvas = new Dictionary<string, Canvas>();    // key = ime canvasa, a vrijednost sam taj Canvas    


        //FileStream fileStream = new FileStream("report.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        public static Reaktor aktuelni_reak = new Reaktor(); //aktuelni za obradu
        public static int trenutno = 0; //tr stanje aktuelnog reaktora
        public static Reaktor o;

        public static Picture draggedItem = null; //objekat koji se draguje
        public static bool dragging = false;   //logicka prom

        //public Helper()
        //{
        //    Helper.datum = DateTime.Now.Date;

        //    Helper.Reaktori = new BindingList<Reaktor>();
        //    Helper.ReaktoriListView = new BindingList<Reaktor>();
        //    Helper.ReaktoriFilterLista = new BindingList<Reaktor>();

        //    Reaktor r1 = new Reaktor(0, "r1", "TIP3");
        //    Reaktor r2 = new Reaktor(1, "r2", "FUZIONI");
        //    Reaktor r3 = new Reaktor(2, "r3", "TERMALNI");
        //    Reaktor r4 = new Reaktor(3, "gaga", "TERMALNI");

           
        //    Helper.Reaktori.Add(r1);
        //    Helper.Reaktori.Add(r2);
        //    Helper.Reaktori.Add(r3);
        //    Helper.Reaktori.Add(r4);
        //    Helper.ReaktoriListView.Add(r1);
        //    Helper.ReaktoriListView.Add(r2);
        //    Helper.ReaktoriListView.Add(r3);
        //    Helper.ReaktoriListView.Add(r4);
        //    Helper.ReaktoriFilterLista.Add(r1);
        //    Helper.ReaktoriFilterLista.Add(r2);
        //    Helper.ReaktoriFilterLista.Add(r3);
        //    Helper.ReaktoriFilterLista.Add(r4);
        //    Helper.ReakDiksn.Add(1, r1);
        //    Helper.ReakDiksn.Add(2, r2);
        //    Helper.ReakDiksn.Add(3, r3);
        //    Helper.ReakDiksn.Add(4, r4);

        //    //comboBoxFilter.ItemsSource = tipovi;

        //    Helper.br += 4; //posto smo dodali 3 reaktora,pa da sljedeci bude na indeksu 4
        //}

    }
}
