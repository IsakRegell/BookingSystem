using System;
using System.Collections.Generic;

namespace BookingSystem.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public int LevelId { get; set; }

    public virtual ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();

    public virtual ClassLevel Level { get; set; } = null!;
}
