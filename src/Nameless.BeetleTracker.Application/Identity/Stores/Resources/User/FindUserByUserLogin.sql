SELECT
    Users.Id,
    Users.UserName,
    Users.FullName,
    Users.AccessFailedCount,
    Users.Email,
    Users.EmailConfirmed,
    Users.LockoutEnabled,
    Users.LockoutEndDateUtc,
    Users.PasswordHash,
    Users.PhoneNumber,
    Users.PhoneNumberConfirmed,
    Users.TwoFactorEnabled,
    Users.SecurityStamp,
    Users.ProfilePicture,
    Users.State,
    Users.Attributes
FROM Users (NOLOCK)
INNER JOIN UsersLogins (NOLOCK) ON UsersLogins.UserId = Users.Id
WHERE
    UsersLogins.ProviderKey = @ProviderKey
AND UsersLogins.LoginProvider = @LoginProvider;