using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class ImageType
{
    public int Id { get; set; }

    public int ConferenceId { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public int? Width { get; set; }

    public int? Height { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual ICollection<SpeakerImage> SpeakerImages { get; } = new List<SpeakerImage>();

    public virtual ICollection<SponsorImage> SponsorImages { get; } = new List<SponsorImage>();
}
