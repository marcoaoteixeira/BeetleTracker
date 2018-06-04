SELECT
    LoginProvider,
    ProviderKey
FROM UsersLogins (NOLOCK)
WHERE
    UserId = @UserId;