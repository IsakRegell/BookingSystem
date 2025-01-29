using BookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Methods
{
    public static class ShowAllClasses
    {
        public static void DisplayAllClasses()
        {
        using (var context = new BookingSystemContext())
        {
                var classes = context.Classes
            .Select(c => new
            {
                c.ClassName,
                Schedules = c.ClassSchedules.Select(s => new
                {
                    s.StartDate,
                    s.EndDate,
                    Instructor = new
                    {
                        s.Instructor.FirstName,
                        s.Instructor.LastName
                    }
                }),
                LevelName = c.Level.LevelName
            }).ToList();

        if (!classes.Any())
        {
            Console.WriteLine("No classes available in the system.");
            return;
        }

        Console.WriteLine("All Classes:");
        foreach (var classObj in classes)
        {
            Console.WriteLine($"Class Name: {classObj.ClassName}");

            foreach (var schedule in classObj.Schedules)
            {
                Console.WriteLine($"  - Schedule: {schedule.StartDate:yyyy-MM-dd} to {schedule.EndDate:yyyy-MM-dd}");
                Console.WriteLine($"  - Instructor: {schedule.Instructor.FirstName} {schedule.Instructor.LastName}");
            }

            Console.WriteLine($"  - Level: {classObj.LevelName}");
        }

    }
}
    }
}
