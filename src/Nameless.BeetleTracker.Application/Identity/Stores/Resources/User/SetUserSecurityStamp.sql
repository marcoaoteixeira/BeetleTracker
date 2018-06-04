UPDATE Users SET
    SecurityStamp = @SecurityStamp
WHERE
    Id = @Id;