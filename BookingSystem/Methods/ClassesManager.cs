using BookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Methods
{
    public static class ClassesManager
    {


        public static void AddClass()
        {
            using (var context = new BookingSystemContext())
            {
                string className = GetValidInput("Enter Class Name:", "Class name cannot be empty.");
                DateOnly startDate = GetValidDate("Enter Start Date (yyyy-MM-dd):");
                DateOnly endDate = GetValidDate("Enter End Date (yyyy-MM-dd):", startDate);

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

                // Hämta nivån från databasen
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

                context.Classes.Add(newClass);
                context.SaveChanges();

                Console.WriteLine($"Class '{className}' added successfully from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd} at {level.LevelName} level with teacher {selectedInstructor.FirstName} {selectedInstructor.LastName}.");
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

        // Metod för att hantera validerad inmatning av datum
        private static DateOnly GetValidDate(string prompt, DateOnly? minDate = null)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();

                if (DateOnly.TryParse(input, out DateOnly date))
                {
                    if (minDate == null || date >= minDate)
                        return date;

                    Console.WriteLine("End date cannot be earlier than start date. Please try again.");
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
                }
            }
        }

        // Metod för att välja en nivå (Beginner, Intermediate, Advanced)
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

        // Metod för att välja en instruktör
        // Metod för att välja en instruktör
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



        //    public static void AddClass()
        //    {
        //        using (var context = new BookingSystemContext())
        //        { 

        //            Console.WriteLine("Enter Class Name:");
        //            string className = Console.ReadLine();
        //             if (string.IsNullOrEmpty(className))
        //            {
        //                   Console.WriteLine("Class name cannot be empty. Please try again.");
        //                    return;
        //            }
        //            else
        //            {
        //                Console.WriteLine($"{className} is added");
        //            }

        //            Console.WriteLine("Enter Start Date (yyyy-MM-dd):");
        //            string startDateInput = Console.ReadLine();
        //            if (!DateOnly.TryParse(startDateInput, out DateOnly startDate))

        //            {
        //             Console.WriteLine("Invalid date format. Please try again.");
        //                    return;
        //            }

        //            Console.WriteLine("Enter End Date (yyyy-MM-dd):");
        //            string endDateInput = Console.ReadLine();
        //            if (!DateOnly.TryParse(endDateInput, out DateOnly endDate))

        //            {
        //                Console.WriteLine("Invalid date format. Please try again.");
        //                return;
        //            }

        //                if (endDate < startDate)
        //                {
        //                    Console.WriteLine("End date cannot be earlier than start date. Please try again.");
        //                    return;
        //                 }

        //                Console.WriteLine("Available Teachers:");
        //                var instructors = context.Instructors.ToList();
        //                foreach (var instructor in instructors)
        //                {
        //                    Console.WriteLine($"{instructor.InstructorId}: {instructor.FirstName} {instructor.LastName}");
        //                 }

        //            string instructorIdInput = Console.ReadLine();
        //            if (!int.TryParse(instructorIdInput, out int instructorId))
        //                {
        //                Console.WriteLine("Invalid Teacher ID. Please try again.");
        //                return;
        //                }

        //                var selectedInstructor = instructors.First(i => i.InstructorId == instructorId);

        //                Console.WriteLine($"{selectedInstructor.InstructorId}: {selectedInstructor.FirstName} {selectedInstructor.LastName} will teach the class");


        //                Console.WriteLine("Enter Level ID:");
        //                string levelIdInput = Console.ReadLine();
        //                 if (!int.TryParse(levelIdInput, out int levelId))
        //                    {
        //                    Console.WriteLine("Invalid Level ID. Please try again.");
        //                    return;
        //                    }


        //                Console.WriteLine("Enter Level Name:");
        //                string levelName = Console.ReadLine();
        //                if (string.IsNullOrEmpty(levelName))
        //                  {
        //                    Console.WriteLine("Level name cannot be empty. Please try again.");
        //                    return;
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine($"{levelName} is added");
        //                     }

        //        var level = new ClassLevel
        //        {
        //            LevelId = levelId,
        //            LevelName = levelName
        //        };

        //        ClassSchedule schedule = new ClassSchedule
        //        {
        //            StartDate = startDate,
        //            EndDate = endDate,
        //            InstructorId = selectedInstructor.InstructorId
        //        };

        //        Class newClass = new Class
        //        {
        //            ClassName = className,
        //            ClassSchedules = new List<ClassSchedule> { schedule },
        //            Level = level
        //        };

        //        context.Classes.Add(newClass);
        //        context.SaveChanges();

        //        Console.WriteLine($"Lesson '{className}' added successfully for {startDate:yyyy-MM-dd} with teacher {selectedInstructor.FirstName} {selectedInstructor.LastName}.");

        //    }
        //}

    
    }
} 


