using BookingSystem.Methods;                   // Gives access to the methods for authentication, class/teacher management, etc.
using BookingSystem.Models;                    // Gives access to the models (User, BookingSystemContext, etc.)
using BookingSystem;                           // Possibly for CurrentSession or other project-level classes
using Microsoft.EntityFrameworkCore;           // Provides Entity Framework Core functionalities
using Spectre.Console;                         // Spectre.Console is a library for creating beautiful console applications
using System;                                  // Basic system functionalities
using System.Collections.Generic;             // Needed for collections like List<T>
using System.Linq;                             // For LINQ functionality
using System.Text;                             // For text manipulation (StringBuilder, etc.)
using System.Threading.Tasks;                  // For asynchronous programming (not directly used here)

namespace BookingSystem.Methods
{
    public class MenuManager
    {
        // Main entry method to run the menu loop
        public static void Run(BookingSystemContext dbContext)
        {
            while (true)
            {
                // Clear the console screen before showing the main login/register menu
                Console.Clear();
                // Print a title using Spectre.Console markup
                AnsiConsole.Markup("[bold yellow]=== Welcome to the Booking System ===[/]\n");

                // Use Spectre.Console's SelectionPrompt to give user a choice: "Login", "Register", "Exit"
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>().PageSize(15)
                        .Title("[bold]Select an option:[/]")
                        .AddChoices("Login", "Register", "Exit") // The available menu options
                );

                // Depending on user choice:
                if (choice == "Login")
                {
                    // Attempt to log in with AuthService
                    var user = AuthService.Login(dbContext);
                    if (user != null)
                    {
                        // If successful, store the user in the current session
                        CurrentSession.CurrentUser = user;
                        // Show the main menu after logging in
                        ShowMainMenu(dbContext);
                    }
                }
                else if (choice == "Register")
                {
                    // Call the registration method to create a new user
                    AuthService.RegisterUser(dbContext);
                }
                else if (choice == "Exit")
                {
                    // If user chooses Exit, break out of the loop (thus ending the program flow here)
                    return;
                }
            }
        }

        // This method displays the main menu once a user has logged in
        private static void ShowMainMenu(BookingSystemContext dbContext)
        {
            // Instantiate the classes we need to handle certain menu actions
            var bookAclass = new BookAClass();
            var viewLessonsByTeacher = new ViewLessonsByTeacher();

            while (true)
            {
                Console.Clear();
                // Show which user is logged in and their role (with some color formatting)
                AnsiConsole.Markup($"[bold green]Logged in as: {CurrentSession.CurrentUser?.Username}[/] [white]{CurrentSession.CurrentUser?.Role}[/]\n\n");

                // Create a list of default menu options
                var menuOptions = new List<string>
                {
                    "View All Classes",
                    "Book a class",
                    "View Classes by Teacher"
                };

                // Determine if the logged-in user is Staff or Admin
                bool isStaff = CurrentSession.CurrentUser?.Role?.Equals("Staff", StringComparison.OrdinalIgnoreCase) == true;
                bool isAdmin = CurrentSession.CurrentUser?.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;

                // If the user is Staff or Admin, add extra options
                if (isStaff || isAdmin)
                {
                    menuOptions.Add("Add a Teacher");
                    menuOptions.Add("Add a Class");
                }
                // If the user is Admin, add even more options
                if (isAdmin)
                {
                    menuOptions.Add("Add a Student");
                    menuOptions.Add("View All Students");
                    menuOptions.Add("Delete a Student");
                    menuOptions.Add("Delete a Teacher");
                }

                // Always include Logout and Exit as the last options
                menuOptions.Add("Logout");
                menuOptions.Add("Exit");

                // Prompt the user with the constructed list of menu options
                var selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>().PageSize(15)
                        .Title("[bold]Select an option:[/]")
                        .AddChoices(menuOptions)
                );

                // Use a switch statement to decide what to do based on the user's choice
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
                        ClassesManager.AddClass();
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
                        // Clear the current user session and return to the previous menu
                        CurrentSession.CurrentUser = null;
                        return;

                    case "Exit":
                        // End the application
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
