using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class Session
{
    public int Id { get; set; }

    public int ConferenceId { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public string? VideoUrl { get; set; }

    public bool VideoPublished { get; set; }

    public bool Published { get; set; }

    public int Priority { get; set; }

    public int? SessionTypeId { get; set; }

    public int? SessionizeId { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual ICollection<RoomSlot> RoomSlots { get; } = new List<RoomSlot>();

    public virtual SessionType? SessionType { get; set; }

    public virtual ICollection<Speaker> Speakers { get; } = new List<Speaker>();

    public virtual ICollection<Tag> Tags { get; } = new List<Tag>();
}
