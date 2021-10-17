CREATE TABLE [dbo].[Bet]
(
	[id] INT NOT NULL  IDENTITY (1, 1),
	[bet_id] UNIQUEIDENTIFIER NOT NULL, 
    [description] VARCHAR(MAX) NOT NULL,
	[coins] INT NULL,
	[end_date] DATETIME2 NOT NULL,
	[member_id] UNIQUEIDENTIFIER NOT NULL,
	[creation_date] DATETIME2 NOT NULL 
    PRIMARY KEY ([bet_id]), 
    CONSTRAINT [FK_Bet_Member] FOREIGN KEY ([member_id]) REFERENCES [Member]([Member_id]) 
)
