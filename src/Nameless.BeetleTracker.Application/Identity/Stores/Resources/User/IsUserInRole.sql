DECLARE @Result BIT
SELECT
    @Result = 1
FROM Roles (NOLOCK)
INNER JOIN UsersInRoles (NOLOCK) ON UsersInRoles.RoleId = Roles.Id
WHERE
    UsersInRoles.UserId = @UserId
AND Roles.Name = @RoleName;
SELECT ISNULL(@Result, 0) AS Result;