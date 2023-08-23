using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using swetugg_public.DbModels;

namespace swetugg_public;

public class ConferenceService
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
            .Include(c => c.Rooms)
            .Include(c => c.Sessions.Where(s => s.Published))
            .Include(c => c.Slots)
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

    public Speaker? GetSpeaker(string slug, string speakerSlug) =>
        ReadThroughCache<Speaker?>($"speaker-{speakerSlug.ToLower()}", () => _context.Speakers
            .AsSplitQuery()
            .Include(s => s.SpeakerImages)
                .ThenInclude(i => i.ImageType)
            .Include(s => s.Sessions.Where(session => session.Published))
            .Include(s => s.Tags)
            .First(s => s.Conference.Slug.ToLower().Equals(slug.ToLower()) && s.Slug.ToLower().Equals(speakerSlug.ToLower())));

    public T ReadThroughCache<T>(string cacheKey, Func<T> fill)
    {
        cacheKey = cacheKey.ToLower();
        if(!_cache.TryGetValue<T>(cacheKey, out T? value))
        {
            value = fill();

            if(value != null)
                _cache.Set(cacheKey, value, TimeSpan.FromMinutes(5));
        }

        return value;
    }
}