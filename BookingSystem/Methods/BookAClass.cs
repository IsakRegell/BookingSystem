using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BookingSystem.Models
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

                    student = new Student
                    {
                        FirstName = firstName,
                        LastName = lastName
                    };

                    context.Students.Add(student);
                    try
                    {
                        context.SaveChanges();
                        Console.WriteLine("New student created successfully!");
                    }
                    catch (DbUpdateException dbEx)
                    {
                        Console.WriteLine("An error occurred while saving changes.");
                        Console.WriteLine(dbEx.Message);
                        if (dbEx.InnerException != null)
                        {
                            Console.WriteLine("Inner Exception:");
                            Console.WriteLine(dbEx.InnerException.Message);
                        }
                        return;
                    }
                }
                else
                {
                    Console.Write("Enter your Student ID: ");
                    var studentIdStr = Console.ReadLine();

                    if (!int.TryParse(studentIdStr, out int studentId))
                    {
                        Console.WriteLine("Invalid Student ID!");
                        return;
                    }

                    student = context.Students.Find(studentId);
                    if (student == null)
                    {
                        Console.WriteLine("No student found with that ID.");
                        return;
                    }
                }

                Console.Clear();
                Console.WriteLine($"Welcome back, {student.FirstName} {student.LastName}!");
                Console.WriteLine("***********************************");

                
                PrintDancestyle();

                Console.Write("\nType the Dancestyle ID for the class you want to book: ");
                var dancestyleChoice = Console.ReadLine();

                if (int.TryParse(dancestyleChoice, out int dancestyleId))
                {
                    var selectedDancestyle = context.DanceStyles.FirstOrDefault(ds => ds.StyleId == dancestyleId);

                    if (selectedDancestyle != null)
                    {
                        Console.Clear();
                        Console.WriteLine($"\n{student.FirstName} {student.LastName} är bokad på {selectedDancestyle.StyleName}!");
                        Console.WriteLine("Press any key to move forward");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Dance ID. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numeric Dance ID.");
                }

                context.SaveChanges();
            }
        }

        public void PrintDancestyle()
        {
            using var context = new BookingSystemContext();
            var danceStyles = context.DanceStyles.ToList();

            Console.WriteLine("\nAvailable Dance Styles:");
            Console.WriteLine("----------------------");

            foreach (var danceStyle in danceStyles)
            {
                Console.WriteLine($"ID: {danceStyle.StyleId} | Name: {danceStyle.StyleName}");
            }

            Console.WriteLine("----------------------");
        }
    }
}
