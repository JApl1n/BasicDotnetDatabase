# Basic Dotnet Database

## Uses Swagger UI to display simple information. The initial setup shown here takes a basic example provided by Dotnet.


### UI with client API:
This is a barebone sproject so the user has access to all the data and can change it however they wantbut has quite a bit on control over the data and that was the intention.
<img src="WholeUI.jpg" alt="UI" width="200"/>

![image](Images/WholeUI.png)
![image](Images/AddItemFunction.png)
The project steps included:
- Add new dotnet webapi
- Add EntityFrameworkCore, EntityFrameworkCore.Sqlite and EntityFrameworkCore.Tools
- Create client program to get and set client data, sets up columns of one row in the table kind of
- Create database context program for talking between C# and SQL, entityframeworks handles this well, also requires editing appsettings.json file and adding the context in Program.cs
- Create database using dotnet-ef for migration which generates SQL commands for our C# code and keeping track of database structure
- Add API controller to give methods for http requests to C# requests
- Replace boilerplate code with client data
- Add validation, logging and DelteAll function
