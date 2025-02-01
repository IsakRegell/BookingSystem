using BookingSystem.Methods;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookingSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Configure EF Core options
            var options = new DbContextOptionsBuilder<BookingSystemContext>()
               .UseSqlServer("Server=PC\\SQLEXPRESS;Database=Bookingsytem;Trusted_Connection=True;TrustServerCertificate=true;")
               .Options;

            var dbContext = new BookingSystemContext(options);

            // Outer loop so we can return to login screen after logout
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Welcome to the Booking System ===");
                Console.WriteLine("1) Login");
                Console.WriteLine("2) Register");
                Console.WriteLine("3) Exit");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    // Attempt to log in
                    var user = AuthService.Login(dbContext);
                    if (user != null)
                    {
                        CurrentSession.CurrentUser = user;
                        ShowMainMenu(dbContext);
                        // After ShowMainMenu returns (on logout), we loop back, re-show login
                    }
                }
                else if (choice == "2")
                {
                    // Register a new user
                    AuthService.RegisterUser(dbContext);
                }
                else if (choice == "3")
                {
                    // Exit the entire application
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
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
                Console.WriteLine($"Logged in as: {CurrentSession.CurrentUser?.Username} [{CurrentSession.CurrentUser?.Role}]\n");

                // Base options for all users:
                var menuOptions = new List<string>
                {
                    "View All Classes",
                    "Book a class",
                    "View Classes by Teacher"
                };

                // Case-insensitive check for Staff or Admin
                bool isStaff = CurrentSession.CurrentUser?.Role?.Equals("Staff", StringComparison.OrdinalIgnoreCase) == true;
                bool isAdmin = CurrentSession.CurrentUser?.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;

                // Staff and Admin can add Teachers/Classes
                if (isStaff || isAdmin)
                {
                    menuOptions.Add("Add a Teacher");
                    menuOptions.Add("Add a Class");
                }

                // Admin can also add or remove Students/Teachers
                if (isAdmin)
                {
                    menuOptions.Add("Add a Student");
                    menuOptions.Add("View All Students");
                    menuOptions.Add("Delete a Student");
                    menuOptions.Add("Delete a Teacher");
                }

                menuOptions.Add("Logout");
                menuOptions.Add("Exit");

                // Print the menu
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {menuOptions[i]}");
                }

                Console.Write("\nSelect an option: ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out int choice) || choice < 1 || choice > menuOptions.Count)
                {
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                var selectedOption = menuOptions[choice - 1];

                // Handle the user's choice
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
                        // Clear the current user and return to Main()
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
