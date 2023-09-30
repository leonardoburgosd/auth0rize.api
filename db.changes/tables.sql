CREATE TABLE [identity].[Person](
    Id INT IDENTITY PRIMARY KEY,
    [Name] VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    MotherLastName VARCHAR(100) NOT NULL,
    Birthday DATE DEFAULT GETUTCDATE() NOT NULL,
    --Seguimiento
    RegistrationDate DATETIME DEFAULT GETUTCDATE(),
    RegistrationUser VARCHAR(100) NOT NULL,
    ModificationDate DATETIME,
    ModificationUser VARCHAR(100),
    DeleteDate DATETIME,
    DeleteUser VARCHAR(100),
    [Delete] BIT DEFAULT 0
)
GO
CREATE TABLE [identity].[TypeUser](
    Id INT IDENTITY PRIMARY KEY,
    Detail VARCHAR(20) NOT NULL -- superadmin,admin,gdn
)
GO
CREATE TABLE [identity].[User](
    Id INT IDENTITY PRIMARY KEY,
    UserName VARCHAR(20) NOT NULL UNIQUE,
    [Password]  VARBINARY(MAX) NOT NULL,
    [Salt] VARBINARY(MAX) NOT NULL,
    [IsDoubleFactorActivate] BIT DEFAULT 0,
    [TypeId] INT FOREIGN KEY REFERENCES [identity].[TypeUser](Id),
    [PersonId] INT FOREIGN KEY REFERENCES [identity].[Person](Id),
    [Avatar] NVARCHAR(500) NOT NULL,
    --Seguimiento
    RegistrationDate DATETIME DEFAULT GETUTCDATE(),
    RegistrationUser VARCHAR(100) NOT NULL,
    ModificationDate DATETIME,
    ModificationUser VARCHAR(100),
    DeleteDate DATETIME,
    DeleteUser VARCHAR(100),
    [Delete] BIT DEFAULT 0
)
GO
CREATE TABLE [identity].[UserEmail](
    Id INT IDENTITY PRIMARY KEY,
    Email NVARCHAR(40) NOT NULL,
    IsPrincipal BIT NOT NULL,
    UserId INT FOREIGN KEY REFERENCES [identity].[User](Id),
    --Seguimiento
    RegistrationDate DATETIME DEFAULT GETUTCDATE(),
    RegistrationUser VARCHAR(100) NOT NULL,
    ModificationDate DATETIME,
    ModificationUser VARCHAR(100),
    DeleteDate DATETIME,
    DeleteUser VARCHAR(100),
    [Delete] BIT DEFAULT 0
)
GO
CREATE TABLE [domain].[Application](
    Id INT IDENTITY PRIMARY KEY,
    [Key] NVARCHAR(256) NOT NULL,
    [Name] NVARCHAR(20) NOT NULL,
    --Seguimiento
    RegistrationDate DATETIME DEFAULT GETUTCDATE(),
    RegistrationUser VARCHAR(100) NOT NULL,
    ModificationDate DATETIME,
    ModificationUser VARCHAR(100),
    DeleteDate DATETIME,
    DeleteUser VARCHAR(100),
    [Delete] BIT DEFAULT 0
)
GO

CREATE TABLE [domain].[ApplicationUser](
    ApplicationId INT FOREIGN KEY REFERENCES [domain].[Application],
    UserId INT FOREIGN KEY REFERENCES [identity].[User],

    --Seguimiento
    RegistrationDate DATETIME DEFAULT GETUTCDATE(),
    RegistrationUser VARCHAR(100) NOT NULL,
    ModificationDate DATETIME,
    ModificationUser VARCHAR(100),
    DeleteDate DATETIME,
    DeleteUser VARCHAR(100),
    [Delete] BIT DEFAULT 0
)
