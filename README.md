# Error-Logger
The system is aimed at software engineers and it will allow them to log error messages in one central place and also provide analysis of the collected data.

The system consists of 3 parts:
1.	A DLL
2.	A Rest Service
3.	A Website

## System Use
- When a developer want to use the error logger, they would reference the library (DLL) in their (host) application. 
- The library would be configured at the application start and it would be used to handle errors in the host application.
	
- When the host application needs to log an error, it would make a call into the library. 
- The library would then send the error message into the REST service. 
- The service would be responsible for logging the error into the database. 

- Finally, when the developer is interested in reviewing logged error messages, they would log into the website and access all of the error   logs that were saved in the DB.
 
#### Data Flow
![Data Flow](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/Data%20Flow.png)

## Running the application

### Note
- Add Connection Strings of your DB in IPFinalProject & LoggerService Web.config files
- Add username, email, password for admin in Startup.cs of IPFinalProject

#### Start 3 projects:
    IPFinalProject - To start the application website
    LoggerService - To start the REST service
    TestClient - DLL to send error logs
    
## Output

#### Login
![Login Page](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/Login.PNG)

#### Admin Home 
![Admin Home Page](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/Admin%20Home.PNG)

#### Admin Applications Details
![Admin Applications Page](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/Admin%20Apps.PNG)

#### Admin Application Logs
![Admin Logs Page](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/Admin%20App%20Logs.PNG)

#### Admin Users Details
![Admin Users Page](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/Admin%20Users.PNG)

#### User Home
![User Home Paage](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/User%20Home.PNG)

#### User Details
![User Details Page](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/User%20Details.PNG)

#### API Home
![API Home Page](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/API%20Home.PNG)

#### API Details
![API Details Page](https://github.com/rahulmaddineni/Error-Logger/blob/master/Screenshots/API%20Details.PNG)
