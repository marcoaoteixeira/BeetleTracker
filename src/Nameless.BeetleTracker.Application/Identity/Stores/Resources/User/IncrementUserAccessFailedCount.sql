UPDATE Users SET
    AccessFailedCount = ISNULL(AccessFailedCount, 0) + 1
WHERE
    Id = @Id;
SELECT
    AccessFailedCount
FROM Users (NOLOCK)
WHERE
    Id = @Id;