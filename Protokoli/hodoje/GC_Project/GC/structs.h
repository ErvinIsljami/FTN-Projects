#pragma once
#include <Windows.h>	// For HANDLE macro

// Represents one element of either occupied or free heap block
// Members:
// void* dataPtr;
// int dataSize;
// bool mark;
typedef struct Heap_block {
	void* dataPtr;
	int dataSize;
	bool mark;
} HEAP_BLOCK;

// Represents one element of list of free and occupied nodes
// Members:
// HEAP_BLOCK blockInfo;
// struct Node* next;
typedef struct Node
{
	HEAP_BLOCK* blockInfo;
	struct Node* next;
} NODE;

// Represents one element of a hashmap
// Members:
// void* key;
// HEAP_BLOCK* blockInfo;
// struct Hashmap_entry* next;
typedef struct Hashmap_entry
{
	void* key;
	HEAP_BLOCK* blockInfo;
	struct Hashmap_entry* next;
} HASHMAP_ENTRY;

// Represents a hashmap
// Members:
// unsigned int size;
// struct Hashmap_entry** buckets;
typedef struct Hashmap
{
	unsigned int size;
	struct Hashmap_entry** buckets;
	/*unsigned int nentries;
	float loadFactor;*/
} HASHMAP;

// Represents and item in Collectors thread collection
// Members:
// void* tHandle;
// char* firstThreadStackAddress;
// int threadStackSize;
typedef struct Thread_collection_element
{
	void* tHandle;
	char* firstThreadStackAddress;
	int threadStackSize;
} THREAD_COLLECTION_ELEMENT;

// Represents a structure that will be used for CreateThreadWrapper function
// Holds the user routine and routine parameters
// Members:
// void* userRoutine;
// void* routineParameters;
typedef struct Thread_function_wrapper_parameters
{
	void* userRoutine;
	void* routineParameters;
} THREAD_FUNCTION_WRAPPER_PARAMETERS;

// This structure is needed because of the partion of "adding new thread handles and getting the thread stack" mechanics
// Members:
// struct Collector* collector;
// int currentThreadCollectionItemIndex;
// THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp;
typedef struct Thread_function_wrapper_parameters_internal_wrapper
{
	struct Collector* collector;
	int currentThreadCollectionItemIndex;
	THREAD_FUNCTION_WRAPPER_PARAMETERS tfwp;
} TFWP_INTERNAL_WRAPPER;

// Represents a part of Garbage Collector for dealing with heap management (allocating and freeing memory)
// Members:
// char* heapStart;
// int heapSize;
// NODE* listOfFree;
// HASHMAP* hashMapOfOccupied;
// void(*free)(struct Heap_manager**, void**);
// void*(*malloc)(struct Heap_manager**, int);
// HANDLE mallocSemaphore;
typedef struct Heap_manager {
	char* heapStart;
	int heapSize;
	NODE* listOfFree;
	HASHMAP* hashMapOfOccupied;
	void(*free)(struct Heap_manager**, void**);
	void*(*malloc)(struct Heap_manager**, int);
	HANDLE mallocSemaphore;
} H_MANAGER;

// Represents a part of Garbage Collector for dealing with the Mark&Sweep algorithm
// Members:
// char* rootCollection[4];
// THREAD_COLLECTION_ELEMENT* threadArr;
// int threadArrFirstFreeIdx;
// int threadArrSize;;
// void(*MarkAndSweep)(struct Collector**, H_MANAGER** hManager);
typedef struct Collector {
	char* rootCollection[4];
	THREAD_COLLECTION_ELEMENT* threadArr;
	int threadArrFirstFreeIdx;
	int threadArrSize;;
	void(*MarkAndSweep)(struct Collector**, H_MANAGER** hManager);
} COLLECTOR;

// Represents a Garbage Collector structure
// Members:
// struct Heap_manager* hManager;
// struct Collector* collector;
typedef struct Garbage_collector
{
	struct Heap_manager* hManager;
	struct Collector* collector;
} GC;