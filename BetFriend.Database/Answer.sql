CREATE TABLE [dbo].[answer]
(
	[bet_id] UNIQUEIDENTIFIER NOT NULL,
	[member_id] UNIQUEIDENTIFIER NOT NULL,
	[is_accepted] BIT NOT NULL,
	[date_answer] DATETIME2 NOT NULL
	PRIMARY KEY (bet_id, member_id, is_accepted), 
    CONSTRAINT [FK_Answer_Bet] FOREIGN KEY ([bet_id]) REFERENCES [bet]([bet_id])
)
