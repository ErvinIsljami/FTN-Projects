# gc
College project - automatic garbage collector for C

Project text:
  - It is needed to implement a heap manager that is based on mark-sweep algorithm for automatic memory management.
  - Roots need to be taken from scanning each application thread stack and iterate over the pointers.
  - Heap needs to be segmentally organized.
  - Stress test are needed to measure permeability of allocation and deallocation from 1, 2, 5 and 10 threads.
  - Heap needs to be thread-safe.
  - Period for work is two weeks.
  - Work is done in a team of two.

**NOTES:**
  - Using .cpp files for source files because of Visual C++ compiler.
  - Because of some issues in team communication and other problems the project is not done to the end.
  - OS didn't not allow reading of thread stack locations. So simulation of roots is done.
  - Heap is not segment but fixed to 100MBs (last known size for testing purposes).
  - Hashmap size is also fix due to scaling with heap size.
