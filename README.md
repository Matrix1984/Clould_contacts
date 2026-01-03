Server side application.

Requirements:
.net core 10 installed.
Visual studio 2022 (preferably) but possible to just run with the cli.
SQL Server


To create a database the following need to be created.

Creation of an SQL database.

Execution of the following script.


CREATE TABLE [dbo].[Contact] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)  NULL,
    [FullAddress]      NVARCHAR (550) NULL,
    [Email]            NVARCHAR (MAX) NULL,
    [Phone]            NVARCHAR (50)  NULL,
    [Cell]             NVARCHAR (50)  NULL,
    [RegistrationDate] NVARCHAR (50)  NULL,
    [ImagePath]        NVARCHAR (MAX) NULL
);



Change the following in the Configurations file:

// Set the connection string.
  public  const string CONNECTION_STRING = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Contacts;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";

 
// If there is an error in connection. The server may run on a different port. This configuration needs to be adjusted.
  public const string SERVER_HOST = "http://localhost:5000";
