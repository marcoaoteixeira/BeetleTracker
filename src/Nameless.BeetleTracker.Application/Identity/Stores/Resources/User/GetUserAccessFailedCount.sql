SELECT
    ISNULL(AccessFailedCount, 0)
FROM Users (NOLOCK)
WHERE
    Id = @Id;