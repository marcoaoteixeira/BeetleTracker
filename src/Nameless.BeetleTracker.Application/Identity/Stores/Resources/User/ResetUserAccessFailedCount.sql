UPDATE Users SET
    AccessFailedCount = 0
WHERE
    Id = @Id;