SELECT
    PhoneNumber
FROM Users (NOLOCK)
WHERE
    Id = @Id;