CREATE TABLE [dbo].[Snapshots]
(
    [AggregateID] UNIQUEIDENTIFIER NOT NULL, 
    [Version] INT NOT NULL, 
    [Payload] VARBINARY(MAX) NOT NULL, 
    [SnapshotType] NVARCHAR(MAX) NOT NULL, 
    [Owner] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_Snapshots_Users] FOREIGN KEY ([Owner]) REFERENCES [Users]([Id]) 
)

GO

CREATE INDEX [IX_Snapshots_SnapshotType_Version] ON [dbo].[Snapshots] ([SnapshotType], [Version])
GO

CREATE INDEX [IX_Snapshots_AggregateID] ON [dbo].[Snapshots] ([AggregateID])