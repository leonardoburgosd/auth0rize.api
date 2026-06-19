# Proyecto: Auth0rize  
## Descripción: 
Proyecto web (REST API, Web) que tiene como objetivo poder implementar un SSO, siendo un PaaS open source. Partes del proyecto Backend (auth0rize.api): Será la parte que gestionará la información y la autenticación de la plataforma como tambien del SSO(Single Sign-On). Frontend (auth0rize.app): Sera la plataforma que se comunicará con el backend la cual gestionará la información (PaaS). Frontend (auth0rize.sso): Sera la plataforma que se comunicara con el backend la cual únicamente proporcionará la autenticación y la cual será implementada en otros proyectos. 
 
## Funcionalidades 
- Siempre habrá un superadmin el cual será registrado la primera vez que se use la plataforma, se tendrá que realizar una confirmación por medio del correo y un mensaje de texto al teléfono proporcionado (código de 6 dígitos). La autenticación del usuario admin será forzada a habilitar el segundo factor de autenticación ya sea por mensaje de texto (por defecto) o por medio de una aplicación de terceros (Google Authenticator, Microsoft Authenticator), el último recurso será la generación de un link de emergencia por email el cual solo durará 5 minutos activo. 
- Al ingresar a la plataforma el superadmin podrá crear domain, application, user, usertype, company. 
- Los usertype por defecto son superadmin, admin y user. 
- El superadmin puede crear domains, applications, users y usertypes. 
- El admin solo puede crear applications, users y domains. 
- El user no tiene acceso al PaaS. 
- Los domain sirven para agrupar a usuarios, un usuario puede pertenecer a uno o más domain y si el admin está en el mismo domain solo puede visualizar a dichos users. 
- Las application sirven para poder identificar un SSO en particular el cual será usado en una aplicación en concreto. En el futuro, va a servir para poder asignar un diseño visual personalizado. 
- Company sirve para poder aislar plataformas. Por ejemplo, puedo tener un negocio de comida rápida, pero quiero tener un mismo SSO asignado para varias aplicaciones internas de dicho negocio; y puedo tener un negocio de venta de ropa y quiero tener otro SSO solo para dicho negocio. 
- Un domain debería estar enlazado con un application y en consecuencia los users de tipo ‘user’ y ‘admin’ deberían tener la capacidad de usar dicho SSO, sin embargo, no podrían usar un SSO distinto.