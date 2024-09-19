using swetugg_public.DbModels;

namespace swetugg_public;

public static class ConferenceHelpers
{
    /// <summary>
    /// Convert the date to a DateTimeOffset with the correct timezone offset for this conference.
    /// </summary>
    /// <param name="dt">The date to convert</param>
    /// <param name="conf">The conference</param>
    /// <returns>A DateTimeOffset with the conference's timezone offset</returns>
    /// <remarks>
    ///   Currently this doesn't actually use the time zone of the conference since
    ///   we don't store that in the database. Instead the time zone is hard-coded to Europe/Stockholm.
    ///   We should add a new field on the Conference table to properly set the time zone.
    /// </remarks>
    public static DateTimeOffset ToConferenceDate(this DateTime dt, Conference conf)
    {
        var timeZoneName = "Europe/Stockholm";
        var tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
        var dto = new DateTimeOffset(dt, tzi.GetUtcOffset(dt));
        return dto;
    }
}