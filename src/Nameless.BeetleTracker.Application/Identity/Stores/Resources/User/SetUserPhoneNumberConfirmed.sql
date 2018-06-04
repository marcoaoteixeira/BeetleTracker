UPDATE Users SET
    PhoneNumberConfirmed = @PhoneNumberConfirmed
WHERE
    Id = @Id;