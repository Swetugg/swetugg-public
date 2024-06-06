## Development
Bear with me, it's all very hacky right now!

### Setup a database connection
1. In 1password you find the entry called "Swetugg dev db connectionstring". This database is copy from production and is renewed every night.
2. Copy the value and run `dotnet user-secrets set "ConnectionStrings:Swetugg" "{The value of the connectionstring}"`.
3. Send your ip-number to be added to the firewall to Cecilia 

### Run the application (VS Code)
1. Run `dotnet watch` from the root of the repository
2. Navigate to "https://localhost:7286/{conference slug}" the conference slugs corresponds to the names of the folders found below "/Pages", such as "sthlm-2023".
3. Ignore the ssl warning.

### Install dotnet tools
1. Run `dotnet tool restore` from the root of the repository.

### Building CSS
We're currently using Sass to build our CSS (subject to change). If you're using VS Code and have installed the workspace recommended extensions (https://marketplace.visualstudio.com/items?itemName=glenn2223.live-sass) then your CSS should be built on save after you've turned on live sass.

Why are the generated css files commited instead of ignored? This is also subject to change, the goal is to have them either generated as part of the build pipeline, or have them generated on demand through the hosting pipeline.

## Test site in Azure
We have QA slot in Azure for the site, just run the github action "" on your branch and it will be deployed to the QA slot. The url is https://swetugg-qa.azurewebsites.net/{conference slug}.