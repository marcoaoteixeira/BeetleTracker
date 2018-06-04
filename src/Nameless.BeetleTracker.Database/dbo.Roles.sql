CREATE TABLE [dbo].[Roles]
(
    [Id] UNIQUEIDENTIFIER NOT NULL , 
    [Name] NVARCHAR(128) NOT NULL, 
    [State] INT DEFAULT 0 NOT NULL, 
    [Attributes] TEXT NULL, 
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
)

GO

CREATE UNIQUE INDEX [IX_Roles_Name] ON [dbo].[Roles] ([Name])
