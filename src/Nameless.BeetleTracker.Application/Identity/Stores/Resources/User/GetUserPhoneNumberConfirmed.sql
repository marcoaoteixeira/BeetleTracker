DECLARE @Result BIT
SELECT
    @Result = PhoneNumberConfirmed
FROM Users (NOLOCK)
WHERE
    Id = @Id;
SELECT ISNULL(@Result, 0) AS Result;