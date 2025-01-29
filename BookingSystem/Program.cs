﻿using BookingSystem.Methods;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookingSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<BookingSystemContext>()
               .UseSqlServer("Server=DESKTOP-M80AGJE\\SQLEXPRESS;Database=BookingSytem;Trusted_Connection=True;TrustServerCertificate=true;")
               .Options;
            var ViewLessonsByTeacher = new ViewLessonsByTeacher();
            var dbContext = new BookingSystemContext(options);
            var bookAclass = new BookAClass();


            while (true)
            {
                Console.WriteLine("====== Booking System Menu ======");
                Console.WriteLine("1. Add a Student"); // Balen
                Console.WriteLine("2. Add a Teacher"); // Tomas
                Console.WriteLine("3. Add a Class"); // Mikael
                Console.WriteLine("4. View All Students"); // Balen
                Console.WriteLine("5. View All Teachers"); // Tomas
                Console.WriteLine("6. View Classes by Teacher"); // Isak
                Console.WriteLine("7. View All Classes"); // Mikael
                Console.WriteLine("8. Delete a Student"); // Balen
                Console.WriteLine("9. Delete a Teacher"); // Tomas
                Console.WriteLine("10. Filter Classes by Date"); // Mikael
                Console.WriteLine("11. Filter Students by Classes"); // Isak
                Console.WriteLine("12. Exit"); // Isak
                Console.WriteLine("=================================");
                Console.Write("Select an option: ");

                var BookingSystemMenuChoice = Console.ReadLine();
                switch (BookingSystemMenuChoice)
                {
                    case "1":
                        StudentManager.AddStudent();  
                        break;
                    case "2":
                        InstructorManager.AddInstructor();  
                        break;
                    case "3":
                        ClassesManager.AddClass();
                        //AddLesson();  Mikael
                        break;
                    case "4":
                        StudentManager.ViewAllStudents();  
                        break;
                    case "5":
                        InstructorManager.ViewAllInstructors(); 
                        break;
                    case "6":
                        ViewLessonsByTeacher.DisplayAllLessonsBasedOnInstructor();
                        break;
                    case "7":
<<<<<<< HEAD
                        ShowAllClasses.DisplayAllClasses();
                            //ViewAllLessons();  Mikael
=======
                        var showAllClasses = new ShowAllClasses(dbContext);
                        showAllClasses.DisplayAllClasses();
>>>>>>> main
                        break;
                    case "8":
                        StudentManager.DeleteStudent();  
                        break;
                    case "9":
                        InstructorManager.DeleteInstructor();  
                        break;
                    case "10":
                        FilterClassesByDate.FilterClasses();
                        //FilterLessonsByDate();  Mikael
                        break;
                    case "11":
                        bookAclass.BookClass();
                        break;
                    case "12":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }

            }
        }

    }
}


    

