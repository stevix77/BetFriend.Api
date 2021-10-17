CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL IDENTITY , 
    [Member_id] UNIQUEIDENTIFIER NOT NULL, 
    [Member_name] VARCHAR(50) NOT NULL, 
    [Wallet] DECIMAL(18, 2) NOT NULL, 
    CONSTRAINT [PK_Member] PRIMARY KEY ([Member_id])
)
