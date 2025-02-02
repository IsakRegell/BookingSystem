using BookingSystem.Models;                 // Import the namespace that contains our database models
using System;                               // Provides fundamental classes and base classes
using System.Collections.Generic;           // Provides generic collection classes (e.g., List<T>)
using System.Globalization;                 // Provides classes for culture-specific operations (e.g., date/time formatting)
using System.Linq;                          // Provides language-integrated query functionality (LINQ)
using System.Text;                          // Provides classes for string manipulation
using System.Threading.Tasks;               // Provides types for asynchronous programming

namespace BookingSystem.Methods
{
    public class ClassesManager
    {
        // This method handles the process of adding a new class to the system
        public static void AddClass()
        {
            // Create a context instance to interact with the database
            using (var context = new BookingSystemContext())
            {
                // Ask the user for a class name and validate that it's not empty
                string className = GetValidInput("Enter Class Name:", "Class name cannot be empty.");

                // Ask the user for a start date (must be in yyyy-MM-dd or yyyyMMdd format)
                DateOnly startDate = GetValidDate("Enter Start Date (yyyy-MM-dd or yyyyMMdd):");

                // Ask the user for an end date (must be in yyyy-MM-dd or yyyyMMdd format)
                // and ensure it is on or after the start date
                DateOnly endDate = GetValidDate("Enter End Date (yyyy-MM-dd or yyyyMMdd):", startDate);

                // Let the user choose an instructor from the database
                var selectedInstructor = SelectInstructor(context);
                // If no instructor was properly chosen, stop the operation
                if (selectedInstructor == null)
                {
                    Console.WriteLine("No valid instructor selected. Operation aborted.");
                    return; // Exit the method early
                }

                // Let the user pick a class level (Beginner, Intermediate, or Advanced)
                int levelId = SelectClassLevel();
                // If the choice is invalid, stop the operation
                if (levelId == 0)
                {
                    Console.WriteLine("Invalid level selected. Operation aborted.");
                    return; // Exit the method early
                }

                // Retrieve the chosen level from the database
                var level = context.ClassLevels.FirstOrDefault(l => l.LevelId == levelId);
                // If the selected level doesn't exist in the database, stop
                if (level == null)
                {
                    Console.WriteLine("Error: Selected level does not exist.");
                    return;
                }

                // Create a new ClassSchedule object with the provided dates and instructor
                var schedule = new ClassSchedule
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    InstructorId = selectedInstructor.InstructorId
                };

                // Create a new Class object, associating it with the schedule and chosen level
                var newClass = new Class
                {
                    ClassName = className,
                    ClassSchedules = new List<ClassSchedule> { schedule },
                    LevelId = level.LevelId
                };

                // Add the new class to the database context (prepares it to be saved)
                context.Classes.Add(newClass);
                // Persist the changes to the actual database
                context.SaveChanges();

                // Inform the user that the class was successfully added
                Console.WriteLine(
                    $"Class '{className}' added successfully for {startDate:yyyy-MM-dd} with teacher {selectedInstructor.FirstName} {selectedInstructor.LastName}.");

                // Pause so the user can see the message, then clear the console
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        // This method repeatedly asks the user for a non-empty string input
        // It displays 'prompt' and if the input is invalid, displays 'errorMessage'
        private static string GetValidInput(string prompt, string errorMessage)
        {
            while (true) // Keep asking until a valid input is provided
            {
                Console.WriteLine(prompt);                // Show the prompt
                string input = Console.ReadLine()?.Trim(); // Read user input and trim any whitespace

                // If the input is not empty, return it
                if (!string.IsNullOrEmpty(input))
                    return input;

                // Otherwise, show the error message and repeat
                Console.WriteLine(errorMessage);
            }
        }

        // This method gets a valid date from the user, allowing yyyy-MM-dd or yyyyMMdd formats
        // If 'minDate' is provided, the date the user enters must be >= 'minDate'
        private static DateOnly GetValidDate(string prompt, DateOnly? minDate = null)
        {
            // The allowed date formats
            string[] formats = { "yyyy-MM-dd", "yyyyMMdd" };

            while (true) // Keep asking until valid date is provided
            {
                Console.WriteLine(prompt); // Show the prompt
                string input = Console.ReadLine()?.Trim(); // Read user input

                // Try parsing the input date using the allowed formats
                if (DateOnly.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
                {
                    // Check if date meets the minDate requirement
                    if (minDate == null || date >= minDate)
                        return date; // Valid date found, return it

                    // If date is before minDate, show an error
                    Console.WriteLine("End date cannot be earlier than start date. Please try again.");
                }
                else
                {
                    // If parsing fails, show an error and allow the user to try again
                    Console.WriteLine("Invalid date format. Please try again.");
                    Console.WriteLine("Allowed formats: yyyy-MM-dd or yyyyMMdd.");
                }
            }
        }

        // This method repeatedly asks the user for an integer input and validates it
        private static int GetValidIntInput(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);        // Show the prompt
                string input = Console.ReadLine(); // Read input as string

                // Try to convert the input into an integer
                if (int.TryParse(input, out int result))
                    return result; // If valid, return the integer

                // If invalid, show an error message and repeat
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

        // This method displays class level options and returns the user's choice (1, 2, or 3)
        private static int SelectClassLevel()
        {
            Console.WriteLine("Select Class Level:");
            Console.WriteLine("1 - Beginner");
            Console.WriteLine("2 - Intermediate");
            Console.WriteLine("3 - Advanced");

            while (true) // Keep asking until the user picks one of the valid options
            {
                int levelId = GetValidIntInput("Enter Level ID:");
                // If within range (1 to 3), return that ID
                if (levelId >= 1 && levelId <= 3)
                    return levelId;

                // Otherwise, inform the user it's invalid
                Console.WriteLine("Invalid Level ID. Please select 1, 2, or 3.");
            }
        }

        // This method lets the user select an instructor from a list of all instructors in the database
        private static Instructor SelectInstructor(BookingSystemContext context)
        {
            // Retrieve all instructors from the database
            var instructors = context.Instructors.ToList();

            // If there are no instructors, inform the user and return null
            if (!instructors.Any())
            {
                Console.WriteLine("No available instructors.");
                return null;
            }

            // Display each instructor's ID and name
            Console.WriteLine("Available Teachers:");
            foreach (var instructor in instructors)
            {
                Console.WriteLine($"{instructor.InstructorId}: {instructor.FirstName} {instructor.LastName}");
            }

            // Let the user pick an instructor by typing in their ID
            while (true)
            {
                int instructorId = GetValidIntInput("Select a Teacher by ID:");

                // Check if an instructor with that ID exists
                var selectedInstructor = instructors.FirstOrDefault(i => i.InstructorId == instructorId);
                if (selectedInstructor != null)
                    return selectedInstructor; // If found, return it

                // If not found, tell the user it's invalid and ask again
                Console.WriteLine("Invalid Teacher ID. Please try again.");
            }

            // Unreachable code theoretically, but required by compiler
            return null;
        }
    }
}
