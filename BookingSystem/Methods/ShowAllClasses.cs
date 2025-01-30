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
            Console.Clear();

            using (var dbContext = new BookingSystemContext())
            {

                var classes = dbContext.Classes
                    .Select(c => new
                    {
                        c.ClassName,
                        Schedules = c.ClassSchedules
                            .OrderBy(s => s.StartDate) // Sortera på StartDate
                            .Select(s => new
                            {
                                s.StartDate,
                                s.EndDate,
                                InstructorName = $"{s.Instructor.FirstName} {s.Instructor.LastName}"
                            })
                            .ToList(), // Gör det till en lista för att undvika LINQ-problem
                        LevelName = c.Level.LevelName
                    })
                    .ToList(); // Hämtar allt till minnet för att använda LINQ

                if (!classes.Any())
                {
                    Console.WriteLine("No classes available in the system.");
                    return;
                }

                Console.WriteLine("All Classes (Sorted by Start Date):");
                Console.WriteLine("______________________________________\n");
                foreach (var classObj in classes)
                {
                    Console.WriteLine($"Class Name: {classObj.ClassName}");
                    Console.WriteLine($"Level: {classObj.LevelName}");

                    foreach (var schedule in classObj.Schedules)
                    {
                        Console.WriteLine($"Schedule: {schedule.StartDate:yyyy-MM-dd} to {schedule.EndDate:yyyy-MM-dd}");
                        Console.WriteLine($"Instructor: {schedule.InstructorName}");
                    }


                    Console.WriteLine("___________________________________\n");
                }

                Console.WriteLine("\nPress any key to move forward...");
                Console.ReadKey();

            }

        }
    }
}
