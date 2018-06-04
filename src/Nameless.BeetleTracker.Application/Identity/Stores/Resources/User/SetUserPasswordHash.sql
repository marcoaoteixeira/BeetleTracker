UPDATE Users SET
    PasswordHash = @PasswordHash
WHERE
    Id = @Id;