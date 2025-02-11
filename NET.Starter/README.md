# **NET.Starter.API**
🚀 .NET 8 Web API Project Starter

## **Prerequisites**
## Installation of .NET 8 SDK RUNTIMES
Make sure you have installed **.NET 8 SDK** before starting.
🔗 **Download .NET 8 SDK & Runtimes**:
[.NET 8 Official Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

---

## **📖 Documentation & Learning Resources**
- **Entity Framework Core**: [Learn EF Core](https://www.learnentityframeworkcore.com/)
- **AutoMapper**: [AutoMapper Docs](https://automapper.org/)

---

## **⚙️ Configuration**
1. Open the `appsettings.json` file located in the `NET.Starter.API/` directory.
2. Update the **Connection String** according to your database settings.
3. Example configurations are available in `appsettings.Development.json`.

---

## **🗄️ Database Migration & Seeding**

### **Adding a New Migration**
1. Open **Visual Studio**.
2. Set **"NET.Starter.API"** as the **Startup Project**.
3. Open **Package Manager Console**.
4. Run the following command:
	```
	Add-Migration -s NET.Starter.API -p NET.Starter.DataAccess.SqlServer -c ApplicationDbContext
	```
5. Start **"NET.Starter.API"** project to create the database.

### Insert GUID with Extension
1. Open **Visual Studio**.
2. Navigate to **Extensions** > **Manage Extensions**.
3. Search for the **"Insert Guid"** that created by **Mads Kristensen**.
4. After installation, use the following shortcut to insert a GUID automatically:
	```
	Ctrl + K, Ctrl + Space
	```

---

## **📊 Logging & Monitoring**
If you are using Grafana for logging, you can run the following query in Grafana Explore:
	```
	{app="net-starter-api", env="dev"} | json
	```

---

## **🎯 Get Started**
1. Clone this repository:
	```
	{app="net-starter-api", env="dev"} | json
	```
2. Install dependencies:
	```
	dotnet restore
	```
3. Run the application:
	```
	dotnet run
	```

---

🚀 **Happy Coding!**
If you have any questions or issues, feel free to create an issue in this repository. 😃
