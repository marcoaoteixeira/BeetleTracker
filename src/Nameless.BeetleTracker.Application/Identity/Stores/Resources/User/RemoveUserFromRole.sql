DELETE
FROM UsersInRoles
INNER JOIN Roles (NOLOCK) ON Roles.Id = UsersInRoles.RoleId
WHERE
    UsersInRoles.UserId = @UserId
AND Roles.Name = @RoleName;