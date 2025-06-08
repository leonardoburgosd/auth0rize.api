# Auth0rize API
Proyecto backend para autenticación y administración de usuarios, como también para asignación de roles, creación de menús por rol.

## Version 1 (desplegado)
- https://auth.api.leonardoburgosd.site/swagger/index.html (deshabilitado temporalmente)

## Tecnologías utilizadas
- :white_check_mark: .NET Core 7.0
- :white_check_mark: Notificaciones por correo Gmail - limite 500 por día
- :white_check_mark: Render.com (despliegue para pruebas - deshabilitado temporalmente)

## Funcionalidades
|Funcionalidad                       | % de desarrollo | Desplegado        |
|------------------------------------|:---------------:|:-----------------:|
|Autenticación                       |80%              |:white_check_mark: |
|Registro de usuario                 |80%              |:white_check_mark: |
|Administración de usuarios          |0%               |:x:                |
|Administración de aplicativos       |0%               |:x:                |
|Administración de menús             |0%               |:x:                |
|Administración de tipos de usuarios |0%               |:x:                |

## Proyectos relacionados

| Funcionalidad           | Acceso                                                    |  
| ----------------------- | --------------------------------------------------------- |
| Frontend administrator  | https://github.com/leonardoburgosd/auth0rize-dashboard    | 
| SSO Frontend            | https://github.com/leonardoburgosd/authenticator.frontend |


## Definición de terminos
- Application : sirve para poder registrar los applicativos en donde se implementara la autenticación

- ApplicationDomain: relaciona directamente un application con un dominio.

- Domain : sirve para identificar a que grupo pertenece un usuario, el administrador es el creador del dominio y los usuarios que cree se relacionaran con dicho dominio. No se puede autenticar un usuario perteneciente a un dominio en otro

# Documentación de esquema de base de datos
## Schema: security
1. UserType : Registra los roles de usuario en el sistema (superadmin, admin, user).

| Columna            | Tipo           | Restricciones                              | Descripción                                 |
| ------------------ | -------------- | ------------------------------------------ | ------------------------------------------- |
| `Id`               | `SERIAL`       | `PRIMARY KEY`                              | Identificador único de rol.                 |
| `Name`             | `VARCHAR(150)` | `NOT NULL, UNIQUE`                         | Nombre del rol.                             |
| `RegistrationDate` | `TIMESTAMP`    | `NOT NULL, DEFAULT CURRENT_TIMESTAMP`      | Fecha de creación del rol.                  |
| `UserRegistration` | `INT`          |                                            | Usuario que creó el rol.                    |
| `DateUpdate`       | `TIMESTAMP`    |                                            | Fecha de última modificación.               |
| `UserUpdate`       | `INT`          | `REFERENCES security."User"(Id)`           | Usuario que realizó la última modificación. |
| `DateDeleted`      | `TIMESTAMP`    |                                            | Fecha de marcado como eliminado.            |
| `UserDeleted`      | `INT`          | `REFERENCES security."User"(Id)`           | Usuario que marcó como eliminado.           |
| `IsDeleted`        | `BOOLEAN`      | `NOT NULL, DEFAULT FALSE`                  | Flag de borrado lógico.                     |

`
Mejora pendiente: índice en Name (ya es UNIQUE) y considerar índices adicionales si se filtra por IsDeleted.
`

2. Domain: Define los dominios de autenticación (multi‑tenant).

| Columna            | Tipo        | Restricciones                                 | Descripción                        |
| ------------------ | ----------- | --------------------------------------------- | ---------------------------------- |
| `Id`               | `SERIAL`    | `PRIMARY KEY`                                 | Identificador interno.             |
| `Code`             | `UUID`      | `NOT NULL, DEFAULT gen_random_uuid(), UNIQUE` | Identificador público del dominio. |
| `RegistrationDate` | `TIMESTAMP` | `NOT NULL, DEFAULT CURRENT_TIMESTAMP`         | Fecha de creación del dominio.     |
| `UserRegistration` | `INT`       | `NOT NULL, REFERENCES security."User"(Id)`    | Usuario que creó el dominio.       |
| `DateUpdate`       | `TIMESTAMP` |                                               | Fecha de última modificación.      |
| `UserUpdate`       | `INT`       | `REFERENCES security."User"(Id)`              | Usuario que modificó el dominio.   |
| `DateDeleted`      | `TIMESTAMP` |                                               | Fecha de marcado como eliminado.   |
| `UserDeleted`      | `INT`       | `REFERENCES security."User"(Id)`              | Usuario que marcó como eliminado.  |
| `IsDeleted`        | `BOOLEAN`   | `NOT NULL, DEFAULT FALSE`                     | Flag de borrado lógico.            |

