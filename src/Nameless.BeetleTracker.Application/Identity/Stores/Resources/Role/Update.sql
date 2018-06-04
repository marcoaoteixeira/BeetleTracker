UPDATE Roles SET
    Name = @Name,
    Attributes = @Attributes
WHERE
    Id = @Id;