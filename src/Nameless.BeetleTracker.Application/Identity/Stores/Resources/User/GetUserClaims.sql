SELECT
    Id,
    Type,
    Value
FROM UsersClaims (NOLOCK)
WHERE
    UserId = @UserId;