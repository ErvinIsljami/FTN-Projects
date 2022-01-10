# web
College subject - Web development

**Project assignment**
By using technologies and techniques used on lectures and exercises, design a software support for information system of **TAXI SERVICE** according to the following specification.

**Request specification**
It's necessary to realize a Web application for a system that supports a taxi service. This application should use 3 types (roles) of users: customers, dispatchers(admins), drivers. Listed entities are described by the following data:

- User:
  - Username (unique)
  - Password
  - Name
  - Lastname
  - Gender
  - National identification number
  - Phone number
  - Email
  - Role
  - Rides (all types of rides)
- Driver (User):
  - Location
  - Car
- Customer (User)
- Dispatcher (User)
- Location
  - X and Y coordinates
  - Address
- Address
  - in format: Street, street number, city, postal number
  - ![Modules](https://i.imgur.com/tek63wh.png)
- Ride
  - Date and time of the order
  - Start location (location on which the taxi will arrive)
  - Desired type of a car (if not indicated - default value, passenger car, van)
  - Customer for whom is the ride created (only if a customer initiated the ride creation)
  - Destination (location on which a ride is successfully finished, driver is responsible for updating the value when the ride 
    is successfully finished)
  - Dispatcher (a dispatcher that formed or processed the ride, if the driver was the one who accepted the ride, this field is  
    empty)
  - Driver (a driver that accepted the ride or to whom the ride was assigned by a dispatcher)
  - Price
  - Comment (optional, except if a ride was cancelled)
  - Status (created - on wait, formed, processed, accepted, cancelled, failed, successful)
- Car
  - Driver
  - Year of manufacturing
  - Registration number
  - Taxi number (unique mark that each car has in within its taxi service)
  - Car type (taxi services has passanger cars and vans in its service)
- Comment
  - Description
  - Date of posting
  - User that posted the comment
  - Ride on which a comment was leaved
  - Ride rating (value from 1 to 5, 0 value is interpreted as if a customer didn't rate its ride)
- Ride statuses
  - Created - on wait (initial status of a ride when it's created by the customer)
  - Cancelled (ride that was in *Created - on wait* status and then the customer cancelled it for some reason)
  - Formed (initial status of the ride when it's created by the dispatcher
  - Processed (ride that was in *Created - on wait* status and then the dispatcher processed it and assigned it a driver
  - Accepted (ride that was in *Created - on wait* status and the driver self initiatively took it)
  - Failed (ride that was in *Formed*, *Processed* or *Accepted* status, driver for it didn't successfully transported the 
    customer (for example: during the ride the car broke, for some reason the customer didn't enter the car...)
  - Successful (ride that was in *Formed*, *Processed* or *Accepted* status, driver for it successfully transported a customer

Implement the following functionality:
- Registration - unregistered user registers himself on the application by filling out the field that are meant for that and 
  after that he becomes a customer
- Administrators (dispatchers) are programmaticaly read from text file and can't be added later. Drivers can be created only by 
  the dispatchers
- Login - user that is not logged has to log in the system in a way that he enters his username and password for which he is 
  registered. After that, user is logged in and can performs activities that are set by his role.
- All users can see their profiles and change their personal data.
- Driver can change his current location.
- Customer can ask for a ride on his own, in which case the current location and desired car type fields are filled. Default 
  status of a ride is *Created - on wait*.
- Customer can change or cancel (ride status *Cancelled*) its ride as long as the ride is in *Created - on wait* state. If a 
  customer cancels the ride, a form for a comment on the ride is popped. Cancelling the ride is an unavailable functionality 
  for a ride when it's created by the dispatcher.
- Admin (dispatcher) has a possibility to form new rides and possibility of processing rides (for example if an order came by
  phone call or SMS)
  - When a dispatcher forms a ride, some data is defined for that ride:
    - Location on which the taxi is coming
    - Optionally a desired car type is defined
    - A driver (from a list of free drivers) is assigned to that ride
    - A dispatcher that created that ride is set
    - A customer is not defined
  - If a customer is the one that initiated the ride and if some from the drivers didn't took the ride, dispatcher can assign a 
    ride to some of free drivers. Dispatcher that initiated the ride is set to the dispatcher that processed the last ride.
- Driver has a possibility to change the status of the ride, for which he is assigned to, to *Failed* or *Successful* status.
  - If the driver changes the ride status to *Failed*, data for *Destination* and *Price* are not entered. When ride status is
    changed to *Failed*, the driver has to post a comment. Basic data about the ride can't be altered after that ride comes in
    this status.
  -If the driver changes the status of the ride to *Successful*, data about the *Destination* and *Price* of the ride are entered.
   If for a ride there is a defined customer, when the ride ends customer can post a comment about the ride. Basic data about 
   the ride can't be altered after that ride comes in this status.
- While displaying the ride, next to the allowed that, a comment is also displayed on which you can see te username, comment 
  text, ride rating and date of posting.
- In his home page, customer only sees his rides.
- In their home pages, dispatchers see a list of rides on which they are on. Also, they can see all the rides in the system.
- In their home pages, drivers see a list of rides on whichthey are on. Also, they can see all the rides in the system that are 
  in *Created - on wait* status.
- Filtering - User can choose the filtering of the rides by status of the ride.
- Sorting - User can choose sorting by:
  - Date (newest)
  - Rating (from highest to lowest)
- Search - User can search the rides by:
  - Date of order (from, to, from - to)
  - Rating (from, to, from - to)
  - Price (from, to, from - to)
- Search for rides that's available only to the Dispatchers
  - by Name or Lastname of the driver
  - by Name or Lastname of the customer

*** **Depending on the concrete implementation, students can organize given entities or add new entities as they like.** ***
Data about all entities are stored in text files that can be in arbitrary format. For given files, it's necessary to implement writing and reading of those data. For data that have predefined set of possible values (gender, type of car, status...) it's necessary to implement appropriate enumerations.

Data can be persisted in a database, but only if a student is using Git system for version control when working on the project, assistents are not in an obligation to solve problems with working with a database or Git.

**Extra assignment**
- While choosing the location, use OpenLayers maps (http://openlayers.org/) or some alternative (Google maps is not free).
- Admins (Dispatchers) have right to block customers/drivers, this prevents them from further activity. Also they can stop to   
  blockade.
- While choosing the driver, dispatcher has in offer only the 5 closest drivers depending on the location of the customer.
- Drivers have a possibility of sorting rides by distance from their current location.
- For counting the distance use absolute distance between to points in coordinate plain.

**NOTE**: the rest is not of interest for the assignment.
