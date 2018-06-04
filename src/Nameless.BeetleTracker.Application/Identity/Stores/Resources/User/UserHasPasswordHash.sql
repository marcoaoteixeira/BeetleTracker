DECLARE @Result BIT
SELECT
    @Result = 1
FROM Users (NOLOCK)
WHERE
    Id = @Id
AND NOT PasswordHash IS NULL;
SELECT ISNULL(@Result, 0) AS Result;