`
Mejora pendiente: índice en (Code) para lookup rápido por UUID.
`

3. "User": Almacena las cuentas de usuario.

| Columna                  | Tipo           | Restricciones                                | Descripción                        |
| -------------------------| -------------- | -------------------------------------------- | ---------------------------------- |
| `Id`                     | `SERIAL`       | `PRIMARY KEY`                                | Identificador del usuario.         |
| `FirstName`              | `VARCHAR(150)` | `NOT NULL`                                   | Nombre propio.                     |
| `LastName`               | `VARCHAR(150)` | `NOT NULL`                                   | Apellido paterno.                  |
| `MotherLastName`         | `VARCHAR(150)` | `NOT NULL`                                   | Apellido materno.                  |
| `UserName`               | `VARCHAR(30)`  | `NOT NULL, UNIQUE`                           | Nombre de usuario.                 |
| `Email`                  | `VARCHAR(150)` | `NOT NULL, UNIQUE`                           | Correo electrónico.                |
| `PasswordHash`           | `BYTEA`        | `NOT NULL`                                   | Hash de la contraseña.             |
| `PasswordSalt`           | `BYTEA`        | `NOT NULL`                                   | Salt utilizado para el hash.       |
| `Avatar`                 | `VARCHAR(100)` |                                              | Ruta o URL al avatar del usuario.  |
| `TypeId`                 | `INT`          | `NOT NULL, REFERENCES security.UserType(Id)` | Rol global del usuario.            |
| `IsDoubleFactorActive`   | `BOOLEAN`      | `NOT NULL, DEFAULT FALSE`                    | Si tiene 2FA habilitado.           |
| `DoubleFactorActiveCode` | `VARCHAR(20)`  |                                              | Secreto TOTP                       |
| `IsConfirmed`            | `BOOLEAN`      | `NOT NULL, DEFAULT FALSE`                    | Si confirmó su cuenta (email/2FA). |
| `RegistrationDate`       | `TIMESTAMP`    | `NOT NULL, DEFAULT CURRENT_TIMESTAMP`        | Fecha de creación de la cuenta.    |
| `UserRegistration`       | `INT`          | `NOT NULL, REFERENCES security."User"(Id)`   | Usuario que creó la cuenta.        |
| `DateUpdate`             | `TIMESTAMP`    |                                              | Fecha de última modificación.      |
| `UserUpdate`             | `INT`          | `REFERENCES security."User"(Id)`             | Usuario que modificó la cuenta.    |
| `DateDeleted`            | `TIMESTAMP`    |                                              | Fecha de marcado como eliminado.   |
| `UserDeleted`            | `INT`          | `REFERENCES security."User"(Id)`             | Usuario que marcó como eliminado.  |
| `IsDeleted`              | `BOOLEAN`      | `NOT NULL, DEFAULT FALSE`                    | Flag de borrado lógico.            |

`
Mejora pendiente: índices en Email, UserName y en TypeId si se realizan filtrados frecuentes.
`

4. ConfirmAccount: Gestiona los códigos de verificación (email/2FA).

