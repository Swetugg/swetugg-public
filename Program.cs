using Microsoft.EntityFrameworkCore;
using swetugg_public;
using swetugg_public.DbModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<SwetuggContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Swetugg"))
  .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddScoped<IConferenceService, ConferenceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/", async context =>
{
  context.Response.Redirect("/gbg-2024");
});

app.MapGet("/{slug}/now-feed", (string slug, IConferenceService conferenceService) =>
{
  // TODO: Fix timezones
  var conference = conferenceService.Get(slug);
  return conference?.Slots.Select(s => new
  {
    Start = s.Start.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff+02:00"),
    End = s.End.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff+02:00"),
    s.Title,
    Sessions = s.RoomSlots.Where(rs => rs.AssignedSession != null && rs.AssignedSession.Published).Select(r => new
    {
      Room = conference?.Rooms?.Where(room => room.Id == r.RoomId).FirstOrDefault()?.Name,
      r.AssignedSession?.Name,
      r.AssignedSession?.Description,
      Start = r.Start?.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff+02:00"),
      End = r.End?.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff+02:00"),
      Speakers = r.AssignedSession?.Speakers.Select(speaker => new
      {
        speaker.Name,
        speaker.Slug
      }),
      Tags = r.AssignedSession?.Tags.Where(t => t.Featured).Select(tag => new
      {
        tag.Name,
        tag.Description,
      })
    })
  });
});

app.MapRazorPages();

app.Run();
