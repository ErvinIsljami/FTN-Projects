#pragma once
#include "heapManager.h"
#include "collector.h"

#ifndef GC_H
#define GC_H

struct Garbage_collector;

/**
@brief Initializes the GC
@params -
@return returns a pointer to the newly initialized GC
 */
struct Garbage_collector* InitializeGC();

/**
@brief Deinitializes the GC which means freeing all the resources
@params Garbage_collector** garbageCollector - pointer to GC
@return returns void
 */
void DeinitializeGC(struct Garbage_collector** garbageCollector);

// HEAP MANAGER FUNCTIONS

/**
@brief Wraps the Malloc function of the GC's Heap manager
@param struct Garbage_collector** garbageCollector - address of a pointer to GC
@param int nbytes - size of the memory block in bytes
@return returns a pointer to newly allocated memory on GC's Heap manager heap
*/
void* GC_MALLOC(struct Garbage_collector** garbageCollector, int nbytes);

// COLLECTOR FUNCTIONS

/**
@brief Creates a thread that will run the thread function wrapper and updates the Collector's thread collection. Calls ScaleThreadCollection if needed.
@param Garbage_collector** garbageCollector - address of a pointer to Garbage_collector
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
HANDLE GC_CREATE_THREAD(struct Garbage_collector** garbageCollector, 
			   THREAD_FUNCTION_WRAPPER_PARAMETERS threadFunctionWrapperParameters,
		       LPSECURITY_ATTRIBUTES	          lpThreadAttributes,
			   SIZE_T							  dwStackSize,
			   DWORD							  dwCreationFlags,
		     LPDWORD							  lpThreadId);

/**
@brief Removes the thread handle from thread collection in Collector and closes the handle
@param Garbage_collector** garbageCollector - address of a pointer to Garbage_collector
@param HANDLE tHandle - handle to a tread that needs to be closed
@return return a bool that tells if CloseHandle was successful
*/
bool GC_CLOSE_THREAD_HANDLE(struct Garbage_collector** garbageCollector, HANDLE tHandle);

#endif // !GC_H
