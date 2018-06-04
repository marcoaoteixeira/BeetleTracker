UPDATE Users SET
    LockoutEnabled = @LockoutEnabled
WHERE
    Id = @Id;