| Columna            | Tipo        | Restricciones                                              | Descripción                                     |
| ------------------ | ----------- | ---------------------------------------------------------- | ----------------------------------------------- |
| `Id`               | `SERIAL`    | `PRIMARY KEY`                                              | Identificador interno.                          |
| `Code`             | `UUID`      | `NOT NULL, DEFAULT gen_random_uuid()`                      | Código de confirmación.                         |
| `UserId`           | `INT`       | `NOT NULL, REFERENCES security."User"(Id)`                 | Usuario al que pertenece el código.             |
| `ExpirationDate`   | `TIMESTAMP` | `NOT NULL, DEFAULT (CURRENT_TIMESTAMP + INTERVAL '1 day')` | Fecha de expiración automática del código.      |
| `RegistrationDate` | `TIMESTAMP` | `NOT NULL, DEFAULT CURRENT_TIMESTAMP`                      | Fecha de emisión del código.                    |
| `UserRegistration` | `INT`       | `NOT NULL, REFERENCES security."User"(Id)`                 | Usuario que generó el código.                   |
| `DateUpdate`       | `TIMESTAMP` |                                                            | Fecha de última actualización de este registro. |
| `IsConfirm`        | `BOOLEAN`   | `NOT NULL, DEFAULT FALSE`                                  | Marca si el código ya fue usado/confirmado.     |

`
Mejora pendiente: índice compuesto en (UserId, IsConfirm) para búsquedas de confirmaciones pendientes.
`

5. UserDomain: Asocia usuarios a dominios y define su rol en cada dominio.

| Columna            | Tipo        | Restricciones                                | Descripción                         |
| ------------------ | ----------- | -------------------------------------------- | ----------------------------------- |
| `UserId`           | `INT`       | `NOT NULL, REFERENCES security."User"(Id)`   | Usuario asociado.                   |
| `DomainId`         | `INT`       | `NOT NULL, REFERENCES security.Domain(Id)`   | Dominio asociado.                   |
| `RoleId`           | `INT`       | `NOT NULL, REFERENCES security.UserType(Id)` | Rol del usuario *en este dominio*.  |
| `RegistrationDate` | `TIMESTAMP` | `NOT NULL, DEFAULT CURRENT_TIMESTAMP`        | Fecha de asociación.                |
| `UserRegistration` | `INT`       | `NOT NULL, REFERENCES security."User"(Id)`   | Usuario que creó la asociación.     |
| `DateUpdate`       | `TIMESTAMP` |                                              | Fecha de última actualización.      |
| `UserUpdate`       | `INT`       | `REFERENCES security."User"(Id)`             | Usuario que la actualizó.           |
| `DateDeleted`      | `TIMESTAMP` |                                              | Fecha de marcado como eliminado.    |
| `UserDeleted`      | `INT`       | `REFERENCES security."User"(Id)`             | Usuario que lo eliminó lógicamente. |
| `IsDeleted`        | `BOOLEAN`   | `NOT NULL, DEFAULT FALSE`                    | Flag de borrado lógico.             |

- PK: (UserId, DomainId)

`
Mejora pendiente: índice en RoleId si se filtra usuarios por rol en un dominio.
`

## Schema: organization
6. Company: Define las compañías o negocios de un dominio.

| Columna            | Tipo           | Restricciones                              | Descripción                         |
| ------------------ | -------------- | ------------------------------------------ | ----------------------------------- |
| `Id`               | `SERIAL`       | `PRIMARY KEY`                              | Identificador de compañía.          |
| `Name`             | `VARCHAR(150)` | `NOT NULL`                                 | Nombre de la compañía.              |
| `DomainId`         | `INT`          | `NOT NULL, REFERENCES security.Domain(Id)` | Dominio al que pertenece.           |
| `Avatar`           | `VARCHAR(100)` |                                            | Logo o imagen representativa.       |
| `RegistrationDate` | `TIMESTAMP`    | `NOT NULL, DEFAULT CURRENT_TIMESTAMP`      | Fecha de creación del registro.     |
| `UserRegistration` | `INT`          | `NOT NULL, REFERENCES security."User"(Id)` | Usuario que creó la compañía.       |
| `DateUpdate`       | `TIMESTAMP`    |                                            | Fecha de última modificación.       |
| `UserUpdate`       | `INT`          | `REFERENCES security."User"(Id)`           | Usuario que la modificó.            |
| `DateDeleted`      | `TIMESTAMP`    |                                            | Fecha de borrado lógico.            |
| `UserDeleted`      | `INT`          | `REFERENCES security."User"(Id)`           | Usuario que lo eliminó lógicamente. |
| `IsDeleted`        | `BOOLEAN`      | `NOT NULL, DEFAULT FALSE`                  | Flag de borrado lógico.             |

