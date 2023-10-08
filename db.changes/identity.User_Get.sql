CREATE PROCEDURE [identity].[User_Get]
AS
SELECT
    u.Id,
    CONCAT(p.Name,' ',p.LastName,' ',p.MotherLastName ,' ') as 'NameComplete',
    a.Name
FROM [identity].[User] u
    INNER JOIN [domain].[ApplicationUser] au ON u.Id = au.UserId and au.[Delete] = 0
    INNER JOIN [domain].[Application] a ON au.ApplicationId = a.Id and a.[Delete] = 0
    INNER JOIN [identity].[Person] p ON u.PersonId = p.Id

