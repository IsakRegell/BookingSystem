using BookingSystem.Methods;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
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
                var menuOptions = new Dictionary<string, Action>
            {
                { "Add a Student", () => StudentManager.AddStudent() },
                { "Add a Teacher", () => InstructorManager.AddInstructor() },
                { "Add a Class", () => new ClassesManager().AddClass(dbContext) },
                { "View All Students", () => StudentManager.ViewAllStudents() },
                { "View All Teachers", () => InstructorManager.ViewAllInstructors() },
                { "View Classes by Teacher", () => ViewLessonsByTeacher.DisplayAllLessonsBasedOnInstructor() },
                { "View All Classes", () => new ShowAllClasses(dbContext).DisplayAllClasses() },
                { "Delete a Student", () => StudentManager.DeleteStudent() },
                { "Delete a Teacher", () => InstructorManager.DeleteInstructor() },
                { "Book a class", () => bookAclass.BookClass() },
                { "Exit", () => { Console.WriteLine("Exiting..."); Environment.Exit(0); } }
            };

                // Skapa en interaktiv meny med Spectre.Console
                var selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]====== Booking System Menu ======[/]")
                        .PageSize(12)
                        .AddChoices(menuOptions.Keys)
                        .HighlightStyle("cyan"));

                // Kör den valda menyalternativets metod
                menuOptions[selectedOption].Invoke();
            }


        }
        }

    }



    

