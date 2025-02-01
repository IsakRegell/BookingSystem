using System.Text;
using BookingSystem.Models;

namespace BookingSystem.Methods
{
    public static class AuthService
    {
        /// <summary>
        /// Register a new user (plain-text passwords).
        /// </summary>
        public static void RegisterUser(BookingSystemContext context)
        {
            Console.Write("Choose a Username: ");
            string username = Console.ReadLine() ?? string.Empty;

            // Check if username is already taken
            var existing = context.Users.FirstOrDefault(u => u.Username == username);
            if (existing != null)
            {
                Console.WriteLine("Username already exists. Please try a different one.");

                // <-- PAUSE HERE
                Console.WriteLine("Press ENTER to try diffrent username...");
                Console.ReadLine(); // or Console.ReadKey()
                return;

            }

            Console.Write("Choose a Password (plain text): ");
            string password = ReadPassword(); // <--- HERE we call our hidden read

            Console.Write("Choose Role (Admin / Staff / Customer): ");
            string role = Console.ReadLine() ?? "Customer";

            // Basic validation for role
            if (!new[] { "Admin", "Staff", "Customer" }
                .Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Invalid role. Defaulting to 'Customer'.");
                role = "Customer";

                Console.ReadLine(); // or Console.ReadKey()
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                Role = role
            };

            context.Users.Add(newUser);
            context.SaveChanges();
            Console.WriteLine($"User '{username}' created with role '{role}'.");
            Console.ReadLine(); // or Console.ReadKey()

        }

        /// <summary>
        /// Simple login: prompt user for username & password, compare plain text.
        /// Returns the matching user or null if invalid.
        /// </summary>
        public static User? Login(BookingSystemContext context)
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Password: ");
            string password = ReadPassword(); // <--- HERE we call our hidden read

            // Check DB for matching user
            var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                Console.WriteLine("Invalid username or password.");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine(); // or Console.ReadKey()
                return null;
            }

            Console.WriteLine($"\nWelcome, {user.Username}! You are logged in as [{user.Role}].");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine(); // or Console.ReadKey()
            return user;
        }

        private static string ReadPassword()
        {
            StringBuilder input = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true); // true = do not show the key

                // If user presses Enter, we're done
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine(); // move to next line
                    break;
                }
                // Handle backspace
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        // remove one character from the stringbuilder
                        input.Remove(input.Length - 1, 1);

                        // move cursor back, print a blank, move cursor back again
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    // For any other character, append to our input and show '*'
                    input.Append(key.KeyChar);
                    Console.Write("*");
                }
            }

            return input.ToString();
        }

    }
}
