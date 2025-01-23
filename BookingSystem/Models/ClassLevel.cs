using System;
using System.Collections.Generic;

namespace BookingSystem.Models;

public partial class ClassLevel
{
    public int LevelId { get; set; }

    public string LevelName { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
