CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL IDENTITY(1, 1), 
    [user_id] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [username] VARCHAR(50) NOT NULL, 
    [email] VARCHAR(50) NOT NULL, 
    [password] VARCHAR(150) NOT NULL, 
    [register_date] DATETIME2 NOT NULL 
)
