using BookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Methods
{
    public class ClassesManager
    {
        public static void AddClass()
        {
            using (var context = new BookingSystemContext())
            {
                string className = GetValidInput("Enter Class Name:", "Class name cannot be empty.");
                DateOnly startDate = GetDateWithMenu("Enter Start Date:");
                DateOnly endDate = GetDateWithMenu("Enter End Date:", startDate);

                var selectedInstructor = SelectInstructor(context);
                if (selectedInstructor == null)
                {
                    Console.WriteLine("No valid instructor selected. Operation aborted.");
                    return;
                }
                int levelId = SelectClassLevel();
                if (levelId == 0)
                {
                    Console.WriteLine("Invalid level selected. Operation aborted.");
                    return;
                }

                // Skapa objekten
                var level = context.ClassLevels.FirstOrDefault(l => l.LevelId == levelId);
                if (level == null)
                {
                    Console.WriteLine("Error: Selected level does not exist.");
                    return;
                }

                // Skapa och spara klass i databasen
                var schedule = new ClassSchedule { StartDate = startDate, EndDate = endDate, InstructorId = selectedInstructor.InstructorId };

                var newClass = new Class
                {
                    ClassName = className,
                    ClassSchedules = new List<ClassSchedule> { schedule },
                    LevelId = level.LevelId
                };

                // Spara i databasen
                context.Classes.Add(newClass);
                context.SaveChanges();

                Console.WriteLine(
                    $"Class '{className}' added successfully for {startDate:yyyy-MM-dd} with teacher {selectedInstructor.FirstName} {selectedInstructor.LastName}.");

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(); // Vänta på att användaren bekräftar innan rensning
                Console.Clear();
            }
        }

        // Metod för att hantera validerad inmatning av text
        private static string GetValidInput(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input)) return input;

                Console.WriteLine(errorMessage);
            }
        }

        private static DateOnly GetDateWithMenu(string prompt, DateOnly? minDate = null)
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;

            Console.WriteLine($"{prompt}");

            // Välj år
            Console.Write($"Enter Year: ");
            string yearInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(yearInput)) year = int.Parse(yearInput);

            // Välj månad
            month = GetMonthFromUser("Enter Month", month);


            // Välj dag
            day = GetDayFromUser("Enter Day", year, month, day);


            DateOnly selectedDate = new DateOnly(year, month, day);

            // Validera att slutdatum inte är före startdatum
            if (minDate != null && selectedDate < minDate.Value)
            {
                Console.WriteLine("End date cannot be earlier than start date. Please try again.");
                return GetDateWithMenu(prompt, minDate);
            }
            return selectedDate;

        }
        private static int GetMonthFromUser(string prompt, int defaultMonth)
            {
                string[] monthsShort = { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
                string[] monthsFull = { "january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december" };

                while (true)
                {
                    Console.Write($"{prompt} ({defaultMonth}): ");
                    string input = Console.ReadLine()?.Trim().ToLower();

                    // Om användaren trycker Enter, använd standardmånad
                    if (string.IsNullOrEmpty(input)) return defaultMonth;

                    // Försök tolka input som en siffra (månad 1-12)
                    if (int.TryParse(input, out int monthNumber) && monthNumber >= 1 && monthNumber <= 12)
                    {
                        return monthNumber;
                    }

                    // Kontrollera om användaren skrev en månadsförkortning
                    int shortIndex = Array.IndexOf(monthsShort, input);
                    if (shortIndex != -1) return shortIndex + 1; // Index börjar på 0, månader på 1

                    // Kontrollera om användaren skrev ett fullständigt månadsnamn
                    int fullIndex = Array.IndexOf(monthsFull, input);
                    if (fullIndex != -1) return fullIndex + 1;

                    Console.WriteLine("Invalid month. Please enter a number (1-12) or a valid month name.");
                }
            }

        private static int GetDayFromUser(string prompt, int year, int month, int defaultDay)
        {
            string[] daysShort = { "sun", "mon", "tue", "wed", "thu", "fri", "sat" };
            string[] daysFull = { "sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday" };

            while (true)
            {
                Console.Write($"{prompt} ({defaultDay}): ");
                string input = Console.ReadLine()?.Trim().ToLower();

                // Om användaren trycker Enter, använd standarddag
                if (string.IsNullOrEmpty(input)) return defaultDay;

                // Om det är en siffra (1-31), returnera den
                if (int.TryParse(input, out int dayNumber) && dayNumber >= 1 && dayNumber <= DateTime.DaysInMonth(year, month))
                {
                    return dayNumber;
                }

                // Kolla om användaren skrev en veckodag
                int shortIndex = Array.IndexOf(daysShort, input);
                int fullIndex = Array.IndexOf(daysFull, input);

                if (shortIndex != -1 || fullIndex != -1)
                {
                    int dayOfWeek = shortIndex != -1 ? shortIndex : fullIndex;
                    return FindClosestDay(year, month, dayOfWeek);
                }

                Console.WriteLine("Invalid day. Please enter a number (1-31) or a valid weekday name.");
            }
        }

        private static int FindClosestDay(int year, int month, int targetDayOfWeek)
        {
            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                DateTime date = new DateTime(year, month, day);
                if ((int)date.DayOfWeek == targetDayOfWeek)
                {
                    return day;
                }
            }
            return 1; // Om något går fel, välj den första dagen
        }


        // Metod för att hantera validerad inmatning av heltal
        private static int GetValidIntInput(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int result)) return result;

                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
        // Metod för att hantera validerad inmatning av datum
        private static int SelectClassLevel()
        {
            Console.WriteLine("Select Class Level:");
            Console.WriteLine("1 - Beginner");
            Console.WriteLine("2 - Intermediate");
            Console.WriteLine("3 - Advanced");

            while (true)
            {
                int levelId = GetValidIntInput("Enter Level ID:");
                if (levelId >= 1 && levelId <= 3)
                    return levelId;

                Console.WriteLine("Invalid Level ID. Please select 1, 2, or 3.");
            }
        }


        // Metod för att låta användaren välja en instruktör
        private static Instructor SelectInstructor(BookingSystemContext context)
        {
            var instructors = context.Instructors.ToList();

            if (!instructors.Any())
            {
                Console.WriteLine("No available instructors.");
                return null;
            }

            Console.WriteLine("Available Teachers:");
            foreach (var instructor in instructors)
            {
                Console.WriteLine($"{instructor.InstructorId}: {instructor.FirstName} {instructor.LastName}");
            }

            while (true)
            {
                int instructorId = GetValidIntInput("Select a Teacher by ID:");

                var selectedInstructor = instructors.FirstOrDefault(i => i.InstructorId == instructorId);
                if (selectedInstructor != null) return selectedInstructor;

                Console.WriteLine("Invalid Teacher ID. Please try again.");
            }

            return null;
        }
    }



}
