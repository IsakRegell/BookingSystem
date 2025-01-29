using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Methods
{
    public class BookAClass
    {
        
        public void BookClass()
        {
            Console.WriteLine("Are you a new student? Y/N : ");
            var MemberchoiceYorN = Console.ReadLine();

            using (var context = new BookingSystemContext())
            {
                Student student = null;

                if (MemberchoiceYorN?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Console.Write("Enter First Name: ");
                    var firstName = Console.ReadLine();

                    Console.Write("Enter Last Name: ");
                    var lastName = Console.ReadLine();

                    // Possibly ask for DOB or other info...

                    student = new Student
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        //DateOfBirth = DateTime.Now // Placeholder
                    };

                    context.Students.Add(student);
                    try
                    {
                        context.SaveChanges();

                        Console.WriteLine(student);
                    }
                    catch (DbUpdateException dbEx)
                    {
                        Console.WriteLine("An error occurred while saving changes.");
                        Console.WriteLine(dbEx.Message);

                        // Look at the inner exception for detail
                        if (dbEx.InnerException != null)
                        {
                            Console.WriteLine("Inner Exception:");
                            Console.WriteLine(dbEx.InnerException.Message);
                        }
                    }

                    Console.WriteLine("New student created successfully!");
                }
                else
                {
                    // RETURNING STUDENT
                    Console.Write("Enter your Student ID: ");
                    var studentIdStr = Console.ReadLine();

                    if (!int.TryParse(studentIdStr, out int studentId))
                    {
                        Console.WriteLine("Invalid Student ID!");
                        return;
                    }

                    // Find existing student
                    student = context.Students.Find(studentId);
                    if (student == null)
                    {
                        Console.WriteLine("No student found with that ID.");
                        return;
                    }

                    Console.WriteLine($"Welcome back, {student.FirstName} {student.LastName}!");
                }
                // 2. Ask for the ClassSchedule ID
                Console.Clear();
                PrintDancestyles();
                Console.Write("\nType the Dancestyle ID to witch class you want to book : ");
                var Dancestylechoise = Console.ReadLine();

               //Gåvidare här!
                

                context.SaveChanges();

            }

            
        }
        public void PrintDancestyles()
        {
            using var context = new BookingSystemContext();

            // Hämta alla dansstilar från databasen
            var danceStyles = context.DanceStyles.ToList();

            // Iterera och skriv ut varje dansstil
            foreach (var danceStyle in danceStyles)
            {
                Console.WriteLine($"ID: {danceStyle.StyleId}, Name: {danceStyle.StyleName}");
            }
        }


    }
}

