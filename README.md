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
We're currently using Sass to build our CSS (subject to change). If you're using VS Code and have installed the workspace recommended extensions (https://marketplace.visualstudio.com/items?itemName=glenn2223.live-sass) then your CSS should be built on save after you've turned on live sass.

Why are the generated css files commited instead of ignored? This is also subject to change, the goal is to have them either generated as part of the build pipeline, or have them generated on demand through the hosting pipeline.

## Set up a new conference

### Setup database
1. In backoffice create a new conference with the desired slug (the slug is the name of the folder in the Pages folder).

### Create the new pages
1. Copy the `Pages/{slug}` folder of the latest conference (assuming that you want the new page to look like the previous one).
1. Use the desired slug (the one that you configured for the Conference database row) for the new conference as the name of the copied folder.
1. Do the same for the `wwwroot/{slug}` folder. Your life will be easier if you pick the folder of the last conference that ran in the same city as the conference you're setting up, as we have different assets (mainly the colors of images and css variables) for different cities. But you should then copy over the swetugg.scss file from the last conference (regardless of city) as we've typically made style changes that we want to keep, and replace the variables section (tope of the .scss file) with the values from the last conference that ran in the same city as the conference you're setting up (this is a bit of a hassle, we should perhaps extract these into a variables.scss, or try even harder to use css variables)

### Update the new pages
Change the following properties of the pages
- `_Layout.cshtml`
    1. `var slug = "gbg-2023";`.
    1. `<meta property="og:site_name" content="Swetugg Göteborg 2023" />`
    1. Update which links are enabled in `<div class="collapse navbar-collapse navbar-right navbar-main-collapse">`. This is currently done by commenting in/out links.
- `Index.cshtml`
    1. `var slug = "gbg-2023";`
    1. `var activeSections` change these to the sections that you want active
    1. `<h1 class="brand-heading">Swetugg <small>Göteborg</small></h1>`
    1. `<p class="intro-text">` change this to the correct date
    1. `<section id="previousLinks"` add a link to the previous conference (probably the one you copied the page from)
    1. Update `<a href="https://sessionize.com/swetugg-goteborg-2024" class="btn btn-default btn-lg">Call for Speakers</a>` to the correct sessionize link. And the date for CFP end found in the `<p>` above.
    1. `<h3>Location</h3>` change the location below this tag.
    1. `<meta property="og:description" content="Swetugg Göteborg, a conference for dotnet developers October 2023" />`
- `Now.cshtml`
    1. `var slug = "gbg-2023";`
- `Speakers.cshtml`
    1. `var slug = "gbg-2023";`
- `Sponsorship.cshtml`
    1. `<h1 class="noSubtitle">Sponsorship Göteborg 2023</h1>`

### Update CSS
- In `swetugg.scss` Change any reference to the old slug, such as `/gbg-2024/img/top.jpg` to point to the new slug.
- Remember to build CSS from the SCSS file

### Make the new page default (when you're ready)
1. Update this code in `Program.cs` to be the desired slug instead of "gbg-2023".
```
app.MapGet("/", async context =>
{
  context.Response.Redirect("/gbg-2023");
});
```
