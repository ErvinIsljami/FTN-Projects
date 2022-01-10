#include <conio.h>
#include <string.h>
#include <math.h>
#include <Windows.h>
#include "Liste.h"
#pragma warning(disable:4996)

DWORD WINAPI StampajKnjigu(LPVOID params)
{
	Knjiga* k = (Knjiga*)params;
	printf("%s \n", k->autor);
}


int main()
{
	Knjiga k = NapraviNovuKnjigu("Knjiga1", "autor1", 56, 32.5);
	DWORD id;
	HANDLE thStampaj = CreateThread(NULL, 0, &StampajKnjigu, &k, 0, &id);

	for (int i = 0; i < 300000; i++)
	{
		int a = 3;
	}

	CloseHandle(thStampaj);

	return 0;
}

