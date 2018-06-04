CREATE TABLE [dbo].[UserClaims]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [Key] NVARCHAR(256) NOT NULL, 
    [Value] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_UserClaims_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]), 
    CONSTRAINT [AK_UserClaims_UserId_Key] UNIQUE ([UserId],[Key])
)
