# **NET.Starter.API**
🚀 .NET 8 Web API Project Starter

## **Prerequisites**
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

## **✅ Coding Rules & Tips**

### 📌 Use async/await Properly
- Prefer async/await over .Result or .Wait() to avoid deadlocks and improve performance.
- ✅ Do:
	```
	var result = await myService.GetDataAsync();
	```
- ❌ Avoid:
	```
	var result = myService.GetDataAsync().Result;
	```

### 📌 Use `DateTime.UtcNow` Instead of `DateTime.Now`
- **Why?** `DateTime.Now` depends on the server's timezone, which can lead to inconsistency across environments or if you change your environment (change on-premise into on-cloud infrastucture).
- ✅ Do:
	```
	var now = DateTime.UtcNow;
	```
- ❌ Avoid:
	```
	var now = DateTime.Now;
	```

### 📌 Define Date or DateTime Column
- Make sure to choose the correct type based on whether you need time precision or not.
- If you only need date without time:
	```
	public DateOnly SomeDate { get; set; }
	```
- If you need both date and time:
	```
	public DateTime SomeDate { get; set; }
	```

### 📌 Use ILogger<T> for Logging
- Prefer dependency injection over `Console.WriteLine`
	```
	private readonly ILogger<MyService> _logger;
	```
- Ensure your functions include meaningful logging to make it easier for maintainers to trace the system flow and diagnose problems.

### 📌 Follow Naming Conventions
- **PascalCase**: for classes, methods, properties.
- **camelCase**: for local variables and method parameters.

### 📌 Avoid Magic Numbers
- Always assign meaningful names to constant values.
- ✅ Do:
	```
	const int MaxRetries = 3;
	for (int i = 0; i < MaxRetries; i++) { ... }
	```
- ❌ Avoid:
	```
	for (int i = 0; i < 3; i++) { ... }
	```

### 📌 Write XML Documentation
- Always write XML documentation (///) for public methods, classes, and interfaces.
- This improves IntelliSense support and helps maintainability.
- ✅ Example:
	```
	/// <summary>
	/// Calculates the total price including tax.
	/// </summary>
	/// <param name="price">The base price.</param>
	/// <param name="taxRate">The tax rate in percentage.</param>
	/// <returns>The total price including tax.</returns>
	public decimal CalculateTotal(decimal price, decimal taxRate)
	{
		return price * (1 + taxRate / 100);
	}
	```

---

## **🎯 Get Started**
1. Clone this repository:
	```
	git clone https://github.com/ronnyharianto/net-starter
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
