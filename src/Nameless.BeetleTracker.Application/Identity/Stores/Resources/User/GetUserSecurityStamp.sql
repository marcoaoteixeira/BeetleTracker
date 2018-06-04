SELECT
    SecurityStamp
FROM Users (NOLOCK)
WHERE
    Id = @Id;