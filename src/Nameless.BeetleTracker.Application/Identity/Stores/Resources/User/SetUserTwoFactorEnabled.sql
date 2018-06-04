UPDATE Users SET
    TwoFactorEnabled = @TwoFactorEnabled
WHERE
    Id = @Id;