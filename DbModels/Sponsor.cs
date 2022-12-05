using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class Sponsor
{
    public int Id { get; set; }

    public int ConferenceId { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public string? Web { get; set; }

    public string? Twitter { get; set; }

    public bool Published { get; set; }

    public int Priority { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual ICollection<SponsorImage> SponsorImages { get; } = new List<SponsorImage>();
}
