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

    public Conference? Get(string slug)
    {
        string normalizedSlug = slug.ToLower();
        string cacheKey = $"conference-{normalizedSlug}";
        if(!_cache.TryGetValue<Conference>(cacheKey, out Conference? conference))
        {
             conference = _context.Conferences
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
                .First(c => c.Slug.ToLower().Equals(normalizedSlug));

            if(conference != null)
                _cache.Set(cacheKey, conference, TimeSpan.FromMinutes(5));
        }

        return conference;
    }
}