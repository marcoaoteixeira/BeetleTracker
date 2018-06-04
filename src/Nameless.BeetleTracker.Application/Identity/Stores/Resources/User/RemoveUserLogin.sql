DELETE
FROM UserLogins
WHERE
    LoginProvider = @LoginProvider
AND ProviderKey = @ProviderKey;