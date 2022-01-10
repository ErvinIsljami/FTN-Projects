#include <stdio.h>
#include "GC.h"
#include <time.h>

typedef struct Struct_1
{
	int a;
	int* b;
} STRUCT_1;

typedef struct Struct_2
{
	int c;
	STRUCT_1* d;
} STRUCT_2;

typedef struct Struct_1_stressTest
{
	char* p;
	long l1;
	long l2;
	long l3;
	long l4;
}SST1;

typedef struct Struct_2_stressTest
{
	struct Struct_1_stressTest* p1;
}SST2;

typedef struct Struct_3_stressTest
{
	struct Struct_1_stressTest* p1;
	struct Struct_1_stressTest* p2;
	struct Struct_1_stressTest* p3;
	struct Struct_2_stressTest* p4;
	struct Struct_2_stressTest* p5;
	struct Struct_2_stressTest* p6;
}SST3;

typedef struct Struct_4_stressTest
{
	struct Struct_1_stressTest* p1;
	struct Struct_2_stressTest* p2;
	struct Struct_3_stressTest* p3;
}SST4;

typedef struct Struct_5_stressTest
{
	struct Struct_1_stressTest* p1;
	struct Struct_2_stressTest* p2;
	struct Struct_3_stressTest* p3;
	struct Struct_4_stressTest* p4;
}SST5;

typedef struct Struct_6_stressTest
{
	struct Struct_1_stressTest* p1;
	struct Struct_2_stressTest* p2;
	struct Struct_3_stressTest* p3;
	struct Struct_4_stressTest* p4;
	struct Struct_5_stressTest* p5;
	struct Struct_6_stressTest* p6;
	struct Struct_7_stressTest* p7;
}SST6;

typedef struct Struct_7_stressTest
{
	struct Struct_1_stressTest* p1;
	struct Struct_2_stressTest* p2;
	struct Struct_3_stressTest* p3;
	struct Struct_4_stressTest* p4;
	long l1;
	long l2;
	long l3;
	long l4;
}SST7;

typedef struct Struct_8_stressTest
{
	long l1;
	long l2;
	long l3;
	long l4;
	struct Struct_8_stressTest* p1;
}SST8;

typedef struct Struct_9_stressTest
{
	struct Struct_1_stressTest* p1;
	struct Struct_2_stressTest* p2;
	struct Struct_3_stressTest* p3;
	struct Struct_4_stressTest* p4;
	struct Struct_5_stressTest* p5;
	struct Struct_6_stressTest* p6;
	struct Struct_7_stressTest* p7;
	struct Struct_8_stressTest* p8;
	struct Struct_9_stressTest* p9;
} SST9;
DWORD WINAPI ThreadAllocation(LPVOID lpParam);
DWORD WINAPI ThreadAllocationLong(LPVOID lpParam);

