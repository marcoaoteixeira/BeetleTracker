/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/

DECLARE @UserID UNIQUEIDENTIFIER = '4795d0dc-9885-41c8-bff5-b482ca6be056',
        @RoleID UNIQUEIDENTIFIER = '9da5351c-abc8-4903-b9e6-77b86216e403';

-- Reference data for Users
MERGE INTO [Users] AS Target
USING (VALUES 
    (
        @UserID,                    /* ID */
        N'administrator',           /* Username */
        N'Administrator',           /* FullName */
        0,                          /* AccessFailedCount */
        N'admin@beetletracker.com', /* Email */
        1,                          /* EmailConfirmed */
        0,                          /* LockoutEnabled */
        NULL,                       /* LockoutEndDateUtc */
        N'',                        /* PasswordHash */
        NULL,                       /* PhoneNumber */
        1,                          /* PhoneNumberConfirmed */
        1,                          /* TwoFactorEnabled */
        NULL,                       /* SecurityStamp */
        NULL,                       /* ProfilePicture */
        2,                          /* State */
        NULL                        /* Attributes */
    )
)
AS Source (
    ID,
    Username,
    FullName,
    AccessFailedCount,
    Email,
    EmailConfirmed,
    LockoutEnabled,
    LockoutEndDateUtc,
    PasswordHash,
    PhoneNumber,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    SecurityStamp,
    ProfilePicture,
    State,
    Attributes
)
ON Target.ID = Source.ID
-- Update matched rows 
WHEN MATCHED THEN
UPDATE SET
    Username = Source.Username,
    FullName = Source.FullName,
    AccessFailedCount = Source.AccessFailedCount,
    Email = Source.Email,
    EmailConfirmed = Source.EmailConfirmed,
    LockoutEnabled = Source.LockoutEnabled,
    LockoutEndDateUtc = Source.LockoutEndDateUtc,
    PasswordHash = Source.PasswordHash,
    PhoneNumber = Source.PhoneNumber,
    PhoneNumberConfirmed = Source.PhoneNumberConfirmed,
    TwoFactorEnabled = Source.TwoFactorEnabled,
    SecurityStamp = Source.SecurityStamp,
    ProfilePicture = Source.ProfilePicture,
    State = Source.State,
    Attributes = Source.Attributes
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN
INSERT (
    ID,
    Username,
    FullName,
    AccessFailedCount,
    Email,
    EmailConfirmed,
    LockoutEnabled,
    LockoutEndDateUtc,
    PasswordHash,
    PhoneNumber,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    SecurityStamp,
    ProfilePicture,
    State,
    Attributes
) VALUES (
    ID,
    Username,
    FullName,
    AccessFailedCount,
    Email,
    EmailConfirmed,
    LockoutEnabled,
    LockoutEndDateUtc,
    PasswordHash,
    PhoneNumber,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    SecurityStamp,
    ProfilePicture,
    State,
    Attributes
)
-- delete rows that are in the target but not the source
WHEN NOT MATCHED BY SOURCE THEN
DELETE;

-- Reference data for Roles
MERGE INTO [Roles] AS Target
USING (VALUES 
    (
        @RoleID,            /* ID */
        N'Administrator',   /* Name */
        2,                  /* State */
        NULL                /* Attributes */
    )
)
AS Source (
    ID,
    Name,
    State,
    Attributes
)
ON Target.ID = Source.ID
-- Update matched rows 
WHEN MATCHED THEN
UPDATE SET
    Name = Source.Name,
    State = Source.State,
    Attributes = Source.Attributes
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN
INSERT (
    ID,
    Name,
    State,
    Attributes
) VALUES (
    ID,
    Name,
    State,
    Attributes
)
-- delete rows that are in the target but not the source
WHEN NOT MATCHED BY SOURCE THEN
DELETE;

-- Reference data for UsersInRoles
MERGE INTO [UsersInRoles] AS Target
USING (VALUES 
    (
        @UserID,
        @RoleID
    )
)
AS Source (
    UserId,
    RoleId
)
ON
    Target.UserId = Source.UserId
AND Target.RoleId = Source.RoleId
-- Update matched rows 
WHEN MATCHED THEN
UPDATE SET
    UserId = Source.UserId,
    RoleId = Source.RoleId
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN
INSERT (
    ID,
    Name,
    State,
    Attributes
) VALUES (
    ID,
    Name,
    State,
    Attributes
)
-- delete rows that are in the target but not the source
WHEN NOT MATCHED BY SOURCE THEN
DELETE;