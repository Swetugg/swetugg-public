using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using swetugg_public.DbModels;

namespace swetugg_public;

public class ConferenceService : IConferenceService
{
    private readonly SwetuggContext _context;
    private readonly IMemoryCache _cache;

    public ConferenceService(SwetuggContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public Conference? Get(string slug) =>
        ReadThroughCache<Conference?>($"conference-{slug.ToLower()}", () => _context.Conferences
            .AsSplitQuery()
            .Include(c => c.Rooms.OrderBy(c => c.Priority))
            .Include(c => c.Sessions.Where(s => s.Published))
                .ThenInclude(c => c.Speakers.Where(s => s.Published))
                    .ThenInclude(s => s.SpeakerImages)
                        .ThenInclude(s => s.ImageType)
            .Include(c => c.Slots.OrderBy(s => s.Start))
                .ThenInclude(s => s.RoomSlots)
                    .ThenInclude(rs => rs.AssignedSession)
                        .ThenInclude(s => s.Speakers)
            .Include(c => c.Speakers.Where(s => s.Published))
                .ThenInclude(s => s.SpeakerImages)
                    .ThenInclude(s => s.ImageType)
            .Include(c => c.Speakers.Where(s => s.Published))
                .ThenInclude(s => s.Tags)
            .Include(c => c.Sponsors.Where(s => s.Published))
                .ThenInclude(s => s.SponsorImages)
                    .ThenInclude(s => s.ImageType)
            .First(c => c.Slug.ToLower().Equals(slug.ToLower())));

    public CurrentConferenceMeta? GetCurrentConferenceMeta() =>
        ReadThroughCache("conference-current", () => _context.Conferences
            .Where(c => c.HighlightDate <= DateTime.UtcNow.Date)
            .OrderBy(c => c.End)
            .Select(c => new CurrentConferenceMeta { Name = c.Name, Slug = c.Slug })
            .FirstOrDefault()
        );

    public Speaker? GetSpeaker(string slug, string speakerSlug) =>
        ReadThroughCache<Speaker?>($"speaker-{speakerSlug.ToLower()}", () => _context.Speakers
            .AsSplitQuery()
            .Include(s => s.SpeakerImages)
                .ThenInclude(i => i.ImageType)
            .Include(s => s.Sessions.Where(session => session.Published))
                .ThenInclude(s => s.RoomSlots)
                    .ThenInclude(rs => rs.Slot)
            .Include(s => s.Sessions.Where(session => session.Published))
                .ThenInclude(s => s.RoomSlots)
                    .ThenInclude(rs => rs.Room)
            .Include(s => s.Tags)
            .First(s => s.Conference.Slug.ToLower().Equals(slug.ToLower()) && s.Slug.ToLower().Equals(speakerSlug.ToLower())));

    private T ReadThroughCache<T>(string cacheKey, Func<T> fill)
    {
        cacheKey = cacheKey.ToLower();
        if (!_cache.TryGetValue<T>(cacheKey, out T? value))
        {
            value = fill();

            if (value != null)
                _cache.Set(cacheKey, value, TimeSpan.FromMinutes(5));
        }

        return value;
    }
}

public class MockDataConferenceService : IConferenceService
{
    private readonly Conference _conference;
    public MockDataConferenceService()
    {
        var startDate = DateTime.Today.AddDays(-1).AddHours(8);
        var endDate = DateTime.Today.AddDays(1).AddHours(17);
        _conference = new Conference { Start = startDate, End = endDate };

        var currentTime = startDate;
        for (var roomIndex = 0; roomIndex < 3; roomIndex++)
        {
            var room = new Room { Id = roomIndex, Name = $"Room {roomIndex}" };
            _conference.Rooms.Add(room);
        }
        var index = 1;
        for (var dayIndex = 1; dayIndex <= 3; dayIndex++)
        {
            for (var speakerIndex = 0; speakerIndex < 9; speakerIndex++)
            {
                var slot = new Slot { Start = currentTime, End = currentTime.AddMinutes(55) };
                if (currentTime.Hour == 12)
                {
                    slot.Title = "Lunch";
                }
                else
                {
                    for (var roomIndex = 0; roomIndex < _conference.Rooms.Count; roomIndex++)
                    {
                        var room = _conference.Rooms.ElementAt(roomIndex);
                        var speaker = new Speaker { Id = index, Name = $"Speaker {index}", Published = true, Slug = $"speakerslug{index}" };
                        var session = new Session { Id = index, Name = $"Session {index}", Published = true, Slug = $"sessionslug{index}" };
                        session.Speakers.Add(speaker);
                        speaker.Sessions.Add(session);
                        var roomSlot = new RoomSlot { RoomId = roomIndex, AssignedSession = session, Start = currentTime, End = currentTime.AddMinutes(55) };
                        room.RoomSlots.Add(roomSlot);
                        slot.RoomSlots.Add(roomSlot);

                        _conference.Sessions.Add(session);
                        _conference.Speakers.Add(speaker);
                        index++;
                    }
                }
                _conference.Slots.Add(slot);
                currentTime = currentTime.AddHours(1);
            }
            currentTime = currentTime.AddDays(1).AddHours(-9);
        }
    }
    public Conference? Get(string slug) =>
        _conference;

    public CurrentConferenceMeta? GetCurrentConferenceMeta() => null;

    public Speaker? GetSpeaker(string slug, string speakerSlug) =>
        _conference.Speakers.FirstOrDefault(s => s.Slug == speakerSlug);
}

public interface IConferenceService
{
    Conference? Get(string slug);
    CurrentConferenceMeta? GetCurrentConferenceMeta();
    Speaker? GetSpeaker(string slug, string speakerSlug);
}

public class CurrentConferenceMeta
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}