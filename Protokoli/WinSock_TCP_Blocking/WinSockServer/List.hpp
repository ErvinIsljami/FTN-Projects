#pragma once

typedef struct cvor_st
{
	int val;
	struct cvor_st* sledeci;
}CVOR;

void Init(CVOR** glava);
void Enqueue(CVOR** glava, int podatak);
void StampajListu(CVOR* glava);
void DestroyList(CVOR** glava);
int Dequeue(CVOR** glava);