# res
**College subject (Development of electroenergetics software)**

Project translation from Serbian Latin to English:

Implementation of a system for writing and reading data of power consumption with optimization for reading using DATA PROXY module.

Often reading data from a database can be an "expensive" process from point of performanse. Data Proxy component is used for caching data in working memory for faster reading. This component moves the "cost" from the processor to the working memory.

In this project assignment the goal is to implement modules for writing and reading achieved power consumption on an hourly basis, so that reading is optimized by using Data Proxy component.

Implementational solution should contain modules as follows:
![Modules](https://i.imgur.com/9kwQFDT.png)

- *DB* - database.
- *DataAccess* - module for accessing the database. Servers for data persistence.
- *DataProxy* - module for caching data for reading.
- *FileReader* - module for reading data from the file system and sending the data for writing to the database.
- *UI* - user interface.

Component *FileReader* reades data about hourly power consumption for one day for a specific geographical area. Data is read from the file system. Files can be either in CSV or XML format, by choice of development team. One file contains data about hourly consumption for one area for one day. Data that are read are the following:
- *Timestamp* (date plus time)
- *Consumption* in mW/h
- *ID* of the geographical area

If there is data for consumption missing for some hour in the day, writing to the database is rejected and information about this is writen to log file.

When data gets to the *FileReader* it is transformed into the model structure that has to be defined by the design of the application. This kind of data is sent to the *DataAccess* component that writes them to the database. *FileReader* also uses the *FileAccess* component for checking if there is already power consumption data for given geographical area and timestamp. If there is, that information is written to log file and writing to database is skipped.

In *UI* component, user gives the date range for which reading is done. This data is sent to the *DataProxy* component. *DataProxy* check if a query for a given date already exists. If not, data are read from the database, written to internal cache structure and sent back to the *UI* component. If a query already exists, data is read from the cache structure, without contacting the database and sent back to the *UI* component. *UI* component shows the given data.

Cached data in *DataProxy* module should exists maximum of 2 hours, after that they are deleted.

**Record of geographical areas**

Geographical area contains the name of the geographical area and a code of that geographical area (shorten name).
Code for geographical area is in CSV files.

Through *UI* it is not needed to record geographical areas. Geographical areas should exist in the database.

**Technical and implementation requirements**
1. In design and architecture of the application it is needed to define possible use-cases, classes, activity of objects of those classes, interaction betweent objects of those classes and software components of the application.

2. Application needs to be in a multi-component architecture. Application has to contain at least these components:
- database
- service layer (optionally there can be a separate layer for database access and business logic layer)
- user interface (console, web or desktop application)

  Layers can communicate between each other directly, i.e. service layer doesn't have to exist on the application server.

  Database can be implemented through some DBMS (MS SQL Server, Oracle), through some embedded systems for database (SQLite, MS Access) or through XML.

3. Servce layer has to be coverd in unit tests. Code coverage has to be at least 60%.

4. Application has to be developed respecting the Agile/Scrum methodologies of development, using TFS.

**Scoring criteria**
- Design and architectural solutions
- Using Scrum methodology of development - defining user stories and tasks, planning and estimation
- Implementation of the solutuion
- Continuous Integration cycle:
  - Build
  - Unit tests
  - Code coverage
