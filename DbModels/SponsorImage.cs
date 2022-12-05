using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class SponsorImage
{
    public int Id { get; set; }

    public int SponsorId { get; set; }

    public int ImageTypeId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual ImageType ImageType { get; set; } = null!;

    public virtual Sponsor Sponsor { get; set; } = null!;
}
