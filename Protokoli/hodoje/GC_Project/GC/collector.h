#pragma once
#include "structs.h"
#include "hashmap.h"
#include "list.h"
#include <stdio.h>

#ifndef COLLECTOR_H
#define COLLECTOR_H

/**
@brief Resizes the Collectors thread collection if needed.
@param Collector** collector - address of a pointer to Collector
@return returns void
*/
void ScaleThreadCollection(struct Collector** collector);
/**
@brief Suspends all threads in Collectors thread collection
@param Collector** collector - address of a pointer to Collector
@return returns void
*/
void SuspendAllCurrentProcessThreads(struct Collector** collector);
/**
@brief Resumes all threads Suspended by SuspendAllCurrentProcessThreads function
@param Collector** collector - address of a pointer to Collector
@return returns void
*/
void ResumeAllCurrentProcessThreads(struct Collector** collector);
/**
@brief Creates a function wrapper that wraps the user defined routine and its parameters. 
       Updates the Collectors thread colllection with thread stack start address and the stack size.
@param A pointer to a variable to be passed to the thread.
@return returns the result of the user thread function wrapper
*/
DWORD WINAPI ThreadFunctionWrapper(__drv_aliasesMem LPVOID lpParameters);
/**
@brief Creates a thread that will run the thread function wrapper and updates the Collector's thread collection. Calls ScaleThreadCollection if needed.
@param Collector** collector - address of a pointer to Collector
@param THREAD_FUNCTION_WRAPPER_PARAMETERS threadFunctionWrapperParameters:
       Represents a structure that will be used for CreateThreadWrapper function.
	   Holds the user routine and routine parameters.
@param LPSECURITY_ATTRIBUTES lpThreadAttributes:
	   A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle can be inherited by child processes.
	   If lpThreadAttributes is NULL, the handle cannot be inherited.
	   The lpSecurityDescriptor member of the structure specifies a security descriptor for the new thread.
	   If lpThreadAttributes is NULL, the thread gets a default security descriptor.
	   The ACLs in the default security descriptor for a thread come from the primary token of the creator.
@param SIZE_T dwStackSize:
	   The initial size of the stack, in bytes. 
	   The system rounds this value to the nearest page. 
	   If this parameter is zero, the new thread uses the default size for the executable.
@param DWORD dwCreationFlags:
	   The flags that control the creation of the thread.
@param LPDWORD lpThreadId:
	   A pointer to a variable that receives the thread identifier. 
	   If this parameter is NULL, the thread identifier is not returned.
@return returns a HANDLE to newly created thread
*/
HANDLE CreateThreadWrapper(struct Collector**				  collector,
								  THREAD_FUNCTION_WRAPPER_PARAMETERS threadFunctionWrapperParameters,
								  LPSECURITY_ATTRIBUTES	          lpThreadAttributes,
								  SIZE_T							  dwStackSize,
								  DWORD							  dwCreationFlags,
								  LPDWORD							  lpThreadId);
/**
@brief Returns a pointer to the found Collector's thread collection element
@param Collector** collector - address of a pointer to Collector
@param HANDLE tHandle - handle to a thread that needs to be found
@param int* outIndex - out parameter that recieves the index of the found thread collection element
@return returns an item in the Collector's thread collection
*/
THREAD_COLLECTION_ELEMENT* FindThreadCollectionItem(struct Collector** collector, HANDLE tHandle, int* outIndex);
/**
@brief Removes the thread handle from thread collection in Collector and closes the handle
@param Collector** collector - address of a pointer to Collector
@param HANDLE tHandle - handle to a thread that needs to be closed
@return return a bool that tells if CloseHandle was successful
*/
bool CloseThreadHandleWrapper(struct Collector** collector, HANDLE tHandle);
/**
@brief Resizes the thread collection from the Collector
@param Collector** collector - address of a pointer to Collector
@return returns void
*/
void ScaleThreadCollection(struct Collector** collector); // NOT IMPLEMENTED
/**
@brief Gets the collection of roots from need for Mark&Sweep algorithm
@param Collector** collector - address of a pointer to Collector
@param Heap_manager** hManager - address of a pointer to Heap manager
@return returns void
*/
void GetRootCollection(struct Collector** collector, struct Heap_manager** hManager);  // NOT IMPLEMENTED
/**
@brief Recursively marks the children in the root's tree
@param Heap_manager** hManager - address of a pointer to Heap manager
@param char* pseudoRoot - returns the first address of memory block on which a root points to
@param int size - size of the memory block
@return returns void
*/
void MarkChildren(H_MANAGER** hManager, char* pseudoRoot, int size);
/**
@brief Performs the marking phase of the Mark&Sweep algorithm
@param Collector** collector - address of a pointer to Collector
@param Heap_manager** hManager - address of a pointer to Heap manager
@return returns void
*/
void Mark(struct Collector** collector, H_MANAGER** hManager);
//void Mark(struct Collector** collector, H_MANAGER** hManager, char* rootCollection);
/**
@brief Performs the sweeping phase of the Mark&Sweep algorithm
@param Heap_manager** hManager - address of a pointer to Heap manager
@return returns void
*/
void Sweep(H_MANAGER** hManager);
/**
@brief Template method that executes:
	      SuspendAllCurrentProcessThreads
		  Mark
		  Sweep
		  ResumeAllCurrentProcessThreads
@param Collector** collector - address of a pointer to Collector
@param Heap_manager** hManager - address of a pointer to Heap manager
@return returns void
*/
void MarkAndSweep(struct Collector** collector, H_MANAGER** hManager);
//void MarkAndSweep(H_MANAGER** hManager, struct Collector** collector, char* rootSet);
/**
@brief Initializes the Collector
@param no params
@return returns a pointer to a newly allocated Collector
*/
struct Collector* CollectorInit();

#endif