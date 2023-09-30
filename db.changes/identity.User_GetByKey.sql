ALTER PROCEDURE [identity].[User_GetByKey]
    @UserName VARCHAR(20),
    --puede ingresar nombre de usuario o email
    @Key NVARCHAR(256)
AS
BEGIN

    SELECT u.Id,u.UserName ,u.[Password], u.Avatar, u.Salt, u.IsDoubleFactorActivate, tu.Detail, p.Name, p.LastName, p.MotherLastName,ue.Email
    FROM [identity].[User] u
        INNER JOIN [domain].[ApplicationUser] au ON u.Id = au.UserId
        INNER JOIN [domain].[Application] a ON au.ApplicationId = a.Id
        INNER JOIN [identity].[TypeUser] tu ON u.TypeId = tu.Id
        INNER JOIN [identity].[Person] p ON u.PersonId = p.Id
        LEFT JOIN [identity].[UserEmail] ue ON a.Id = ue.UserId
    WHERE   (u.UserName = @UserName OR (ue.Email = @UserName AND ue.IsPrincipal = 1))
        AND a.[Key] = @Key
        AND u.[Delete] = 0
END
