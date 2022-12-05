using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class SpeakerImage
{
    public int Id { get; set; }

    public int SpeakerId { get; set; }

    public int ImageTypeId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual ImageType ImageType { get; set; } = null!;

    public virtual Speaker Speaker { get; set; } = null!;
}