`
Mejora pendiente: índice en DomainId para consultas por dominio.
`

7. Application: Registra las aplicaciones que utilizan el sistema de login.

| Columna            | Tipo           | Restricciones                                 | Descripción                             |
| ------------------ | -------------- | --------------------------------------------- | --------------------------------------- |
| `Id`               | `SERIAL`       | `PRIMARY KEY`                                 | Identificador interno.                  |
| `Name`             | `VARCHAR(150)` | `NOT NULL`                                    | Nombre de la aplicación.                |
| `Code`             | `UUID`         | `NOT NULL, DEFAULT gen_random_uuid(), UNIQUE` | Identificador público de la aplicación. |
| `Description`      | `VARCHAR(150)` |                                               | Breve descripción de su propósito.      |
| `Avatar`           | `VARCHAR(100)` |                                               | Ícono o imagen de la aplicación.        |
| `RegistrationDate` | `TIMESTAMP`    | `NOT NULL, DEFAULT CURRENT_TIMESTAMP`         | Fecha de creación del registro.         |
| `UserRegistration` | `INT`          | `NOT NULL, REFERENCES security."User"(Id)`    | Usuario que creó la aplicación.         |
| `DateUpdate`       | `TIMESTAMP`    |                                               | Fecha de última modificación.           |
| `UserUpdate`       | `INT`          | `REFERENCES security."User"(Id)`              | Usuario que la modificó.                |
| `DateDeleted`      | `TIMESTAMP`    |                                               | Fecha de borrado lógico.                |
| `UserDeleted`      | `INT`          | `REFERENCES security."User"(Id)`              | Usuario que lo eliminó lógicamente.     |
| `IsDeleted`        | `BOOLEAN`      | `NOT NULL, DEFAULT FALSE`                     | Flag de borrado lógico.                 |

`
Mejora pendiente: índice en (Code) para lookup rápido por UUID.
`

8. ApplicationCompany: Tabla puente entre aplicaciones y compañías (multi‑tenant).

| Columna            | Tipo        | Restricciones                                       | Descripción                         |
| ------------------ | ----------- | --------------------------------------------------- | ----------------------------------- |
| `ApplicationId`    | `INT`       | `NOT NULL, REFERENCES organization.Application(Id)` | Aplicación asociada.                |
| `CompanyId`        | `INT`       | `NOT NULL, REFERENCES organization.Company(Id)`     | Compañía asociada.                  |
| `RegistrationDate` | `TIMESTAMP` | `NOT NULL, DEFAULT CURRENT_TIMESTAMP`               | Fecha de creación de la asociación. |
| `UserRegistration` | `INT`       | `NOT NULL, REFERENCES security."User"(Id)`          | Usuario que creó esta asociación.   |
| `DateUpdate`       | `TIMESTAMP` |                                                     | Fecha de última modificación.       |
| `UserUpdate`       | `INT`       | `REFERENCES security."User"(Id)`                    | Usuario que la modificó.            |
| `DateDeleted`      | `TIMESTAMP` |                                                     | Fecha de borrado lógico.            |
| `UserDeleted`      | `INT`       | `REFERENCES security."User"(Id)`                    | Usuario que lo eliminó lógicamente. |
| `IsDeleted`        | `BOOLEAN`   | `NOT NULL, DEFAULT FALSE`                           | Flag de borrado lógico.             |

- PK: (ApplicationId, CompanyId)

`
Mejora pendiente: índices en ApplicationId y CompanyId para consultas de multidominio.
`

### Configuración de variables de entorno
| Nombre                  | Descripción                                                   |  
| ----------------------- | --------------------------------------------------------------|
| auth0rize-postgres-DEV  |                                                               |
| symmetricKey            |                                                               |
| issuer                  |                                                               |
| audience                |                                                               |
| hours                   |                                                               |
| withorigins             |                                                               |
| postAuth0rize           |                                                               |
| hostAuth0rize           |                                                               |
| passwordAuth0rize       |                                                               |
| emailAuth0rize          |                                                               |
| urlDeployAuth0rize      | Variable indica el nombre del host donde está alojada la vista|