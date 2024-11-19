# NET.Starter.API
.NET 8 Web API Project Starter

## Installation of .NET 8 SDK RUNTIMES
1. Download .NET 8 SDK and runtimes from the following link:
   ```
   https://dotnet.microsoft.com/en-us/download/dotnet/8.0
   ```

## Documentation To Learn
1. Read anything about Entity Framework Core at https://www.learnentityframeworkcore.com/
2. Read anything about AutoMapper at https://automapper.org/

## Configuration
1. Open the "NET.Starter.API/appsettings.json" file and update the connection string.

## Data Seeding
1. Open Visual Studio
2. Go to Extensions > Manage Extensions
3. Search "Insert Guid" that created by Mads Kristensen
4. To use this extension use this shortcut
   ```
   Ctrl + K, Ctrl + Space
   ```
5. That will insert a GUID exactly in the point you are in the code

## Migration Database
1. Open Visual Studio
2. Set the "NET.Starter.API" as the startup project.
3. Open the Package Manager Console.
4. Run the following command to add new migration:
   ```
   Add-Migration -s NET.Starter.API -p NET.Starter.API.DataAccess -c ApplicationDbContext
   ```
5. Start "NET.Starter.API" project to create the database.