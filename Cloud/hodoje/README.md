# cc
**College subject (Cloud Computing)**

**NOTE:**
  - Compute's app.config is assigned with keys for which value is an absolute path!
  - The logic is that at one time only a single dll can be executed.
  - This gives us a history of only one dll.
  - Exercise 5 files are in the "Exercise 5" folder.
  
Project stage A translation (originally it's written on Serbian Latin):

In accordance with the scheme (picture 1), implement a Cloud compute service:

1. Create a project with name *ComputeService* that will run a total of 4 container console applications. Console applications
       simulate nodes of one Cloud system. *ComputeService* service has a task to scan a predefined location, to process packets 
       that are made of one .dll and one .xml file and run those .dlls on the container apps. *ComputeService* service does the
       following actions:
- Runs 4 processes of a container app that are implemented as console applications, providing them the port on which
their WCF server will run.
- Periodically checks the predefined for new packets. Predefined location has to be configurable (XML file).
(Which means the path to the predefined locations has to be given in the *ComputeService*'s app.config
- Reads an .xml file from a packet and checks for number of instances(of containers) that it needs to run. If the number
is lower than 1 and greater than 4, it needs to write out a message that the packet's configuration is invalid and 
delete the packet from predefined location.
- It copies the .dll file from the packet on n destinations and engage n containers via WCF service to read the .dll
and run it, by running the *Start* method from interface *IWorkerRole* given in the listing 1. Number of n is given in
step 3.
- Each container has its on WCF server with method *Load* given in the listing 2. After reading the .dll, its being ran 
and checking out, returning back an information about the execution of the .dll (was it ok or if there was an error).
- **NOTE:** All WCF servers are ran on localhost address, but on different ports. For simplicity of the solution, it is
allowed to use predefined ports. Example of ports interval is: 10010 - 10050.

Project stage B and C translation:

In accordance with the scheme (picture 1), implement a Cloud compute service:

2. Create robustness for containers, so that if one results in failure, *ComputeService* service will figure that out and start a new container with client programs (dlls) that failed together with the container. *ComputeService* recovers the containers in the following way:
- Periodically checks the state of containers via WCF communication and *CheckState* method, so the interface from listing 1. is extended with a method given in listing 2.
- If any of the containers failed, recovery is executed by starting a new container and starting the packets that were executed inside that container. Recovery is executed in the following way:
  - If there are free containers that are not executing the client's program, first start a new instance of the client's program and then start a new container.
  - If all of the containers are busy, first start a new container and then load the last executing client program in the newly created container.
 
3. Provide a functionality for *ComputeService* that it can offer information to instances about their brother instances via WCF server, in the following way:
- *ComputeService* contains a WCF server with interface in listing 3. When a client programs are loaded inside of containers, they ask for their address from Compute service using the method *GetAddress*.
- *ComputeService* internally maintains where which service resides. It implements a method from listing 3, *BrotherInstances* in a way that, on a basis of the name of the program and name of the instance itself uniquely identified with it's own address (it's recommended to use only the port as a unique identifier), it finds the rest of the ports reserved for other instances and returns a list of ports as an aswer. 

Project stage D and E:

In accordance with the scheme (picture 1), implement a Cloud compute service:

4. Provide *RoleEnvironment* class via client library.
- For assignment done under c), it's necessary to implement the functionality within a class *RoleEnvironment* given in listing 1. Class should come in a separate library so it could be distributed to users of PaaS environment.
- *Note*: for simplicity of the assignment, client applications will not have more than one WCF server.

5. Migrate a task from Exercises 5 from Microsoft Azure platform to the platform implemented under a, b, c and d. Transactions do not have to implement durability attribute.