int main()
{
	//clock_t start = clock();
	GC* gc = InitializeGC();

	// Because of simulation in main thread we set the roots
	// other mallocs from other threads won't have a pointer in roots
	SST7* array[5];
	for (int i = 0; i < 5; i++)
	{
		SST7* s17 = (SST7*)GC_MALLOC(&gc, sizeof(SST7));
		s17->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p2 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p2->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p2->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3 = (SST3*)GC_MALLOC(&gc, sizeof(SST3));
		s17->p3->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p2 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p3 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p4 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p3->p5 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p3->p6 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p3->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3->p2->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3->p3->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3->p4->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p5->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p6->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4 = (SST4*)GC_MALLOC(&gc, sizeof(SST4));
		s17->p4->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p2 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p4->p3 = (SST3*)GC_MALLOC(&gc, sizeof(SST3));
		s17->p4->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p2->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p2->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p3->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p3->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p3->p2 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p3->p2->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p3->p3 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p3->p3->p = (char*)GC_MALLOC(&gc, sizeof(char));
		array[i] = s17;
		if (i == 0)
		{
			gc->collector->rootCollection[0] = (char*)s17;
		}
		if (i == 1)
		{
			gc->collector->rootCollection[1] = (char*)s17;
		}
		if (i == 2)
		{
			gc->collector->rootCollection[2] = (char*)s17;
		}
		if (i == 3)
		{
			gc->collector->rootCollection[3] = (char*)s17;
		}
	}

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp;
	tfwp.userRoutine = ThreadAllocationLong;
	tfwp.routineParameters = gc;
	HANDLE tH = GC_CREATE_THREAD(&gc, tfwp, NULL, 0, 0, NULL);

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp2;
	tfwp2.userRoutine = ThreadAllocation;
	tfwp2.routineParameters = gc;
	HANDLE tH2 = GC_CREATE_THREAD(&gc, tfwp2, NULL, 0, 0, NULL);

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp3;
	tfwp3.userRoutine = ThreadAllocationLong;
	tfwp3.routineParameters = gc;
	HANDLE tH3 = GC_CREATE_THREAD(&gc, tfwp3, NULL, 0, 0, NULL);

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp4;
	tfwp4.userRoutine = ThreadAllocation;
	tfwp4.routineParameters = gc;
	HANDLE tH4 = GC_CREATE_THREAD(&gc, tfwp4, NULL, 0, 0, NULL);

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp5;
	tfwp5.userRoutine = ThreadAllocationLong;
	tfwp5.routineParameters = gc;
	HANDLE tH5 = GC_CREATE_THREAD(&gc, tfwp5, NULL, 0, 0, NULL);

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp6;
	tfwp6.userRoutine = ThreadAllocation;
	tfwp6.routineParameters = gc;
	HANDLE tH6 = GC_CREATE_THREAD(&gc, tfwp6, NULL, 0, 0, NULL);	

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp7;
	tfwp7.userRoutine = ThreadAllocationLong;
	tfwp7.routineParameters = gc;
	HANDLE tH7 = GC_CREATE_THREAD(&gc, tfwp7, NULL, 0, 0, NULL);

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp8;
	tfwp8.userRoutine = ThreadAllocation;
	tfwp8.routineParameters = gc;
	HANDLE tH8 = GC_CREATE_THREAD(&gc, tfwp8, NULL, 0, 0, NULL);

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp9;
	tfwp9.userRoutine = ThreadAllocationLong;
	tfwp9.routineParameters = gc;
	HANDLE tH9 = GC_CREATE_THREAD(&gc, tfwp9, NULL, 0, 0, NULL);

	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp10;
	tfwp10.userRoutine = ThreadAllocation;
	tfwp10.routineParameters = gc;
	HANDLE tH10 = GC_CREATE_THREAD(&gc, tfwp10, NULL, 0, 0, NULL);

	// Sleep so the directly called Mark&Sweep wouldn't run before each thread malloced what it should
	Sleep(3000);
	gc->collector->MarkAndSweep(&gc->collector, &gc->hManager);

	GC_CLOSE_THREAD_HANDLE(&gc, tH);
	GC_CLOSE_THREAD_HANDLE(&gc, tH2);
	GC_CLOSE_THREAD_HANDLE(&gc, tH3);
	GC_CLOSE_THREAD_HANDLE(&gc, tH4);
	GC_CLOSE_THREAD_HANDLE(&gc, tH5);
	GC_CLOSE_THREAD_HANDLE(&gc, tH6);
	GC_CLOSE_THREAD_HANDLE(&gc, tH7);
	GC_CLOSE_THREAD_HANDLE(&gc, tH8);
	GC_CLOSE_THREAD_HANDLE(&gc, tH9);
	GC_CLOSE_THREAD_HANDLE(&gc, tH10);

	DeinitializeGC(&gc);

	//clock_t end = clock();
	//float res = (float)(end - start) / CLOCKS_PER_SEC;
	return 0;
}

DWORD WINAPI ThreadAllocation(LPVOID lpParam)
{
	SST7* array[5];
	GC* gc = ((GC*)lpParam);

	for (int i = 0; i < 5; i++)
	{
		SST7* s17 = (SST7*)GC_MALLOC(&gc, sizeof(SST7));
		s17->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p2 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p2->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p2->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3 = (SST3*)GC_MALLOC(&gc, sizeof(SST3));
		s17->p3->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p2 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p3 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p4 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p3->p5 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p3->p6 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p3->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3->p2->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3->p3->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3->p4->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p5->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p6->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4 = (SST4*)GC_MALLOC(&gc, sizeof(SST4));
		s17->p4->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p2 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p4->p3 = (SST3*)GC_MALLOC(&gc, sizeof(SST3));
		s17->p4->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p2->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p2->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p3->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p3->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p3->p2 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p3->p2->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p3->p3 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p3->p3->p = (char*)GC_MALLOC(&gc, sizeof(char));
		array[i] = s17;
		if (i == 0)
		{
			gc->collector->rootCollection[0] = (char*)s17;
		}
		if (i == 1)
		{
			gc->collector->rootCollection[1] = (char*)s17;
		}
		if (i == 2)
		{
			gc->collector->rootCollection[2] = (char*)s17;
		}
		if (i == 3)
		{
			gc->collector->rootCollection[3] = (char*)s17;
		}
	}

	return 0;
}

DWORD WINAPI ThreadAllocationLong(LPVOID lpParam)
{
	SST7* array[10];
	GC* gc = ((GC*)lpParam);

	clock_t start = clock();
	for (int i = 0; i < 10; i++)
	{
		SST7* s17 = (SST7*)GC_MALLOC(&gc, sizeof(SST7));
		s17->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p2 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p2->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p2->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3 = (SST3*)GC_MALLOC(&gc, sizeof(SST3));
		s17->p3->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p2 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p3 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p4 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p3->p5 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p3->p6 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p3->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3->p2->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3->p3->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p3->p4->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p5->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p3->p6->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4 = (SST4*)GC_MALLOC(&gc, sizeof(SST4));
		s17->p4->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p2 = (SST2*)GC_MALLOC(&gc, sizeof(SST2));
		s17->p4->p3 = (SST3*)GC_MALLOC(&gc, sizeof(SST3));
		s17->p4->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p2->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p2->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p3->p1 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p3->p1->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p3->p2 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p3->p2->p = (char*)GC_MALLOC(&gc, sizeof(char));
		s17->p4->p3->p3 = (SST1*)GC_MALLOC(&gc, sizeof(SST1));
		s17->p4->p3->p3->p = (char*)GC_MALLOC(&gc, sizeof(char));
		array[i] = s17;
	}

	while (1) {

	}

	return 0;
}