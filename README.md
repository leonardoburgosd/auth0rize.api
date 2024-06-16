# Auth0rize API
Proyecto backend para autenticación y administración de usuarios, como también para asignación de roles, creación de menús por rol.

## Tecnologías utilizadas
- :white_check_mark: .NET Core 7.0
- :white_check_mark: Render.com (despliegue)

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

## Version 1 (desplegado)
- https://auth.api.leonardoburgosd.site/swagger/index.html


## Definición de terminos
- Application : sirve para poder registrar los applicativos en donde se implementara la autenticación

- ApplicationDomain: relaciona directamente un application con un dominio.

- Domain : sirve para identificar a que grupo pertenece un usuario, el administrador es el creador del dominio y los usuarios que cree se relacionaran con dicho dominio. No se puede autenticar un usuario perteneciente a un dominio en otro.