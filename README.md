# 🧾 Product Information API

A clean and well-structured ASP.NET Core Web API project for managing **Products** and **Manufacturers**, built using **.NET 8**, **EF Core**, and **SQLite** with proper **validation**, **error handling**, and **RESTful conventions**.

---

## 🚀 Features

- 🧱 Entity Framework Core 8 with SQLite
- ✅ Serilog structured logging
- 🧾 Input validation and structured error responses
- 🔁 DTOs for safe and consistent data contracts
- 🔍 Efficient LINQ projections
- 🌐 Global exception handling via middleware
- 🔄 Database schema managed via EF Core Migrations

---

## 📦 Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- (Optional) [Postman](https://www.postman.com/) or `curl` for testing
- EF Core CLI: Install with
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## 🧱 Getting Started

### 1. Apply Migrations / Create the Database

```bash
dotnet ef database update
```

This uses SQLite and will generate a `productinfo.db` file locally.

### 2. Run the API

```bash
dotnet run --project ProductInformationApi
```

API will be available at: `https://localhost:{port}`

---

## 📚 API Endpoints

### 🏭 Manufacturers

- `GET /manufacturers` – List all manufacturers + their products
- `GET /manufacturers/{id}` – Get a single manufacturer
- `GET /manufacturers/{id}/products` – Get products by manufacturer
- `POST /manufacturers` – Create a new manufacturer

### 📦 Products

- `GET /products` – List all products
- `GET /products/{id}` – Get product by ID
- `POST /products` – Create a new product (must provide valid Manufacturer ID)

---

## ✅ Validation

- All fields are validated using `[Required]` and custom logic
- Error Responses use `ValidationProblemDetails`

---

## 💥 Error Handling

Global exception handler middleware provides clean, consistent error messages:

- 400 for validation issues
- 404 for missing resources
- 500 for unhandled errors (stack leaks prevented)

---

## 🧪 Testing

Integration tests use `xUnit`, `WebApplicationFactory`, and an in-memory SQLite DB.

To run all tests:

```bash
dotnet test
```

---

## 📂 Project Structure

- `Controllers/` – API endpoints
- `Models/Entities/` – EF Core models
- `Models/DTO/` – DTOs for input/output
- `Contexts/` – EF DbContext and configuration
- `Middleware/` – Global exception handler
- `Tests/` – Integration tests using `xUnit`

---

## 🧠 Notes

- Uses `.Select(...)` projections for performance and shaping
- Unique product names enforced both in code **and** via database index
- Circular references avoided by not exposing raw EF entities

---

## 🔮 Future Development

**Iterative work to do:**

- AutoMapper integration for cleaner DTO mapping
- Finalise stack trace information on failed requests
- Provide more user-friendly errors on invalid GUID requests
- Make `Product.Name` unique via an index / unique constraint
