// Imenko Prezimenic EE12345

#include <iostream>
#include <vector>
#include <deque>
#include <algorithm>
#include <string>
#include "cv_red.h"

mutex term_m;

class OS
{
private:
    mutex m;
    Cv_red red_sistemskih;
    Cv_red red_korisnickih;
    int aktivanProcess;
	condition_variable cv_rasporedjivac;
	bool rasporedjivac_aktivan;
	public:
		// TODO po potrebi implementirati konstruktor
        OS() : red_sistemskih(0), red_korisnickih(1)
        {
            aktivanProcess = -1;
            rasporedjivac_aktivan = false;
        }
		void izvrsi(int id_procesa, int broj_naredbi, int tip_procesa)
		{
		    if(tip_procesa != 0 && tip_procesa != 1)
            {
                cout<<"Proces "<<id_procesa<<" prekinut zbog neispravnog prioriteta! Prioritet procesa je: "<<tip_procesa<<"."<<endl;
            }

            for(int i = 0; i < broj_naredbi; i++)
            {
                unique_lock<mutex> l(m);

                //bezuslovno zauzimanje
                //ako naidjem na procesor koji je slobodan nema sta da cekam, zauzmem ga
                if(aktivanProcess == -1)
                {
                    aktivanProcess = id_procesa;
                }

                //provera da li je trenutni proces onaj koji se izvrsava
                while(aktivanProcess != id_procesa)
                {
                    if(tip_procesa == 0) //ako je tip procesa nula znaci da sam ja sistemski proces
                    {
                        red_sistemskih.dodaj_u_red(id_procesa, l);  //kao da smo mu rekli wait();
                    }
                    else    //ako nije nula onda je jedan a to je korisnicki proces
                    {
                        red_korisnickih.dodaj_u_red(id_procesa, l);
                    }
                }

                //uspavamo nit na 300ms, uvek se ovako sleepuje
                //simuliramo izvrsavanje jedne istrukcije
                l.unlock();
                this_thread::sleep_for(milliseconds(300));
                l.lock();

                //ako sam izvrsio dve uzastopne naredbe, ili ako je poslednja naredba
                if(i % 2 == 1 || i == broj_naredbi -1)
                {
                    rasporedjivac_aktivan = true;
                    cv_rasporedjivac.notify_one();
                }
                cout<<"Proces "<<id_procesa<<" izvrsio naredbu "<<i<<"."<<endl;
            }
		}

		void rasporedi()
		{
            unique_lock<mutex> l(m);
            while(true)
            {
                while(rasporedjivac_aktivan == false)
                {
                    cv_rasporedjivac.wait(l);
                }
                cout<<"[R] Rasporedjivac aktivan."<<endl;
                //odredi sledeci process

                //prvo proverim da li ima sistemskih
                if(red_sistemskih.prazan() == false)
                {
                    aktivanProcess = red_sistemskih.izbaci_iz_reda();
                    cout<<"[R]Naredni aktivni proces je "<<aktivanProcess<<" prioriteta 0."<<endl;
                }
                else if(red_korisnickih.prazan() == false)
                {
                    aktivanProcess = red_korisnickih.izbaci_iz_reda();
                    cout<<"[R]Naredni aktivni proces je "<<aktivanProcess<<" prioriteta 1."<<endl;
                }
                else
                {
                    aktivanProcess = -1;
                    cout<<"[R]Naredni aktivni proces je -1."<<endl;
                }

                rasporedjivac_aktivan = false;
            }
		}
};

const int BROJ_PROCESA = 5;

void proces(OS& r, int id_procesa, int broj_naredbi, int tip_procesa) {
	char strbafer[100];
	sprintf(strbafer, "Kreira se %s proces [ID: %d, broj naredbi: %d].\n", ((tip_procesa) ? "korisnicki" : "sistemski"), id_procesa, broj_naredbi);
    {
        unique_lock<mutex> l(term_m);
		cout << strbafer;

        if (rand() % 10 > 5) tip_procesa += rand() % 100 - (100 * rand() % 2); // 10% sansi da se "pokvari" prioritet procesa
    }
    r.izvrsi(id_procesa, broj_naredbi, tip_procesa);
    unique_lock<mutex> l(term_m);
    cout << "Proces " << id_procesa << " se zavrsio." << endl;
}

void rasporedjivac(OS &os) {
	while (true) {
		os.rasporedi();
	}
}

int main()
{
    srand(time(NULL));

    OS os;
	thread nit_rasporedjivac(rasporedjivac, ref(os));
	nit_rasporedjivac.detach();

    thread procesi[BROJ_PROCESA];
    for (int i = 0; i < BROJ_PROCESA; i++)
	   procesi[i] = thread(proces, ref(os), i+1, rand() % 10 + 1, (i + 1) % 2);

    for (int i = 0; i < BROJ_PROCESA; i++)
	   procesi[i].join();

	exit(0);
}

