Born 30/11/2022

## Database

To run database first, need to install the following packages:

1. Microsoft.EntityFrameworkCore
2. Microsoft.EntityFrameworkCore.Tools
3. Microsoft.EntityFrameworkCore.Proxies
4. Microsoft.EntityFrameworkCore.SqlServer
5. Npgsql.EntityFrameworkCore.PostgreSQL

dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Proxies
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

For VidsDb

```shell
dotnet ef dbcontext scaffold "Host=[ip];Port=5432;Database=VidsDb;Username=[username];Password=[password]" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir "DbContexts\Postgres\VidsDb" --context VidsDbContext --use-database-names --force --no-pluralize --no-onconfiguring
```

For TransDb

```shell
dotnet ef dbcontext scaffold "Host=[ip];Port=5432;Database=TransDb;Username=[username];Password=[password]" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir "DbContexts\Postgres\TransDb" --context TransDbContext --use-database-names --force --no-pluralize --no-onconfiguring
```

## API Key Authentication

Reference

1. http://codingsonata.com/secure-asp-net-core-web-api-using-api-key-authentication/

- Configure the API key in appsetting.json "ApiKeys" section. For development mode, use appsetting.Development.json.
- Decorate the Class or method with [ApiKey] attribute.
- API key is to authenticate 3rd party service that will consume our API.
