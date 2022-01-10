#ifndef CV_RED_H_INCLUDED
#define CV_RED_H_INCLUDED

#include "cv_hrono.h"

//klasa sa vezbi za prioritete
class Cv_red
{
private:
	int prioritet;
	cv_hrono* cv;
	deque<int> spremni;
public:
		Cv_red(int p): prioritet(p)
		{
			cv = new cv_hrono;
		}
		void dodaj_u_red(int id_procesa, unique_lock<mutex>& l)
		{
            auto it = find(spremni.begin(), spremni.end(), id_procesa);
            if (it == spremni.end())
            {
                spremni.push_back(id_procesa);
            }
			cv->wait(l);
		}
		int izbaci_iz_reda()
		{
			cv->notify_one();
			int id_procesa = spremni.front();
			spremni.pop_front();
			return id_procesa;
		}
		bool prazan()
		{
			return spremni.empty();
		}
};

#endif // CV_RED_H_INCLUDED
