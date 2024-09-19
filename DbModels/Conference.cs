using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class Conference
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }

    public DateTime? HighlightDate { get; set; }

    public int MinNumberOfSpeakers { get; set; }

    public virtual ICollection<ImageType> ImageTypes { get; } = new List<ImageType>();

    public virtual ICollection<Room> Rooms { get; } = new List<Room>();

    public virtual ICollection<SessionType> SessionTypes { get; } = new List<SessionType>();

    public virtual ICollection<Session> Sessions { get; } = new List<Session>();

    public virtual ICollection<Slot> Slots { get; } = new List<Slot>();

    public virtual ICollection<Speaker> Speakers { get; } = new List<Speaker>();

    public virtual ICollection<Sponsor> Sponsors { get; } = new List<Sponsor>();
}
