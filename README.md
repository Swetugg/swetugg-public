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

## Set up a new conference

### Setup database
1. Create a new row in the `Conferences` table

### Create the new pages
1. Copy the `Pages/{slug}` folder of the latest conference (assuming that you want the new page to look like the previous one).
1. Use the desired slug (the one that you configured for the Conference database row) for the new conference as the name of the copied folder.
1. Do the same for the `wwwroot/{slug}` folder. Your life will be easier if you pick the folder of the last conference that ran in the same city as the conference you're setting up, as we have different assets (mainly the colors of images and css variables) for different cities.

### Update the new pages
Change the following properties of the pages
- `_Layout.cshtml`
    1. `var slug = "gbg-2023";`.
    1. `<meta property="og:site_name" content="Swetugg Göteborg 2023" />`
- `Index.cshtml`
    1. `var slug = "gbg-2023";`
    1. `var activeSections` change these to the sections that you want active
    1. `<h1 class="brand-heading">Swetugg <small>Göteborg</small></h1>`
    1. `<p class="intro-text">` change this to the correct date
    1. `<section id="previousLinks"` add a link to the previous conference (probably the one you copied the page from)
    1. `<h3>Location</h3>` change the location below this tag.
    1. `<meta property="og:description" content="Swetugg Göteborg, a conference for dotnet developers October 2023" />`
- `Now.cshtml`
    1. `var slug = "gbg-2023";`
- `Speakers.cshtml`
    1. `var slug = "gbg-2023";`
- `Sponsorship.cshtml`
    1. `<h1 class="noSubtitle">Sponsorship Göteborg 2023</h1>`

### Make the new page default (when you're ready)
1. Update this code in `Program.cs` to be the desired slug instead of "gbg-2023".
```
app.MapGet("/", async context =>
{
  context.Response.Redirect("/gbg-2023");
});
```
