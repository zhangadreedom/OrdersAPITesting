# OrdersAPITesting
Orders API code Testing

**Local dev:**

1. Set user secrets:

   cd src\Orders.Api
   
   dotnet user-secrets init
   
   dotnet user-secrets set "ConnectionStrings:Default" "Server=localhost;Database=OrdersDb;User Id=XXXXX;Password=XXXXXXX;TrustServerCertificate=True;MultipleActiveResultSets=True"



**Generate DB migration:**

1 cd src\Order.Infra

2 dotnet ef migrations add InitialCreate -s ..\Order.Api\Order.Api.csproj

3 dotnet ef database update -s ..\Order.Api\Order.Api.csproj



**Local testing:**
Post => Adding a new Order:
curl -X POST https://localhost:7256/api/orders -H "Content-Type: application/json" -d "{\"orderId\":\"16f5a78c-4685-46e2-ba38-14d4b8502890\",\"customerName\":\":Lisi\",\"createdAt\":\"2025-10-15T15:30:00+08:00\",\"items\":[{\"productId\":\"11111111-2222-3333-4444-555555555556\",\"quantity\":12},{\"productId\":\"76666666-7777-8888-9999-000000000000\",\"quantity\":11}]}"

Get => Getting an Order by ID:

curl -X GET https://localhost:7256/api/orders/16f5a78c-4685-46e2-ba38-14d4b8502890 -H "Accept: application/json"



**UT Test:**
dotnet test

~74.6% overall coverage

| Assembly                  | Covered (Blocks) | Not Covered (Blocks) | Covered (Lines) | Partially Covered (Lines) | Not Covered (Lines) |
| ------------------------- | ---------------- | -------------------- | --------------- | ------------------------- | ------------------- |
| **order.api.dll**         | 58               | 20                   | 34              | 2                         | 15                  |
| **order.domain.dll**      | 40               | 14                   | 29              | 6                         | 2                   |
| **order.application.dll** | 135              | 22                   | 71              | 1                         | 19                  |
| **order.unittests.dll**   | 287              | 1                    | 109             | 1                         | 0                   |
| **order.infra.dll**       | 105              | 155                  | 42              | 0                         | 305                 |
| **🔹 Total**               | **625**          | **212**              | **285**         | **10**                    | **341**             |



##### Project Structure Overview

| Layer / Project       | Key Directories / Files                                      | Responsibility                                               |
| --------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **Order.Api**         | `Controllers/`, `Program.cs`, `appsettings.json`             | **Presentation layer** — defines HTTP endpoints and application startup. Handles request routing, model validation, and returns standardized responses (e.g., `201 Created`, `404 NotFound`). |
| **Order.Application** | `Services/`, `Validation/`, DTOs like `CreateOrderRequest`, `GetOrderResponse` | **Business logic / use case layer** — contains `OrderService` which implements application rules, orchestration, and DTO mapping between domain and API. |
| **Order.Domain**      | `Entities/Orders.cs`, `Entities/OrderItem.cs`, `IOrderRepository.cs` | **Domain model layer** — encapsulates core business entities and rules (Order, OrderItem). Independent of any infrastructure or frameworks. |
| **Order.Infra**       | `Repositories/OrderRepository.cs`, `OrderDbContext.cs`, `Migrations/`, `DI/InfrastructureServiceCollectionExtensions.cs` | **Infrastructure layer** — implements persistence using EF Core. Contains `OrderRepository` (implements `IOrderRepository`) and `OrderDbContext` for EF Core Code-First database mapping. |
| **Order.UnitTests**   | `OrderServiceTests.cs`, `OrderIntegrationTests.cs`, `CreateOrderRequestValidationTests.cs`, `TestWebAppFactory.cs` | **Testing layer** — contains both **unit tests** and **integration tests**. Unit tests cover services and validators; integration tests run against a real in-memory EF Core DB. |

**Dependency Flow**

Order.Api  ───►  Order.Application  ───►  Order.Domain
                     ▲
                     │
                Order.Infra

- API depends only on Application layer.
- Application depends only on Domain (via interfaces).
- Infra implements Domain interfaces and is injected at runtime.
- Tests reference all layers but will not mix test-only dependencies with production code.

