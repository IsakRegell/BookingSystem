using System;
using System.Collections.Generic;

namespace BookingSystem.Models;

public partial class ClassSchedule
{
    public int ScheduleId { get; set; }

    public int ClassId { get; set; }

    public int InstructorId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Instructor Instructor { get; set; } = null!;
}
