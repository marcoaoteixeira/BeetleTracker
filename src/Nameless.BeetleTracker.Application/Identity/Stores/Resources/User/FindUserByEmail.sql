SELECT
    Id,
    UserName,
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
FROM Users (NOLOCK);