CREATE EXTENSION IF NOT EXISTS "pgcrypto";
CREATE SCHEMA security;
CREATE SCHEMA organization;
/*
                TABLAS ESQUEMA SEGURITY
*/

/*
    Tabla sirve para registrar los tipos de usuario que existen en el sistema
    superadmin: usuario maestro, el que crea el login y los dominios, puede crear otros superadmin, admin, users
    admin: usuario que puede crear usuarios en un dominio, puede crear otros admin y users
    user: solo puede ingresar al sistema por el dominio al que pertenece, y actualizar sus datos, no puede crear otros usuarios
*/
CREATE TABLE security.UserType (
    Id               SERIAL    PRIMARY KEY,
    Name             VARCHAR(150) NOT NULL UNIQUE,

    RegistrationDate TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT           ,
    DateUpdate       TIMESTAMP,
    UserUpdate       INT           ,
    DateDeleted      TIMESTAMP,
    UserDeleted      INT           ,
    IsDeleted        BOOLEAN       NOT NULL DEFAULT FALSE
);

/*
    Mejora disponible para la busqueda de dominios por el codigo, ya que el dominio es unico
    CREATE INDEX IF NOT EXISTS IX_Domain_Code ON security.Domain(Code);
*/

/*
    Tabla sirve para registrar a los usuarios del sistema
    IsDoubleFactorActivate: indica si el usuario tiene activado el doble factor de seguridad (el usuario maestro siempre debe tenerlo activado)
    isConfirm: indica si el usuario ah confirmado su cuenta con el envio de correo (el usuario maestro siempre debe confirmalo)
    UserRegistration: para el usuario maestro puede ser 0, pero para el resto de usuarios no debe serlo
*/
CREATE TABLE security.user (
    Id                   SERIAL     PRIMARY KEY,
    FirstName            VARCHAR(150) NOT NULL,
    LastName             VARCHAR(150) NOT NULL,
    MotherLastName       VARCHAR(150) NOT NULL,
    UserName             VARCHAR(30)  NOT NULL UNIQUE,
    Email                VARCHAR(150) NOT NULL UNIQUE,
    PasswordHash         BYTEA        NOT NULL,
    PasswordSalt         BYTEA        NOT NULL,
    Avatar               VARCHAR(100),
    TypeId               INT          NOT NULL REFERENCES security.UserType(Id),
    IsDoubleFactorActive BOOLEAN      NOT NULL DEFAULT FALSE,
    DoubleFactorActiveCode VARCHAR(20),
    LastLogin            TIMESTAMP NULL,
    IsConfirmed          BOOLEAN      NOT NULL DEFAULT FALSE,
    RegistrationDate     TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UserRegistration     INT          NOT NULL REFERENCES security.user(Id) DEFERRABLE INITIALLY DEFERRED,
    DateUpdate           TIMESTAMP,
    UserUpdate           INT          REFERENCES security.user(Id),
    DateDeleted          TIMESTAMP,
    UserDeleted          INT          REFERENCES security.user(Id),
    IsDeleted            BOOLEAN      NOT NULL DEFAULT FALSE
);

/*
    Tabla sirve para generar los codigo de cofirmacion de la cuenta por doble factor
    IsConfirm: indica que el usuario ah confirmado la cuenta desde la aplicacion de doble factor
*/
CREATE TABLE security.ConfirmAccount (
    Id               SERIAL    PRIMARY KEY,
    Code             UUID      NOT NULL,
    UserId           INT       NOT NULL REFERENCES security.user(Id),
    ExpirationDate   TIMESTAMP NOT NULL,

    RegistrationDate TIMESTAMP NOT NULL,
    UserRegistration INT       NOT NULL REFERENCES security.user(Id) DEFERRABLE INITIALLY DEFERRED,
    DateUpdate       TIMESTAMP NULL,
    IsConfirm        BOOLEAN   NOT NULL DEFAULT FALSE
);


/*
    Tabla sirve para registrar el dominio al que pertenece un usuario o varios usuarios, se genera uno por superadmin
    pero el superadmin puede crear otros dominios para que los usuarios puedan registrarse por separado, tambien puede asignar
    a un admin para que administre solo los usuarios relacionados con dicho dominio ya sea uno o varios dominios
    Code: servira para identificar el dominio al que pertenece el login que se va a implementar
*/
CREATE TABLE security.Domain (
    Id               SERIAL       PRIMARY KEY,
    Code             UUID         NOT NULL DEFAULT gen_random_uuid() UNIQUE,

    RegistrationDate TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT           NOT NULL REFERENCES security.user(Id) DEFERRABLE INITIALLY DEFERRED,
    DateUpdate       TIMESTAMP,
    UserUpdate       INT           REFERENCES security.user(Id),
    DateDeleted      TIMESTAMP,
    UserDeleted      INT           REFERENCES security.user(Id),
    IsDeleted        BOOLEAN      NOT NULL DEFAULT FALSE
);




