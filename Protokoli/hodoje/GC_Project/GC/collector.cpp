#include "collector.h"

#define PRAGMA_PACK_FOUR 4
#define POINTER_SIZE 4

void MarkChildren(H_MANAGER** hManager, char* pseudoRoot, int size)
{
	// If size is less than POINTER_SIZE that means a pointer is certainly not stored on that location
	if (size < POINTER_SIZE)
	{
		return;
	}

	H_MANAGER* hM = *hManager;
	HASHMAP_ENTRY* hashmapEntry = NULL;
	int iterations = size / PRAGMA_PACK_FOUR;

	for (int i = 0; i < iterations; i++)
	{
		// Cast to int* to point to 4 bytes, then dereference it to take the int value and cast it to a pointer
		char* child = (char*)(*((int*)pseudoRoot + i));

		// Check if child is in occupied
		if ((hashmapEntry = HashmapGetElement(&hM->hashMapOfOccupied, child)) != NULL)
		{
			// If yes, mark it and go to his children
			hashmapEntry->blockInfo->mark = true;
			MarkChildren(&hM, (char*)hashmapEntry->blockInfo->dataPtr, hashmapEntry->blockInfo->dataSize);
		}
	}
}

void Mark(struct Collector** collector, H_MANAGER** hManager)
{
	H_MANAGER* hM = *hManager;
	COLLECTOR* col = *collector;
	char* heapStartAddress = hM->heapStart;
	char* heapEndAddress = heapStartAddress + hM->heapSize;

	// Simulation of rootCollection
	char* currentRoot = NULL;
	for (int i = 0; i < 4; i++)
	{
		// Mark the root		
		currentRoot = col->rootCollection[i];
		HASHMAP_ENTRY* occupiedHashEntry = HashmapGetElement(&hM->hashMapOfOccupied, currentRoot);
		occupiedHashEntry->blockInfo->mark = true;

		// Check his children and mark if necessary
		MarkChildren(&hM, (char*)occupiedHashEntry->blockInfo->dataPtr, occupiedHashEntry->blockInfo->dataSize);
	}
}

void Sweep(H_MANAGER** hManager)
{
	H_MANAGER* hM = *hManager;
	HASHMAP* htoc = hM->hashMapOfOccupied;
	HASHMAP_ENTRY* entryToSweepOrUnmark = NULL;
	int htocSize = htoc->size;

	// Iterate over all hashmap indexes
	for (int index = 0; index < htocSize; index++)
	{
		// Get the entry if not NULL on that index
		if ((entryToSweepOrUnmark = htoc->buckets[index]) != NULL)
		{
			// If next is not NULL there are more elements on that index that we have to care for
			if (entryToSweepOrUnmark->next != NULL)
			{
				while (entryToSweepOrUnmark != NULL)
				{
					// Save the reference to next because after removing and freeing we won't have the stored information
					HASHMAP_ENTRY* next = entryToSweepOrUnmark->next;

					if (entryToSweepOrUnmark->blockInfo->mark == true)
					{
						entryToSweepOrUnmark->blockInfo->mark = false;
					}
					else
					{
						hM->free(&hM, &entryToSweepOrUnmark->blockInfo->dataPtr);
					}
					entryToSweepOrUnmark = next;
				}
			}
			// If next is NULL, there are no more elements on that index so we can sweep or unmark with ease
			else
			{
				if (entryToSweepOrUnmark->blockInfo->mark == true)
				{
					entryToSweepOrUnmark->blockInfo->mark = false;
				}
				else
				{
					hM->free(&hM, &entryToSweepOrUnmark->blockInfo->dataPtr);
				}
			}
		}
	}
}

void MarkAndSweep(struct Collector** collector, H_MANAGER** hManager)
{
	H_MANAGER* hM = *hManager;
	COLLECTOR* col = *collector;
	SuspendAllCurrentProcessThreads(&col);
	Mark(&col, &hM);
	Sweep(&hM);
	ResumeAllCurrentProcessThreads(&col);
}

void SuspendAllCurrentProcessThreads(struct Collector** collector)
{
	COLLECTOR* col = *collector;

	// Suspend threads in an array
	for (int i = 0; i < col->threadArrSize; i++)
	{
		// Does not break when HANDLE is NULL
		SuspendThread(col->threadArr[i].tHandle);
	}
}

void ResumeAllCurrentProcessThreads(struct Collector** collector)
{
	COLLECTOR* col = *collector;

	// Resume all threads from an array
	for (int i = 0; i < col->threadArrSize; i++)
	{
		// Does not break when HANDLE is NULL
		ResumeThread(col->threadArr[i].tHandle);
	}
}

DWORD WINAPI ThreadFunctionWrapper(__drv_aliasesMem LPVOID lpParameters)
{
	COLLECTOR* col = ((TFWP_INTERNAL_WRAPPER*)lpParameters)->collector;
	int currentThreadCollectionItemIndex = ((TFWP_INTERNAL_WRAPPER*)lpParameters)->currentThreadCollectionItemIndex;
	LPTHREAD_START_ROUTINE userRoutine = (LPTHREAD_START_ROUTINE)((TFWP_INTERNAL_WRAPPER*)lpParameters)->tfwp.userRoutine;
	LPVOID routineParameters = ((TFWP_INTERNAL_WRAPPER*)lpParameters)->tfwp.routineParameters;

	unsigned long lowLimit;
	unsigned long HighLimit;
	GetCurrentThreadStackLimits(&lowLimit, &HighLimit);
	printf("Lowlimit - 0x%X\nHighlimit - 0x%X\n", lowLimit, HighLimit);

	// We will use the address of this variable as a first stack location
	//int firstStackAddressHelper = 0;

	col->threadArr[currentThreadCollectionItemIndex].firstThreadStackAddress = (char*)lowLimit;
	col->threadArr[currentThreadCollectionItemIndex].threadStackSize = 1024 * 1024 
																	 - sizeof(COLLECTOR*) 
																	 - sizeof(int) 
																	 - sizeof(LPTHREAD_START_ROUTINE)
																	 - sizeof(LPVOID);

	free(lpParameters);

	return userRoutine(routineParameters);
}

