CREATE TABLE [dbo].[UserTokens]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [Token] NVARCHAR(512) NOT NULL, 
    [TimeStamp] DATETIME NOT NULL, 
    CONSTRAINT [FK_UserTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
)
