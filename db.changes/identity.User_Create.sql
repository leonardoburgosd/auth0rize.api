ALTER PROCEDURE [identity].[User_Create]
@UserName VARCHAR(20) ,
@Password VARBINARY(MAX),
@Salt  VARBINARY(MAX),
@Name VARCHAR(100),
@LastName VARCHAR(100),
@MotherLastName VARCHAR(100),
@Birthday DATE,
@Avatar VARCHAR(500),
@RegistrationUser VARCHAR(100)
AS
BEGIN
    DECLARE @PersonId INT
    INSERT INTO [identity].[Person]([Name],LastName,MotherLastName,Birthday,RegistrationUser)
                             VALUES(@Name,@LastName,@MotherLastName,@Birthday,@RegistrationUser)
    SET @PersonId = @@Identity

    DECLARE @TypeUser INT = (SELECT TOP 1 Id FROM [identity].[TypeUser] WHERE Detail = 'Superadmin')

    INSERT INTO [identity].[User](UserName,[Password],[Salt],[IsDoubleFactorActivate],[TypeId],[PersonId],[Avatar],RegistrationUser)
                            VALUES(@UserName,@Password,@Salt,0,@TypeUser,@PersonId,@Avatar,@RegistrationUser)
END
