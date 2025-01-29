using BookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Methods
{
    public class ShowAllClasses
    {
        private readonly BookingSystemContext dbContext;

        // Konstruktor för att initialisera listan

        public ShowAllClasses(BookingSystemContext context)
        {
            dbContext = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
        }

        public void DisplayAllClasses()
        {

            Console.Clear();
            var classes = dbContext.Classes
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

            Console.WriteLine("Press any key to move forward");
            Console.ReadKey();

        }
    }
}