/*
    Mejora disponible para la busqueda de dominios por el codigo
    CREATE INDEX IF NOT EXISTS IX_ConfirmAccount_UserId_IsConfirm
    ON security.ConfirmAccount(UserId, IsConfirm);
*/


/*
    Tabla sirve para registrar los dominios que le pertenecen a un usuario en concreto, todos los dominios, minimo le pertenecen
    al superadmin, pero el superadmin puede asignar a un admin para que administre solo los usuarios relacionados con dicho dominio
    y el admin puede asignar a un usuario para que administre solo los usuarios relacionados con dicho dominio

    Asociación usuario–dominio con rol en ese dominio
*/
CREATE TABLE security.UserDomain (
    UserId           INT NOT NULL REFERENCES security.user(Id),
    DomainId         INT NOT NULL REFERENCES security.Domain(Id),
    RoleId           INT NOT NULL REFERENCES security.UserType(Id),

    RegistrationDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT       NOT NULL REFERENCES security.user(Id) DEFERRABLE INITIALLY DEFERRED,
    DateUpdate       TIMESTAMP NULL,
    UserUpdate       INT       NULL,
    DateDeleted      TIMESTAMP NULL,
    UserDeleted      INT       NULL,
    IsDeleted        BOOLEAN   NOT NULL DEFAULT FALSE,

    PRIMARY KEY(UserId, DomainId)
);


/*
                TABLAS ESQUEMA ORGANIZATION
*/

/*
    Tabla sirve para registrar los distintos negocios que pueden tener los usuarios
    Ejemplo: yo como persona puedo tener un negocio de comida y otro de ropa, entonces
    puedo tener dos compañias, una para cada negocio, pero el usuario es el mismo
*/
CREATE TABLE organization.Company (
    Id               SERIAL     PRIMARY KEY,
    Name             VARCHAR(150) NOT NULL,
    DomainId         INT          NOT NULL REFERENCES security.Domain(Id),
    Avatar           VARCHAR(100),

    RegistrationDate TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT          NOT NULL REFERENCES security.user(Id) DEFERRABLE INITIALLY DEFERRED,
    DateUpdate       TIMESTAMP,
    UserUpdate       INT          REFERENCES security.user(Id),
    DateDeleted      TIMESTAMP,
    UserDeleted      INT          REFERENCES security.user(Id),
    IsDeleted        BOOLEAN      NOT NULL DEFAULT FALSE
);
/*
    Mejora disponible para la busqueda de dominios por el codigo
    CREATE INDEX IF NOT EXISTS IX_Application_Code ON organization.Application(Code);
*/


/*
    Tabla sirve para registrar el aplicativo donde va a ser usado el login
    Name: nombre que aparecera en el diseño del login y tambien en la configuración
    Code: identificador unico de aplicacion que sera usado en el login
*/
CREATE TABLE organization.Application (
    Id               SERIAL     PRIMARY KEY,
    Name             VARCHAR(150) NOT NULL,
    Code             UUID         NOT NULL DEFAULT gen_random_uuid() UNIQUE,
    Description      VARCHAR(150),
    Avatar           VARCHAR(100),

    RegistrationDate TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT          NOT NULL REFERENCES security.user(Id) DEFERRABLE INITIALLY DEFERRED,
    DateUpdate       TIMESTAMP,
    UserUpdate       INT          REFERENCES security.user(Id),
    DateDeleted      TIMESTAMP,
    UserDeleted      INT          REFERENCES security.user(Id),
    IsDeleted        BOOLEAN      NOT NULL DEFAULT FALSE
);

/*
    Tabla sirve para registrar las compañias que va a usar el login
    Ejemplo: mi compañia de ropa y comida pueden usar el mismo login, 
    pero para mi compañia de hoteles y turismo quiero usar un login distinto 
*/
CREATE TABLE organization.ApplicationCompany (
    ApplicationId    INT NOT NULL REFERENCES organization.Application(Id),
    CompanyId        INT NOT NULL REFERENCES organization.Company(Id),

    RegistrationDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT       NOT NULL REFERENCES security.user(Id) DEFERRABLE INITIALLY DEFERRED,
    DateUpdate       TIMESTAMP,
    UserUpdate       INT       REFERENCES security.user(Id),
    DateDeleted      TIMESTAMP,
    UserDeleted      INT       REFERENCES security.user(Id),
    IsDeleted        BOOLEAN   NOT NULL DEFAULT FALSE,

    PRIMARY KEY(ApplicationId, CompanyId)
);
go

/*
    Datos iniciales para el tipo de usuario
*/
INSERT INTO security.UserType(Name) 
VALUES('superadmin')
INSERT INTO security.UserType(Name) 
VALUES('admin')
INSERT INTO security.UserType(Name) 
VALUES('user')