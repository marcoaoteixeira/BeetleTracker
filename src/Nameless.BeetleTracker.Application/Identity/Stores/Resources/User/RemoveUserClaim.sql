DELETE
FROM UserClaims
WHERE
    Type = @Type
AND Value = @Value
AND UserId = @UserId;