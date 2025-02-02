using BookingSystem.Models;              // Gives access to the BookingSystemContext, Instructor, ClassSchedule, etc.
using Microsoft.EntityFrameworkCore;     // Allows us to use Entity Framework Core functionalities (e.g., Include)

namespace BookingSystem.Methods
{
    public class ViewLessonsByTeacher
    {
        // Method to display all instructors in the system
        public void DisplayAllInstructors()
        {
            // Create a new context to interact with the database
            using var context = new BookingSystemContext();

            // Retrieve all instructors from the database
            var instructors = context.Instructors.ToList();

            // Loop through each instructor and display their information
            foreach (var instructor in instructors)
            {
                Console.WriteLine($"ID: {instructor.InstructorId}, Name: {instructor.FirstName} {instructor.LastName}, Style: {instructor.Style}");
            }

            // Prompt the user to press ENTER before continuing
            Console.WriteLine("\nPress ENTER to move forward");
            Console.ReadLine();
        }

        // Method to display all lessons (class schedules) based on a chosen instructor
        public void DisplayAllLessonsBasedOnInstructor()
        {
            // Clear the console screen for cleanliness
            Console.Clear();

            // First, show a list of all instructors
            DisplayAllInstructors();

            // Ask the user to input the instructor's ID for whom they want to see the schedule
            Console.Write("\nAnge Instruktörens ID för att se Schemat: ");
            var IDinputSchedule = Convert.ToInt32(Console.ReadLine()); // Convert user input to integer

            // Create a new database context
            using var context = new BookingSystemContext();

            // Retrieve all schedules where the InstructorId matches the user input
            // Include the Class object so we can display the class name
            var schedules = context.ClassSchedules
                .Where(cs => cs.InstructorId == IDinputSchedule)
                .Include(cs => cs.Class)
                .ToList();

            // Check if we got any schedules back
            if (schedules.Any())
            {
                Console.WriteLine("\nSchedule for chosen instructor:");
                // Display each schedule's class name, start date, and end date
                foreach (var schedule in schedules)
                {
                    Console.WriteLine($"Class: {schedule.Class?.ClassName}, Start: {schedule.StartDate}, End: {schedule.EndDate}");
                }
            }
            else
            {
                // If no schedules found, print a message
                Console.WriteLine("\nNo class was found based on that teacher");
            }

            // Pause so the user can read the output
            Console.WriteLine("\nPress any key to move forward");
            Console.ReadKey();
        }
    }
}
