UPDATE Users SET
    LockoutEndDate = @LockoutEndDate
WHERE
    Id = @Id;