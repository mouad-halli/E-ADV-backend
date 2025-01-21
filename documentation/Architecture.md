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
├── Repositories/
├── Services/
├── Data/
├── Interfaces/ │
                ├── Repositories/
                └── Services/
├── Migrations/
├── Models/ │
            ├── DTOS/ │
            └── Entities/
├── common/ │
            ├── Exceptions/
            └── Middlewares/
├── Program.cs
├── Server.csproj
├── Server.sln
```
