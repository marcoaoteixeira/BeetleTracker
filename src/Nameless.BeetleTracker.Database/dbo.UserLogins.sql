CREATE TABLE [dbo].[UserLogins]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [LoginProvider] NVARCHAR(256) NOT NULL, 
    [ProviderKey] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [AK_UserLogins_UserId_LoginProvider] UNIQUE ([UserId], [LoginProvider]), 
    CONSTRAINT [FK_UserLogins_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
)
