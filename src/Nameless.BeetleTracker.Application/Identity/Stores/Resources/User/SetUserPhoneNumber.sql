UPDATE Users SET
    PhoneNumber = @PhoneNumber
WHERE
    Id = @Id;