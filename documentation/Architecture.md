# Architecture

E-ADV Server is built using ASP.NET Core and follows a layered architecture. The main layers are:

1. **Controllers**: Handle HTTP requests and responses.
2. **Services**: Contain business logic.
3. **Repositories**: Handle data access and persistence.
4. **Models**: Define the data structures.
5. **Middlewares**: Handle cross-cutting concerns like exception handling.
6. **Exceptions**: Custom exceptions for error handling.

## Project Structure

```bash
├── Controllers/
├── Services/
├── Repositories/
├── Models/ │
            ├── DTOS/ │
            └── Entities/
├── common/ │
            ├── Exceptions/
            └── Middlewares/
├── Interfaces/ │
                ├── Repositories/
                └── Services/
├── Data/
├── Migrations/
├── Program.cs
├── Server.csproj
├── Server.sln
```
## Extended Project Structure
```bash
└── e-adv-backend/
    ├── README.md
    ├── Program.cs
    ├── Server.csproj
    ├── Server.sln
    ├── reference.appSettings.json
    ├── Controllers/
    │   ├── AppointmentController.cs
    │   ├── AuthenticationController.cs
    │   ├── ProductPresentationController.cs
    │   ├── ProductSlideController.cs
    │   └── UserController.cs
    ├── Data/
    │   └── AppDbContext.cs
    ├── Interfaces/
    │   ├── Repositories/
    │   │   ├── IAppointmentRepository.cs
    │   │   ├── ILocationRepository.cs
    │   │   ├── IProductPresentationRepository.cs
    │   │   ├── IProductSlideRepository.cs
    │   │   └── IUserRepository.cs
    │   └── Services/
    │       ├── IAppointmentService.cs
    │       ├── ICurrentUserService.cs
    │       ├── IGraphApiService.cs
    │       ├── ILocationService.cs
    │       ├── IProductPresentationService.cs
    │       ├── IProductSlideService.cs
    │       ├── ITokenService.cs
    │       └── IUserService.cs
    ├── Migrations/
    │   ├── 20250112082311_initialMigration.Designer.cs
    │   ├── 20250112082311_initialMigration.cs
    │   └── AppDbContextModelSnapshot.cs
    ├── Models/
    │   ├── DTOS/
    │   │   ├── AppointmentDTO.cs
    │   │   ├── LocationDTO.cs
    │   │   ├── ProductPresentationDTO.cs
    │   │   ├── UserDTO.cs
    │   │   └── productSlideDTO.cs
    │   └── Entities/
    │       ├── Appointment.cs
    │       ├── Location.cs
    │       ├── ProductPresentation.cs
    │       ├── ProductSlide.cs
    │       └── User.cs
    ├── Properties/
    │   └── launchSettings.json
    ├── Repositories/
    │   ├── AppointmentRepository.cs
    │   ├── LocationRepository.cs
    │   ├── ProductPresentationRepository.cs
    │   ├── ProductSlideRepository.cs
    │   └── UserRepository.cs
    ├── Services/
    │   ├── AppointmentService.cs
    │   ├── CurrentUserService.cs
    │   ├── GraphApiService.cs
    │   ├── LocationService.cs
    │   ├── ProductPresentationService.cs
    │   ├── ProductSlideService.cs
    │   ├── TokenService.cs
    │   └── UserService.cs
    ├── common/
    │   ├── Exceptions/
    │   │   ├── BadRequestException.cs
    │   │   ├── ForbiddenException.cs
    │   │   ├── NotFoundException.cs
    │   │   └── UnauthorizedException.cs
    │   └── Middlewares/
    │       └── ExceptionHandlerMiddleware.cs
    └── documentation/
        ├── API.md
        ├── Architecture.md
        ├── Exceptions.md
        ├── Middlewares.md
        ├── Models.md
        ├── Repositories.md
        ├── Services.md
        └── Setup.md

```
