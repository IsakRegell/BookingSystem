using BookingSystem.Models;                      // Gives access to the database context and model classes (Class, Instructor, etc.)
using System;                                    // Basic system functionality (Console, etc.)
using System.Collections.Generic;               // Provides generic collection classes (List, etc.)
using System.Linq;                               // Enables LINQ methods (Select, Where, etc.)
using System.Text;                               // For text manipulation (StringBuilder, etc.)
using System.Threading.Tasks;                    // For asynchronous programming

namespace BookingSystem.Methods
{
    public class ShowAllClasses
    {
        // We keep a reference to the BookingSystemContext, which we use to query the database
        private readonly BookingSystemContext dbContext;

        // Constructor that requires a valid BookingSystemContext. If null is passed, it will throw an ArgumentNullException.
        public ShowAllClasses(BookingSystemContext context)
        {
            dbContext = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
        }

        // Method to display all classes, their schedules, instructors, and level
        public void DisplayAllClasses()
        {
            // Clear the console screen before showing the classes
            Console.Clear();

            // Query the Classes table in the database, then project each Class into a new anonymous object
            var classes = dbContext.Classes
                .Select(c => new
                {
                    c.ClassName,                                  // Class Name
                    Schedules = c.ClassSchedules.Select(s => new
                    {
                        s.StartDate,                             // Start date of the schedule
                        s.EndDate,                               // End date of the schedule
                        Instructor = new
                        {
                            s.Instructor.FirstName,              // Instructor first name
                            s.Instructor.LastName                // Instructor last name
                        }
                    }),
                    LevelName = c.Level.LevelName                // Name of the Class Level (e.g., Beginner/Intermediate/Advanced)
                })
                .ToList();

            // Check if no classes exist in the system
            if (!classes.Any())
            {
                Console.WriteLine("No classes available in the system.");
                return; // End the method if there are no classes to display
            }

            // Print a header
            Console.WriteLine("All Classes:");

            // Loop over each class object we just retrieved
            foreach (var classObj in classes)
            {
                // Print the class name
                Console.WriteLine($"Class Name: {classObj.ClassName}");

                // Each class can have multiple schedules, so iterate through them
                foreach (var schedule in classObj.Schedules)
                {
                    // Display schedule dates
                    Console.WriteLine($"  - Schedule: {schedule.StartDate:yyyy-MM-dd} to {schedule.EndDate:yyyy-MM-dd}");
                    // Display the instructor's name
                    Console.WriteLine($"  - Instructor: {schedule.Instructor.FirstName} {schedule.Instructor.LastName}");
                }

                // Show the level name (Beginner, Intermediate, Advanced, etc.)
                Console.WriteLine($"  - Level: {classObj.LevelName}");
            }

            // Prompt user to press any key to move on
            Console.WriteLine("Press any key to move forward");
            Console.ReadKey();
        }
    }
}
