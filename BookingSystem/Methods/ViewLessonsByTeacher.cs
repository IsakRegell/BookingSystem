using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace BookingSystem.Methods
{
    public class ViewLessonsByTeacher
    {
        public void DisplayAllInstructors()
        {
            using var context = new BookingSystemContext(); // Skapa en databas-kontakt

            // Hämta alla lärare från databasen
            var instructors = context.Instructors.ToList();

            // Iterera och visa varje lärare
            foreach (var instructor in instructors)
            {
                Console.WriteLine($"ID: {instructor.InstructorId}, Name: {instructor.FirstName} {instructor.LastName}, Style: {instructor.Style}");
            }
        }

        public void DisplayAllLessonsBasedOnInstructor()
        {
            Console.Clear();
            DisplayAllInstructors();

            Console.Write("\nAnge Instructorns ID för att se Schemat : ");
            var IDinputSchedule = Convert.ToInt32(Console.ReadLine());

            using var context = new BookingSystemContext();

            // Hämta scheman för den valda instruktören
            var schedules = context.ClassSchedules
                .Where(cs => cs.InstructorId == IDinputSchedule)
                .Include(cs => cs.Class) // Inkludera klassdetaljer
                .ToList();

            // Kontrollera om scheman hittades
            if (schedules.Any())
            {
                Console.WriteLine("\nScheduael for chosen instructor:");
                foreach (var schedule in schedules)
                {
                    Console.WriteLine($"Class: {schedule.Class?.ClassName}, Start: {schedule.StartDate}, End: {schedule.EndDate}");
                }
            }
            else
            {
                Console.WriteLine("\nNo class was found based on that teacher");
            }
            Console.WriteLine("\nPress any key to move forward");
            Console.ReadKey();
        }
    }
}
