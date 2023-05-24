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

### Building CSS
We're currently using Sass to build our CSS (subject to change). If you're using VS Code and have installed the workspace recommended extensions then your CSS should be built on save after you've turned on live sass.

Why are the generated css files commited instead of ignored? This is also subject to change, the goal is to have them either generated as part of the build pipeline, or have them generated on demand through the hosting pipeline.