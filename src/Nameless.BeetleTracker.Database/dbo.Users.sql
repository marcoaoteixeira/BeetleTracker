CREATE TABLE [dbo].[Users]
(
    [Id] UNIQUEIDENTIFIER NOT NULL, 
    [UserName] NVARCHAR(128) NOT NULL, 
    [FullName] NVARCHAR(256) NOT NULL, 
    [AccessFailedCount] INT NOT NULL DEFAULT 0, 
    [Email] NVARCHAR(512) NOT NULL, 
    [EmailConfirmed] BIT NOT NULL DEFAULT 0, 
    [LockoutEnabled] BIT NOT NULL DEFAULT 0, 
    [LockoutEndDateUtc] DATETIME NULL, 
    [PasswordHash] NVARCHAR(128) NULL, 
    [PhoneNumber] NVARCHAR(32) NULL, 
    [PhoneNumberConfirmed] BIT NOT NULL DEFAULT 0, 
    [TwoFactorEnabled] BIT NOT NULL DEFAULT 0, 
    [SecurityStamp] NVARCHAR(512) NULL, 
    [ProfilePicture] VARBINARY(MAX) NULL, 
    [State] INT NOT NULL DEFAULT 0, 
    [Attributes] TEXT NULL, 
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]), 
    CONSTRAINT [AK_Users_Email] UNIQUE ([Email]) 
)

GO

CREATE INDEX [IX_Users_UserName] ON [dbo].[Users] ([UserName])

GO

CREATE INDEX [IX_Users_FullName] ON [dbo].[Users] ([FullName])

GO

CREATE INDEX [IX_Users_Email] ON [dbo].[Users] ([Email])
