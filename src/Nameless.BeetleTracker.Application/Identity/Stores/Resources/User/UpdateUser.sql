UPDATE Users SET
    UserName                = @UserName,
    FullName                = @FullName,
    AccessFailedCount       = @AccessFailedCount,
    Email                   = @Email,
    EmailConfirmed          = @EmailConfirmed,
    LockoutEnabled          = @LockoutEnabled,
    LockoutEndDateUtc       = @LockoutEndDateUtc,
    PasswordHash            = @PasswordHash,
    PhoneNumber             = @PhoneNumber,
    PhoneNumberConfirmed    = @PhoneNumberConfirmed,
    TwoFactorEnabled        = @TwoFactorEnabled,
    SecurityStamp           = @SecurityStamp,
    ProfilePicture          = @ProfilePicture,
    Attributes              = @Attributes,
    State                   = @State
WHERE
    Id = @Id;