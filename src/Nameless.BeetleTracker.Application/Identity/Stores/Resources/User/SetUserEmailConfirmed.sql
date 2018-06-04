UPDATE Users SET
    EmailConfirmed = @EmailConfirmed
WHERE
    Id = @Id;