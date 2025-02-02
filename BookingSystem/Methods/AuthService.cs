using System.Text;                      // Provides classes to manipulate character strings (StringBuilder, etc.)
using BookingSystem.Models;            // Gives access to our database models (e.g., User, BookingSystemContext)

namespace BookingSystem.Methods
{
    public static class AuthService
    {
        /// <summary>
        /// Register a new user (plain-text passwords).
        /// </summary>
        public static void RegisterUser(BookingSystemContext context)
        {
            // Prompt the user to enter a desired username
            Console.Write("Choose a Username: ");
            string username = Console.ReadLine() ?? string.Empty; // Read user input; if null, use empty string

            // Check if the username is already taken in the database
            var existing = context.Users.FirstOrDefault(u => u.Username == username);
            if (existing != null)
            {
                // If it exists, tell the user and ask them to press ENTER to continue
                Console.WriteLine("Username already exists. Please try a different one.");
                Console.WriteLine("Press ENTER to try different username...");
                Console.ReadLine(); // Wait for user to acknowledge before returning
                return; // Stop execution here
            }

            // Ask the user to choose a password (will be read in hidden/asterisk form)
            Console.Write("Choose a Password (plain text): ");
            string password = ReadPassword(); // Calls our "hidden input" method

            // Ask the user to choose a role
            Console.Write("Choose Role (Admin / Staff / Customer): ");
            string role = Console.ReadLine() ?? "Customer"; // Default to "Customer" if null

            // Validate the role input against the allowed roles; if invalid, default to "Customer"
            if (!new[] { "Admin", "Staff", "Customer" }
                .Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Invalid role. Defaulting to 'Customer'.");
                role = "Customer";
                Console.ReadLine(); // Pause so the user can see the message
            }

            // Create a new User object based on the provided info
            var newUser = new User
            {
                Username = username,
                Password = password,
                Role = role
            };

            // Add this new user to the database context
            context.Users.Add(newUser);
            // Save changes to the database
            context.SaveChanges();
            // Inform the user that the account has been created
            Console.WriteLine($"User '{username}' created with role '{role}'.");
            // Pause so the user can read the confirmation
            Console.ReadLine();
        }

        /// <summary>
        /// Simple login: prompt user for username & password, compare plain text.
        /// Returns the matching user or null if invalid.
        /// </summary>
        public static User? Login(BookingSystemContext context)
        {
            // Ask user to enter their username
            Console.Write("Enter Username: ");
            string username = Console.ReadLine() ?? string.Empty; // Default to empty string if null

            // Ask user to enter their password (hidden/asterisk form)
            Console.Write("Enter Password: ");
            string password = ReadPassword();

            // Search the database for a user with the given username and password
            var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                // If not found, tell the user and pause so they can see the error
                Console.WriteLine("Invalid username or password.");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
                return null; // Return null to indicate login failure
            }

            // If found, welcome the user and show their role
            Console.WriteLine($"\nWelcome, {user.Username}! You are logged in as [{user.Role}].");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            return user; // Return the found user object
        }

        // This private method reads keystrokes from the console without displaying them (shows '*' instead)
        private static string ReadPassword()
        {
            StringBuilder input = new StringBuilder(); // Holds the user's typed characters

            while (true)
            {
                // Read a single key press from the console (true = do not echo the character)
                ConsoleKeyInfo key = Console.ReadKey(true);

                // If the user presses ENTER, break the loop and finish reading
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine(); // Move to the next line so it looks tidy
                    break;
                }
                // If the user presses BACKSPACE
                else if (key.Key == ConsoleKey.Backspace)
                {
                    // Remove the last character from 'input' if there is one
                    if (input.Length > 0)
                    {
                        input.Remove(input.Length - 1, 1);
                        // Move the cursor back by one, print a blank space, and move back again
                        // This visually erases the last '*' character on the console
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    // For any other key, add its character to our input
                    input.Append(key.KeyChar);
                    // Print an asterisk to mask the character
                    Console.Write("*");
                }
            }

            // Return the final typed password as a string
            return input.ToString();
        }
    }
}
