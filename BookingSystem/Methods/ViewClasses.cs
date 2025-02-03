using BookingSystem.Models;                      // Gives access to the Classes, ClassSchedules, and BookingSystemContext models
using Microsoft.EntityFrameworkCore;             // Entity Framework Core functionality (e.g., DbContext, DbUpdateException)
using Microsoft.Identity.Client;                 // (Not used here, likely referenced for other project components)
using Microsoft.IdentityModel.Tokens;            // (Not used here, likely referenced for other project components)
using System;                                    // Basic system functionality (Console, etc.)
using System.Collections.Generic;                // Generic collection classes (List<T>, etc.)
using System.Linq;                               // Enables LINQ methods (Select, Where, etc.)
using System.Text;                               // For text manipulation
using System.Threading.Tasks;                    // For asynchronous programming

namespace BookingSystem.Methods
{
    public static class ViewClasses
    {
        // This method retrieves and displays all classes ordered by start date
        public static void ViewClassesByDate()
        {
            // Clear the console screen for neat output
            Console.Clear();

            // Create a new database context to interact with the database
            using (var dbcontext = new BookingSystemContext())
            {
                // Query the Classes table, then shape the data into an anonymous object
                var classes = dbcontext.Classes

                    .Select(c => new
                    {
                        c.ClassName,                                     // The name of the class
                        // For each class, fetch and order all schedules by start date
                        Schedules = c.ClassSchedules.OrderBy(s => s.StartDate).Select(s => new
                        {
                            s.StartDate,                                 // The beginning of the schedule
                            s.EndDate,                                   // The end of the schedule
                            Instructor = new
                            {
                                s.Instructor.FirstName,                  // The instructor's first name
                                s.Instructor.LastName                    // The instructor's last name
                            }
                        })
                        .ToList(), // Convert schedules to a list to avoid potential expression tree issues
                        LevelName = c.Level.LevelName                    // Name of the class level (e.g., Beginner, Intermediate, Advanced)
                    })
                    // Convert the result to IEnumerable for in-memory operations (helpful when dealing with LINQ after projecting data)
                    .AsEnumerable();

                // Check if there are no classes in the system
                if (!classes.Any())
                {
                    Console.WriteLine("No classes available in the system..");
                    return; // Exit the method if no classes
                }

                // Print a header
                Console.WriteLine("\nAll Classes (Ordered by Start Date):");

                // Loop over each class object
                foreach (var classObj in classes)
                {
                    // Display the class name
                    Console.WriteLine($"Class Name: {classObj.ClassName}");

                    // For each schedule in the class
                    foreach (var schedule in classObj.Schedules)
                    {
                        // Show the scheduled date range
                        Console.WriteLine($"  - Schedule: {schedule.StartDate:yyyy-MM-dd} to {schedule.EndDate:yyyy-MM-dd}");
                        // Show the instructor’s name
                        Console.WriteLine($"  - Instructor: {schedule.Instructor.FirstName} {schedule.Instructor.LastName}");
                    }

                    // Display the class level (e.g., Beginner, Intermediate, Advanced)
                    Console.WriteLine($"  - Level: {classObj.LevelName}");

                    // Prompt the user to press a key before moving to the next class listing
                    Console.WriteLine("Press any key to move forward");
                    Console.ReadKey();
                }
            }
        }
    }
}
