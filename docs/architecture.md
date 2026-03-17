# Arquitectura del Proyecto auth0rize.api

Este proyecto sigue los principios de **Clean Architecture** y emplea el patrón **CQRS (Command Query Responsibility Segregation)** utilizando la librería **MediatR**. El acceso a datos se realiza de forma eficiente mediante **Dapper** a través de un Repositorio Genérico.

---

## 🏗️ Estructura de Capas

### 1. `auth0rize.auth.api`
Es el punto de entrada de la aplicación (Web API).
- **Controllers**: Contienen los endpoints. Inyectan `IMediator` y despachan las peticiones a la capa de aplicación.
- **Middlewares**: Manejo global de excepciones (`ErrorHandlerMiddleware`) y logs.
- **Program.cs**: Configuración de servicios e inyección de dependencias.

### 2. `auth0rize.auth.application`
Contiene la lógica de negocio y los casos de uso.
- **Features**: Organizado por entidad y acción (Ej: `User/Command/UserCreate`).
    - **Command/Query**: Registros (Records) que definen la entrada.
    - **Handler**: Clases que procesan la lógica usando repositorios.
    - **Request/Response**: DTOs para comunicación externa.
- **Wrappers**: Clase `Response<T>` para estandarizar las respuestas de la API.

### 3. `auth0rize.auth.domain`
El núcleo del sistema. No tiene dependencias de otras capas.
- **Entities**: Definición de tablas como clases POCO (ej. `User`, `Company`).
- **Interfaces**: Contratos para repositorios e inyección de dependencias como `IUnitOfWork` e `IGenericRepository`.
- **Primitives**: Tipos base como `BaseEntity` y constantes de esquemas (`Schemas`).

### 4. `auth0rize.auth.infraestructure`
Implementaciones concretas de la infraestructura.
- **Persistence**: 
    - `GenericRepository`: Implementación robusta con Dapper para operaciones CRUD automáticas.
    - `UnitOfWork`: Gestiona las transacciones y la conexión a la base de datos.
- **Extensions**: Configuración de servicios para acceso a datos.

### 5. `auth0rize.auth.notification`
Módulo para tareas en segundo plano y comunicaciones.
- **BackgroundTaskQueue**: Cola para procesos asíncronos (ej. envío de correos).
- **UserNotificationRepository**: Implementación específica para notificaciones (HTML mailing).

---

## 🛠️ Guía de Implementación de Nuevos Endpoints

Para agregar una nueva funcionalidad (ej. Crear una "Empresa"), sigue estos pasos:

### Paso 1: Definir la Entidad (Domain)
Crea la clase en `auth0rize.auth.domain/Company/Company.cs`.
```csharp
public class Company : BaseEntity {
    public string Name { get; set; }
    public string TaxId { get; set; }
}
```

### Paso 2: Crear el Feature (Application)
Crea una carpeta en `auth0rize.auth.application/Features/Company/Command/CompanyCreate`.

1. **CompanyCreate.cs** (El Request de MediatR):
   ```csharp
   public record CompanyCreate(string Name, string TaxId) : IRequest<Response<int>>;
   ```

2. **CompanyCreateHandler.cs** (La Lógica):
   ```csharp
   internal class CompanyCreateHandler : IRequestHandler<CompanyCreate, Response<int>> {
       private readonly IUnitOfWork _unitOfWork;
       public CompanyCreateHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

       public async Task<Response<int>> Handle(CompanyCreate request, CancellationToken ct) {
           var id = await _unitOfWork.Repository<Company>().InsertAsync(new Company {
               Name = request.Name,
               TaxId = request.TaxId
           }, Schemas.Security);
           
           return new Response<int>(id, "Empresa creada con éxito.");
       }
   }
   ```

### Paso 3: Crear el Controlador (API)
Añade el endpoint en `auth0rize.auth.api/Controllers/v1/CompanyController.cs`.
```csharp
public class CompanyController : BaseApiController {
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CompanyCreate command) {
        return Ok(await Mediator.Send(command));
    }
}
```

---

## 💾 Acceso a Datos con IGenericRepository

El repositorio genérico facilita las consultas sin escribir SQL manual para casos comunes:

- **Inserción**: `await _unitOfWork.Repository<T>().InsertAsync(entity, Schemas.Security);`
- **Consulta con Filtros**: 
  ```csharp
  var filters = new Dictionary<string, object> { { "Email", email } };
  var users = await _unitOfWork.Repository<User>().QueryAsync<User>(filters, Schemas.Security);
  ```
- **Paginación**:
  ```csharp
  var (data, total) = await _unitOfWork.Repository<User>().QueryPagedAsync<User>(
      filters: null, orderBy: "Id", ascending: true, skip: 0, take: 10, schema: Schemas.Security
  );
  ```

---

## 💡 Mejores Prácticas
1. **Validación**: Lanza una `ApiException` para errores de lógica de negocio; el middleware la capturará y devolverá un `400 Bad Request`.
2. **Esquemas**: Especifica siempre el esquema usando el enum `Schemas` (ej. `Schemas.Security`) en las llamadas al repositorio.
3. **Inyección**: Usa la inyeccion de dependencias vía constructor.
4. **Tareas Pesadas**: Para envíos de correos o procesos largos, usa `IBackgroundTaskQueue` para no bloquear al usuario.
