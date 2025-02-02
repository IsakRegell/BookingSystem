using BookingSystem.Methods;
using BookingSystem.Models;
using BookingSystem;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Methods
{
    public class MenuManager
    {
        public static void Run(BookingSystemContext dbContext)
        {
            while (true)
            {
                Console.Clear();
                AnsiConsole.Markup("[bold yellow]=== Welcome to the Booking System ===[/]\n");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>().PageSize(15)
                        .Title("[bold]Select an option:[/]")
        .AddChoices("Login", "Register", "Exit")
                );

                if (choice == "Login")
                {
                    var user = AuthService.Login(dbContext);
                    if (user != null)
                    {
                        CurrentSession.CurrentUser = user;
                        ShowMainMenu(dbContext);
                    }
                }
                else if (choice == "Register")
                {
                    AuthService.RegisterUser(dbContext);
                }
                else if (choice == "Exit")
                {
                    return;
                }
            }
        }

        private static void ShowMainMenu(BookingSystemContext dbContext)
        {
            var bookAclass = new BookAClass();
            var viewLessonsByTeacher = new ViewLessonsByTeacher();

            while (true)
            {
                Console.Clear();
                AnsiConsole.Markup($"[bold green]Logged in as: {CurrentSession.CurrentUser?.Username}[/] [white]{CurrentSession.CurrentUser?.Role}[/]\n\n");

                var menuOptions = new List<string>
                {
                    "View All Classes",
                    "Book a class",
                    "View Classes by Teacher"
                };

                bool isStaff = CurrentSession.CurrentUser?.Role?.Equals("Staff", StringComparison.OrdinalIgnoreCase) == true;
                bool isAdmin = CurrentSession.CurrentUser?.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;

                if (isStaff || isAdmin)
                {
                    menuOptions.Add("Add a Teacher");
                    menuOptions.Add("Add a Class");
                }
                if (isAdmin)
                {
                    menuOptions.Add("Add a Student");
                    menuOptions.Add("View All Students");
                    menuOptions.Add("Delete a Student");
                    menuOptions.Add("Delete a Teacher");
                }
                menuOptions.Add("Logout");
                menuOptions.Add("Exit");

                var selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>().PageSize(15)
                        .Title("[bold]Select an option:[/]")
                        .AddChoices(menuOptions)
                );

                switch (selectedOption)
                {
                    case "View All Classes":
                        new ShowAllClasses(dbContext).DisplayAllClasses();
                        break;
                    case "Book a class":
                        bookAclass.BookClass();
                        break;
                    case "View Classes by Teacher":
                        viewLessonsByTeacher.DisplayAllLessonsBasedOnInstructor();
                        break;
                    case "Add a Teacher":
                        InstructorManager.AddInstructor();
                        break;
                    case "Add a Class":
                        new ClassesManager().AddClass(dbContext);
                        break;
                    case "Add a Student":
                        StudentManager.AddStudent();
                        break;
                    case "View All Students":
                        StudentManager.ViewAllStudents();
                        break;
                    case "Delete a Student":
                        StudentManager.DeleteStudent();
                        break;
                    case "Delete a Teacher":
                        InstructorManager.DeleteInstructor();
                        break;
                    case "Logout":
                        CurrentSession.CurrentUser = null;
                        return;
                    case "Exit":
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}

    
