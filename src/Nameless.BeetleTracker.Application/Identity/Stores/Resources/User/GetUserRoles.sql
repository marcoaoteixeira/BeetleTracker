SELECT
    Roles.Name
FROM Roles (NOLOCK)
INNER JOIN UsersInRoles (NOLOCK) ON UsersInRoles.RoleId = Roles.Id
WHERE
    UsersInRoles.UserId = @UserId;