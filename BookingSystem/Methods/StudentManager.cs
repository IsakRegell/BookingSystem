using Microsoft.EntityFrameworkCore;          // Entity Framework Core functionality (e.g., DbUpdateException)
using System;                                // Basic system functionalities (Console, etc.)
using System.Collections.Generic;            // Provides generic collection classes (List<T>, etc.)
using System.Linq;                           // Enables LINQ methods (ToList, FirstOrDefault, etc.)
using System.Text;                           // For text manipulation (StringBuilder, etc.)
using System.Threading.Tasks;                // For asynchronous programming (not used here)
using BookingSystem.Models;                  // Gives access to the BookingSystemContext, Student class, etc.

namespace BookingSystem.Models
{
    public static class StudentManager
    {
        // Method for adding a new student to the system
        public static void AddStudent()
        {
            Console.Clear(); // Clear the console for a clean user interface

            // Create a new database context to interact with the database
            using (var context = new BookingSystemContext())
            {
                // Prompt the user for the new student's first name
                Console.Write("Enter Student First Name: ");
                string firstName = Console.ReadLine();

                // Prompt the user for the new student's last name
                Console.Write("Enter Student Last Name: ");
                string lastName = Console.ReadLine();

                // Create a new Student object with the provided info
                var newStudent = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                };

                try
                {
                    // Add the new Student to the database context
                    context.Students.Add(newStudent);
                    // Commit the changes to the database
                    context.SaveChanges();

                    // Inform the user that the student was successfully added
                    Console.WriteLine("Student added successfully!");
                    // Pause so the user can see the message
                    Console.ReadLine(); // or Console.ReadKey()
                }
                catch (DbUpdateException ex)
                {
                    // Handle any database-specific errors
                    Console.WriteLine("An error occurred while saving the student to the database.");
                    Console.WriteLine($"Error details: {ex.Message}");
                    Console.ReadLine(); // Pause to let user read the error
                }
                catch (Exception ex)
                {
                    // Handle any other unexpected errors
                    Console.WriteLine("An unexpected error occurred.");
                    Console.WriteLine($"Error details: {ex.Message}");
                }

                // Prompt user to press a key to continue
                Console.WriteLine("Press any key to move forward");
                Console.ReadKey();
            }
        }

        // Method for displaying all students in the system
        public static void ViewAllStudents()
        {
            Console.Clear(); // Clear the console for clarity

            // Open a new database context for retrieval
            using (var context = new BookingSystemContext())
            {
                // Fetch all students from the database as a list
                var students = context.Students.ToList();

                // Display header
                Console.WriteLine("====== All Students ======");
                // Loop through each student and print basic info
                foreach (var student in students)
                {
                    Console.WriteLine(
                        $"ID: {student.Student_Id}, " +
                        $"Name: {student.FirstName} {student.LastName}"
                    );
                }

                // Pause so the user can see the list before returning
                Console.WriteLine("Press any key to move forward");
                Console.ReadKey();
            }
        }

        // Method for deleting a student based on their ID
        public static void DeleteStudent()
        {
            Console.Clear(); // Clear the console

            // Create a database context to find and remove a student record
            using (var context = new BookingSystemContext())
            {
                // Prompt user for the student ID
                Console.Write("Enter Student ID to delete: ");
                var input = Console.ReadLine();

                // Validate the input as an integer
                if (!int.TryParse(input, out int studentId))
                {
                    // If validation fails, inform the user and exit the method
                    Console.WriteLine("Invalid ID. Please enter a numeric value.");
                    return;
                }

                // Attempt to find a matching student in the database
                var student = context.Students.Find(studentId);
                if (student == null)
                {
                    // If no student is found with that ID, let the user know
                    Console.WriteLine("Student not found.");
                    return;
                }

                // Remove the found student from the context
                context.Students.Remove(student);
                // Persist the changes (deletion) to the database
                context.SaveChanges();

                // Inform the user that the student was deleted successfully
                Console.WriteLine("Student deleted successfully!");
                Console.WriteLine("\nPress any key to move forward");
                Console.ReadKey();
            }
        }
    }
}
