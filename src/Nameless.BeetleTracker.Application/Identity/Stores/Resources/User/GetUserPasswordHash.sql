SELECT
    PasswordHash
FROM Users (NOLOCK)
WHERE
    Id = @Id;