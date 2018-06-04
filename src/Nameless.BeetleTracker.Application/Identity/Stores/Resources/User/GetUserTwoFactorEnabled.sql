SELECT
    TwoFactorEnabled
FROM Users (NOLOCK)
WHERE
    Id = @Id;