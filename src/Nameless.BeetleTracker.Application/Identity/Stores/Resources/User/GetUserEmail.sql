SELECT
    Email
FROM Users (NOLOCK)
WHERE
    Id = @Id;