using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class Slot
{
    public int Id { get; set; }

    public int ConferenceId { get; set; }

    public string? Title { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public bool HasSessions { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual ICollection<RoomSlot> RoomSlots { get; } = new List<RoomSlot>();
}
