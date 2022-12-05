using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class Tag
{
    public int Id { get; set; }

    public int ConferenceId { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public bool Featured { get; set; }

    public string? Description { get; set; }

    public int Priority { get; set; }

    public int Type { get; set; }

    public virtual ICollection<Session> Sessions { get; } = new List<Session>();

    public virtual ICollection<Speaker> Speakers { get; } = new List<Speaker>();
}
