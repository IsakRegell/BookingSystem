using BookingSystem.Models;                       // Gives access to the database models (e.g., Student, DanceStyle)
using Microsoft.EntityFrameworkCore;              // Provides Entity Framework Core functionalities (DbUpdateException, etc.)
using System;                                     // Basic system functions (Console, etc.)
using System.Linq;                                // Enables LINQ methods like FirstOrDefault, ToList, etc.

namespace BookingSystem.Models
{
    public class BookAClass
    {
        public void BookClass()
        {
            // Ask the user whether they are a new student (Y) or existing student (N)
            Console.WriteLine("Are you a new student? Y/N : ");
            var MemberchoiceYorN = Console.ReadLine();

            // Create a context object to interact with the database
            using (var context = new BookingSystemContext())
            {
                // We'll store a student reference here after we figure out if they're new or existing
                Student student = null;

                // If the user indicates they're new with 'Y' (case-insensitive check)
                if (MemberchoiceYorN?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // Ask for first name
                    Console.Write("Enter First Name: ");
                    var firstName = Console.ReadLine();

                    // Ask for last name
                    Console.Write("Enter Last Name: ");
                    var lastName = Console.ReadLine();

                    // Create a new student object
                    student = new Student
                    {
                        FirstName = firstName,
                        LastName = lastName
                    };

                    // Add the new student to the context's Students collection
                    context.Students.Add(student);

                    try
                    {
                        // Save changes (inserts the new student into the database)
                        context.SaveChanges();
                        Console.WriteLine("New student created successfully!");
                    }
                    catch (DbUpdateException dbEx)
                    {
                        // If there's a database error while saving, display a message
                        Console.WriteLine("An error occurred while saving changes.");
                        Console.WriteLine(dbEx.Message);

                        // If there's an inner exception, print it for more details
                        if (dbEx.InnerException != null)
                        {
                            Console.WriteLine("Inner Exception:");
                            Console.WriteLine(dbEx.InnerException.Message);
                        }
                        // Stop the method if there's an error
                        return;
                    }
                }
                else
                {
                    // If they're not new, ask for their Student ID
                    Console.Write("Enter your Student ID: ");
                    var studentIdStr = Console.ReadLine();

                    // Convert the entered ID to an integer
                    if (!int.TryParse(studentIdStr, out int studentId))
                    {
                        // If parsing fails, show an error and return
                        Console.WriteLine("Invalid Student ID!");
                        return;
                    }

                    // Attempt to find the student record in the database by ID
                    student = context.Students.Find(studentId);
                    if (student == null)
                    {
                        // If no record is found, inform the user and return
                        Console.WriteLine("No student found with that ID.");
                        return;
                    }
                }

                // Clear the console to have a fresh screen
                Console.Clear();
                // Greet the user by their first and last name
                Console.WriteLine($"Welcome back, {student.FirstName} {student.LastName}!");
                Console.WriteLine("***********************************");


                // Show a list of all dance styles by calling PrintDancestyle method
                PrintDancestyle();

                // Ask which dance style the user wants to book (by ID)
                Console.Write("\nType the Dancestyle ID for the class you want to book: ");
                var dancestyleChoice = Console.ReadLine();

                // Attempt to convert the user's input into an integer
                if (int.TryParse(dancestyleChoice, out int dancestyleId))
                {
                    // Find the chosen dance style in the database
                    var selectedDancestyle = context.DanceStyles.FirstOrDefault(ds => ds.StyleId == dancestyleId);

                    // If the dance style exists, confirm the booking
                    if (selectedDancestyle != null)
                    {
                        Console.Clear();
                        Console.WriteLine($"\n{student.FirstName} {student.LastName} är bokad på {selectedDancestyle.StyleName}!");
                        Console.WriteLine("Press any key to move forward");
                        Console.ReadKey();
                    }
                    else
                    {
                        // If the user entered an ID that doesn't match a dance style, show an error
                        Console.WriteLine("Invalid Dance ID. Please try again.");
                    }
                }
                else
                {
                    // If the user didn't type a valid integer, show an error
                    Console.WriteLine("Invalid input. Please enter a numeric Dance ID.");
                }

                // Save any changes to the database context (if needed)
                context.SaveChanges();
            }
        }

        public void PrintDancestyle()
        {
            // Create a new context to query the dance styles
            using var context = new BookingSystemContext();
            // Retrieve all dance styles from the database
            var danceStyles = context.DanceStyles.ToList();

            // Print a header
            Console.WriteLine("\nAvailable Dance Styles:");
            Console.WriteLine("----------------------");

            // Loop through each dance style and print its ID and name
            foreach (var danceStyle in danceStyles)
            {
                Console.WriteLine($"ID: {danceStyle.StyleId} | Name: {danceStyle.StyleName}");
            }

            Console.WriteLine("----------------------");
        }
    }
}
