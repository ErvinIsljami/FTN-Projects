# ssps
Faculty subject - Security and Safety in Power Systems

**NOTE: Text may be subjected to change.**

Implement a *BankingService* component that supports two interfaces: *IUserServices* and *IAdminServices*.
<br/>
- *IUserServices* is meant for work with clients that want to register, log in, open an account in the bank, raise credit or execute a transaction (pay/withdraw) on his account. 
<br/>

**NOTE: (this might change)**
  - *IAdminServices* is an interface meant for administrators that create a database for storing all incoming requests and periodically check the obsolescence of incomed requests. Request is considered obsolete if it's in the database longer than some defined period and in that case an administrator removes it.
  
Bank has three sectors:
  - **IT sector for account opening**
  - **Credit sector**
  - **Payment/withdrawal sector**
<br/>

After a request arrives, *Banking Service* passes the request to sectors for processing. It's mandatory to encrypt these request using AES cryptographic algorithm in CBC mode. After the request arrives, it is stored in the database where record about requests are kept and are passed to sectors for processing when they are free(**this might change**). 

Client is notified when their request has been processed, whether it was succcessful or failed.

Authentication between the bank and its sectors is via Windows authentication protocol, and their communication should provide only data integrity (not confidentiality).

Authentication between all clients and the *BankingService* component is done via certificates, and validation is done using chain of trust rule. Additionally, *IUserServices* and *IAdminServies* interface methods are required to be protected according to user group that is defined with an attribute OU within SubjectName value of the certificate of each client.

All actions in the system, starting with authentication, authorization, as well as work on database, are required to be logged within a specific log file inside Windows Event Log.
