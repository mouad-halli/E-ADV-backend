# Setup

## Prerequisites

- .NET 8.0 SDK
- SQL Server

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/mouad-halli/E-ADV-backend.git
    cd E-ADV-backend
    ```

2. Create appsettings.json in your root folder and Configure the database connection string, you can see [appsettings.json](/reference.appSettings.json) as a reference:
    ```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "Your-Database-Connection-String"
        },
        "Jwt": {
            "Key": "Your-JWT-Secret-Key",
            "Issuer": "Your-JWT-Issuer"
        },
        "MobileApp": {
            "Url": "Your mobile app url"
        }
    }
    ```

3. Run the migrations to set up the database:
    ```sh
    dotnet ef database update
    ```

4. Build and run the project:
    ```sh
    dotnet build
    dotnet run
    ```

5. The API will be available at `https://localhost:7125` or `http://localhost:4200`.