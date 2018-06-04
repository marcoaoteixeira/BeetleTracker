CREATE TABLE [dbo].[UsersInRoles]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [RoleId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [AK_UsersInRoles_UserId_RoleId] UNIQUE ([UserId], [RoleId]), 
    CONSTRAINT [FK_UsersInRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]), 
    CONSTRAINT [FK_UsersInRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id])
)
