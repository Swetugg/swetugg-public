using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class Speaker
{
    public int Id { get; set; }

    public int ConferenceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Company { get; set; }

    public string Slug { get; set; } = null!;

    public string? Bio { get; set; }

    public string? Web { get; set; }

    public string? Twitter { get; set; }

    public string? GitHub { get; set; }

    public string? LinkedIn { get; set; }

    public bool Published { get; set; }

    public int Priority { get; set; }

    public Guid? SessionizeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? SessionizeImageUrl { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual ICollection<SpeakerImage> SpeakerImages { get; } = new List<SpeakerImage>();

    public virtual ICollection<Session> Sessions { get; } = new List<Session>();

    public virtual ICollection<Tag> Tags { get; } = new List<Tag>();
}
