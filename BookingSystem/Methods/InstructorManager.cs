using System;                                // Basic system functionalities (Console, etc.)
using System.Collections.Generic;            // Generic collections (List<T>, etc.)
using System.Linq;                           // Enables LINQ methods (ToList, FirstOrDefault, etc.)
using System.Text;                           // Classes for working with strings (StringBuilder, etc.)
using System.Threading.Tasks;                // Provides types for asynchronous programming
using Microsoft.EntityFrameworkCore;         // Entity Framework Core functionalities (DbUpdateException, etc.)
using BookingSystem.Models;                  // Gives access to database models (e.g., Instructor, BookingSystemContext)

namespace BookingSystem.Models
{
    public static class InstructorManager
    {
        // Method to add a new instructor to the database
        public static void AddInstructor()
        {
            // Create the database context
            using (var context = new BookingSystemContext())
            {
                Console.Clear();                                      // Clear the console screen for neatness
                Console.Write("Enter Instructor First Name: ");       // Prompt the user
                string firstName = Console.ReadLine();                // Read the first name from user input

                Console.Write("Enter Instructor Last Name: ");        // Prompt the user
                string lastName = Console.ReadLine();                 // Read the last name

                Console.Write("Enter Dance Style: ");                 // Prompt the user
                string style = Console.ReadLine();                    // Read the dance style

                // Create a new Instructor object with the gathered info
                var newInstructor = new Instructor
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Style = style
                };

                try
                {
                    // Here we again create a context (though context is already open—this could be streamlined)
                    using (var BookingSystemcontext = new BookingSystemContext())
                    {
                        // Add the new instructor to the Instructors collection
                        context.Instructors.Add(newInstructor);
                        // Persist the changes to the database
                        context.SaveChanges();

                        Console.WriteLine("Instructor added successfully!");
                    }
                }
                catch (DbUpdateException ex)
                {
                    // If an error occurs related to the database update, display a message and the error details
                    Console.WriteLine("An error occurred while saving the instructor to the database.");
                    Console.WriteLine($"Error details: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Catch any other unanticipated exceptions
                    Console.WriteLine("An unexpected error occurred.");
                    Console.WriteLine($"Error details: {ex.Message}");
                }
            }
        }

        // Method to retrieve and display all instructors
        public static void ViewAllInstructors()
        {
            // Open a new context for database operations
            using (var context = new BookingSystemContext())
            {
                // Get all instructors as a list from the database
                var instructors = context.Instructors.ToList();

                Console.Clear();                                             // Clear the console for neatness
                Console.WriteLine("====== All Instructors ======");          // Print a header

                // Loop through each instructor and display their info
                foreach (var instructor in instructors)
                {
                    Console.WriteLine(
                        $"ID: {instructor.InstructorId}, " +
                        $"Name: {instructor.FirstName} {instructor.LastName}, " +
                        $"Style: {instructor.Style}"
                    );
                }

                // Prompt user to press any key to continue
                Console.WriteLine("Press any key to move forward");
                Console.ReadKey();
            }
        }

        // Method to delete an instructor by their ID
        public static void DeleteInstructor()
        {
            // Create a new context
            using (var context = new BookingSystemContext())
            {
                Console.Clear();
                // Ask the user for the ID of the instructor to delete
                Console.Write("Enter Instructor ID to delete: ");
                var input = Console.ReadLine();

                // Validate if the user input can be parsed as an integer
                if (!int.TryParse(input, out int instructorId))
                {
                    Console.WriteLine("Invalid ID. Please enter a numeric value.");
                    return;
                }

                // Attempt to find the instructor in the database
                var instructor = context.Instructors.Find(instructorId);
                if (instructor == null)
                {
                    // If not found, inform the user and stop
                    Console.WriteLine("Instructor not found.");
                    return;
                }

                // Remove the instructor from the context
                context.Instructors.Remove(instructor);
                // Save changes to the database
                context.SaveChanges();
                Console.WriteLine("Instructor deleted successfully!");

                // Prompt user to press any key to continue
                Console.WriteLine("\nPress any key to move forward");
                Console.ReadKey();
            }
        }
    }
}
