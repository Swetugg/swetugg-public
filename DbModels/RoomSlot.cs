using System;
using System.Collections.Generic;

namespace swetugg_public.DbModels;

public partial class RoomSlot
{
    public int RoomId { get; set; }

    public int SlotId { get; set; }

    public int? AssignedSessionId { get; set; }

    public bool IsChanged { get; set; }

    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }

    public virtual Session? AssignedSession { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual Slot Slot { get; set; } = null!;
}
