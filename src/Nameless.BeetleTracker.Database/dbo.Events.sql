CREATE TABLE [dbo].[Events]
(
    [AggregateID] UNIQUEIDENTIFIER NOT NULL, 
    [Version] INT NOT NULL, 
    [Payload] VARBINARY(MAX) NOT NULL, 
    [TimeStamp] DATETIMEOFFSET NOT NULL, 
    [EventType] NVARCHAR(MAX) NOT NULL, 
    [Owner] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_Events_Users] FOREIGN KEY ([Owner]) REFERENCES [Users]([Id]) 
)

GO

CREATE INDEX [IX_Events_EventType_Version] ON [dbo].[Events] ([EventType], [Version])

GO

CREATE INDEX [IX_Events_AggregateID] ON [dbo].[Events] ([AggregateID])
