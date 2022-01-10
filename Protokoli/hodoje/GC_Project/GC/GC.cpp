#include "GC.h"

struct Garbage_collector* InitializeGC()
{
	struct Garbage_collector* gc = NULL;

	if((gc = (struct Garbage_collector*)malloc(sizeof(struct Garbage_collector))) == NULL)
	{
		return NULL;
	}
	gc->collector = CollectorInit();
	gc->hManager = HeapManagerInit();

	return gc;
}

void DeinitializeGC(struct Garbage_collector** garbageCollector)
{
	GC* gc = *garbageCollector;
	H_MANAGER* hM = gc->hManager;
	COLLECTOR* col = gc->collector;

	// FREE FOR HEAP MANAGER
	NODE* temp = NULL;
	NODE* head = hM->listOfFree;
	while (head != NULL)
	{
		temp = head;
		head = head->next;
		free(temp->blockInfo);
		free(temp);
	}
	free(hM->heapStart);
	ReleaseSemaphore(hM->mallocSemaphore, 0, 0);
	CloseHandle(hM->mallocSemaphore);
	for (int i = 0; i < hM->hashMapOfOccupied->size; i++)
	{
		HASHMAP_ENTRY* temp;
		HASHMAP_ENTRY* head = hM->hashMapOfOccupied->buckets[i];
		if (head == NULL)
		{
			free(hM->hashMapOfOccupied->buckets[i]);
		}
		else
		{
			while (head != NULL)
			{
				temp = head;
				HEAP_BLOCK* hb = temp->blockInfo;
				free(hb);
				head = head->next;
				free(temp);
			}
		}
	}
	free(hM->hashMapOfOccupied->buckets);
	free(hM->hashMapOfOccupied);
	free(hM);

	// FREE FOR COLLECTOR
	free(col->threadArr);
	free(col);

	// FREE THE GARBAGE COLLECTOR ITSELF
	free(gc);
}

HANDLE GC_CREATE_THREAD(struct Garbage_collector** garbageCollector,
			   THREAD_FUNCTION_WRAPPER_PARAMETERS threadFunctionWrapperParameters,
			    LPSECURITY_ATTRIBUTES	          lpThreadAttributes,
			   SIZE_T							  dwStackSize,
			   DWORD							  dwCreationFlags,
		     LPDWORD							  lpThreadId)
{
	GC* gc = *garbageCollector;
	COLLECTOR* col = gc->collector;

	return CreateThreadWrapper(&col, threadFunctionWrapperParameters, lpThreadAttributes, dwStackSize, dwCreationFlags, lpThreadId);
}

bool GC_CLOSE_THREAD_HANDLE(struct Garbage_collector** garbageCollector, HANDLE tHandle)
{
	GC* gc = *garbageCollector;
	COLLECTOR* col = gc->collector;

	return CloseThreadHandleWrapper(&col, tHandle);
}

void* GC_MALLOC(struct Garbage_collector** garbageCollector, int nbytes)
{
	GC* gc = *garbageCollector;
	H_MANAGER* hM = gc->hManager;

	return hM->malloc(&hM, nbytes);
}