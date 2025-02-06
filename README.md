# Portfolio

[WIP]

For Client Prod deployment:  
- add a new file to `Portfolio/wwwroot/appsettings.Production.json`
- insert and update these settings
```json
{
  "ApiUrl": "https://TODO/api",
  "ClientAppConfig": {
    "OwnerName": "TODO",
    "CVDesignType": 1
  }
}
```
- az login
- swa deploy

## API (Server):
### DB Migrations
`dotnet tool install --global dotnet-ef --version 8.0.12`

#### Local
- generate migrations
```
dotnet ef migrations add CreatePortfolioDB --project Portfolio.API.DataAccess.SQLServer --startup-project Portfolio.API -- --DatabaseProvider "SQLServer"

dotnet ef migrations add CreatePortfolioDB --project Portfolio.API.DataAccess.SQLite --startup-project Portfolio.API -- --DatabaseProvider "SQLite"
```
- run migrations
```
dotnet ef database update --project Portfolio.API.DataAccess --startup-project Portfolio.API -- --DatabaseProvider "SQLite"
```

#### Deploy
- run migration
```
dotnet ef database update --project Portfolio.API.DataAccess --startup-project Portfolio.API -- --DatabaseProvider "SQLServer" --ConnectionStrings:PortfolioDB "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"
```
