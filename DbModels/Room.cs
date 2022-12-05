using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class Room
{
    public int Id { get; set; }

    public int ConferenceId { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public int Priority { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual ICollection<RoomSlot> RoomSlots { get; } = new List<RoomSlot>();
}
