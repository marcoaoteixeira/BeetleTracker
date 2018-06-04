SELECT
    ISNULL(LockoutEndDateUtc, '0001-01-01')
FROM Users (NOLOCK)
WHERE
    Id = @Id;