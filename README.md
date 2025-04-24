# 🧾 Product Information API

A clean and well-structured ASP.NET Core Web API project for managing **Products** and **Manufacturers**, built using *
**.NET 8**, **EF Core**, **MySQL**, and **Docker**, with automated integration tests using **Testcontainers**.

---

## 🚀 Features

- 🐳 Fully Dockerized API and MySQL database
- 🧱 Entity Framework Core 8 with real MySQL (not SQLite)
- ✅ Serilog structured logging
- 🧾 Input validation and structured error responses
- 🔁 DTOs for safe and consistent data contracts
- 🔍 Efficient LINQ projections with optional AutoMapper support
- 🌐 Global exception handling via middleware
- 📥 Real EF Core Migrations applied automatically
- 🧪 Integration tests using real MySQL via Testcontainers

---

## 📦 Requirements

- [Docker](https://www.docker.com/products/docker-desktop)
- (Optional) [.NET 8 SDK](https://dotnet.microsoft.com/download) if you want to generate migrations or run outside
  Docker
- (Optional) [Postman](https://www.postman.com/) or `curl` for testing
- (Optional) EF Core CLI (for creating migrations):

```bash
dotnet tool install --global dotnet-ef
```

---

## 🐳 Running the Application

### 1. Build and start with Docker Compose

```bash
docker compose up --build
```

This will:

- Build the API image
- Start the API container and MySQL container

### 2. Access the API

- `http://localhost:8080`
- Swagger UI: `http://localhost:8080/swagger`

---

## 🧪 Running Integration Tests

Integration tests use `xUnit` + `Testcontainers` to spin up a MySQL container automatically:

```bash
dotnet test
```

This does **not** rely on Docker Compose and uses disposable databases per test run.

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
- 500 for unhandled errors (stack traces hidden from responses)

---

## 📂 Project Structure

- `Controllers/` – API endpoints
- `Models/Entities/` – EF Core models
- `Models/DTO/` – DTOs for input/output
- `Contexts/` – EF DbContext and configuration
- `Middleware/` – Global exception handler
- `Tests/` – Integration tests with `Testcontainers`

---

## 🧠 Notes

- Uses `.Select(...)` projections for efficient query shaping
- Ensures test database matches production schema via EF `Migrate()`
- No SQLite — everything uses MySQL for realism and accuracy

---

## 🔮 Future Development

**Iterative work to be considered:**

- AutoMapper integration for cleaner DTO mapping
- Finalise stack trace information on failed requests
- Provide more user-friendly errors on invalid GUID requests
- Make `Product.Name` unique via index / unique constraint
