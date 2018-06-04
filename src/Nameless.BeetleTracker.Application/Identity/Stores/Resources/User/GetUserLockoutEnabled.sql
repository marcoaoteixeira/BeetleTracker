DECLARE @Result BIT
SELECT
    @Result = ISNULL(LockoutEnabled, 0)
FROM Users (NOLOCK)
WHERE
    Id = @Id;
SELECT ISNULL(@Result, 0) AS Result;