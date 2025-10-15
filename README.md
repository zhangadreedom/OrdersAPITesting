# OrdersAPITesting
Orders API code Testing

Local dev:

1. Set user secrets:

   cd src\Orders.Api
   
   dotnet user-secrets init
   
   dotnet user-secrets set "ConnectionStrings:Default" "Server=localhost;Database=OrdersDb;User Id=XXXXX;Password=XXXXXXX;TrustServerCertificate=True;MultipleActiveResultSets=True"



Generate DB migration:

1 cd src\Order.Infra

2 dotnet ef migrations add InitialCreate -s ..\Order.Api\Order.Api.csproj

3 dotnet ef database update -s ..\Order.Api\Order.Api.csproj
