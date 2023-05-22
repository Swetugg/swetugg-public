## Development
Bear with me, it's all very hacky right now!

### Setup a database connection
1. Find a suitable connectionstring by looking at one of the WebApps in Azure, you're looking for the connectionstring called "DefaultConnection" on the Configuration page of the web app.
1. Copy the value and run `dotnet user-secrets set "ConnectionStrings:Swetugg" "{The value of the connectionstring you copied from Azure}"`.
1. Go to the SQL server "swetuggv2" in Azure and add your IP address to the Firewall rules of the Networking tab.

### Run the application (VS Code)
1. Run `dotnet watch` from the root of the repository
1. Navigate to "https://localhost:7286/{conference slug}" the conference slugs corresponds to the names of the folders found below "/Pages", such as "sthlm-2023".
1. Ignore the ssl warning.

### Install dotnet tools
1. Run `dotnet tool restore` from the root of the repository.