HANDLE CreateThreadWrapper(struct Collector**				  collector,
						   THREAD_FUNCTION_WRAPPER_PARAMETERS threadFunctionWrapperParameters,
						   LPSECURITY_ATTRIBUTES			  lpThreadAttributes,
						   SIZE_T							  dwStackSize,
						   DWORD							  dwCreationFlags,
						   LPDWORD							  lpThreadId)
{
	COLLECTOR* col = *collector;
	TFWP_INTERNAL_WRAPPER* tfwpac = NULL;

	if((tfwpac = (TFWP_INTERNAL_WRAPPER*)malloc(sizeof(TFWP_INTERNAL_WRAPPER))) == NULL)
	{
		return NULL;
	}

	tfwpac->collector = col;
	int currentThreadCollectionItemIndex = col->threadArrFirstFreeIdx;		
	tfwpac->currentThreadCollectionItemIndex = currentThreadCollectionItemIndex;
	tfwpac->tfwp = threadFunctionWrapperParameters;

	HANDLE tHandle = CreateThread(lpThreadAttributes, dwStackSize, &ThreadFunctionWrapper, (void*)tfwpac, dwCreationFlags, lpThreadId);

	// Part of adding a new item in a thread array is done here where the handle is added to the first free index
	// Second part is done in thread above where the approximate stack base address is added and also the size
	col->threadArr[col->threadArrFirstFreeIdx].tHandle = tHandle;

	// Update the next free index
	// If not possible, resize
	bool shouldResize = true;
	for(int i = 0; i < col->threadArrSize; i++)
	{
		if(col->threadArr[i].tHandle == NULL)
		{
			col->threadArrFirstFreeIdx = i;
			shouldResize = false;
			break;
		}
	}

	if(shouldResize)
	{
		ScaleThreadCollection(&col);
	}

	return tHandle;
}

THREAD_COLLECTION_ELEMENT* FindThreadCollectionItem(struct Collector** collector, HANDLE tHandle, int* outIndex)
{
	COLLECTOR* col = *collector;
	for (int i = 0; i < col->threadArrSize; i++)
	{
		if (col->threadArr[i].tHandle == tHandle)
		{
			*outIndex = i;
			return &col->threadArr[i];
		}
	}
	return NULL;
}

bool CloseThreadHandleWrapper(struct Collector** collector, HANDLE tHandle)
{
	COLLECTOR* col = *collector;
	THREAD_COLLECTION_ELEMENT* tciToClose = NULL;
	int tciIndex = 0;

	tciToClose = FindThreadCollectionItem(&col, tHandle, &tciIndex);
	if (tciToClose != NULL)
	{
		if(CloseHandle(tHandle))
		{
			tciToClose->tHandle = NULL;
			tciToClose->firstThreadStackAddress = NULL;
			tciToClose->threadStackSize = NULL;
			col->threadArrFirstFreeIdx = tciIndex;
			return true;
		}
		else
		{
			return false;
		}		
	}
	return false;
}

void ScaleThreadCollection(struct Collector** collector)
{
	COLLECTOR* col = *collector;
	col->threadArr = (THREAD_COLLECTION_ELEMENT*)realloc(col->threadArr, 2 * col->threadArrSize * sizeof(THREAD_COLLECTION_ELEMENT));
	col->threadArrSize = col->threadArrSize + col->threadArrSize / 2;
	col->threadArrFirstFreeIdx = col->threadArrSize / 2;
}

void GetRootCollection(struct Collector** collector, struct Heap_manager** hManager)
{
	//// SIMULATION
	//H_MANAGER* hM = *hManager;
	//COLLECTOR* col = *collector;
	//char* heapStartAddress = hM->heapStart;
	//char* heapEndAddress = hM->heapStart + hM->heapSize;
	//SuspendAllCurrentProcessThreads(&col);

	//// Iterate through the whole thread array
	//// Not optimal because it's goint to the end of the array
	//// although there might not be stored handles
	//for (int i = 0; i < col->threadArrSize; i++)
	//{
	//	// Iterate the stack
	//	int* stackReader = (int*)(col->threadArr[i].firstThreadStackAddress);
	//	for (int j = 0; j < col->threadArr[i].threadStackSize; j++)
	//	{
	//		/*if (*stackReader >= (int)heapStartAddress && *stackReader <= (int)heapEndAddress)
	//		{
	//			// Add to roots if in range			
	//		}*/
	//		stackReader = stackReader + 1;
	//	}
	//}
}

COLLECTOR* CollectorInit()
{
	COLLECTOR* collector = NULL;

	if((collector = (COLLECTOR*)malloc(sizeof(COLLECTOR))) == NULL)
	{
		return NULL;
	}
	
	if((collector->threadArr = (THREAD_COLLECTION_ELEMENT*)calloc(32, sizeof(THREAD_COLLECTION_ELEMENT))) == NULL)
	{
		return NULL;
	}

	collector->threadArrFirstFreeIdx = 0;
	collector->threadArrSize = 32;
	collector->rootCollection[4];
	collector->MarkAndSweep = MarkAndSweep;
	return collector;
}
