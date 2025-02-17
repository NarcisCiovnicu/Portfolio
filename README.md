# Portfolio

## [WIP]

### Prerequisites
- To run the project
  - .Net 9 - for Client
  - .Net 8 - for API
  - (Visual Studio 2022)
- To generate and run migrations
  - `dotnet tool install --global dotnet-ef --version 8.0.12`

---

## Client deployment (Azure static app)
For Client Prod/Stage deployment:  
- Add a new file to
  - `Portfolio/wwwroot/appsettings.Production.json`
  - `Portfolio/wwwroot/appsettings.Staging.json`
- Insert and update these settings
  ```json
  {
    "ClientAppConfig": {
      "ApiUrl": "https://TODO/api",
      "OwnerName": "TODO",
      "CVDesignType": 1,
      "EmailContact": "TODO"
    }
  }
  ```
- az login
- swa deploy
  - `swa deploy --subscription-id [sub-id] --tenant-id [tenant-id] --app-name [app-name] --resource-group [group-name] --app-location ".\PublishClient\wwwroot" --env [production / preview] --output-location "." --swa-config-location "./Portfolio/Properties/[Prod / Stage]"`

## API
### 1. Run migrations
- Starting the API in DEBUG mode also runs migrations
```
dotnet ef database update --project Portfolio.API.DataAccess --startup-project Portfolio.API -- --DatabaseProvider "SQLite"

dotnet ef database update --project Portfolio.API.DataAccess --startup-project Portfolio.API -- --DatabaseProvider "SQLServer" --ConnectionStrings:PortfolioDB "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"
```

### 2. Setup DB
Add a password for authentication in DB using SHA-256 encryption  
https://emn178.github.io/online-tools/sha256.html
#### Run query:
- password: Test#123
```SQL
INSERT INTO Passwords VALUES('3cd29ab3024a695b648213a3df488e6d99ba3ca1497b6a8bf4289c7692ca5f52');
```

---

## Generate Migrations
```
dotnet ef migrations add [Name] --project Portfolio.API.DataAccess.SQLite --startup-project Portfolio.API -- --DatabaseProvider "SQLite"

dotnet ef migrations add [Name] --project Portfolio.API.DataAccess.SQLServer --startup-project Portfolio.API -- --DatabaseProvider "SQLServer" --ConnectionStrings:PortfolioDB "Server=something_to_pass_validation"
```
