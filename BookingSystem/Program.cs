using BookingSystem.Models;

namespace BookingSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var realDatabase = new BookingSystemContext())
            {
                // Ensure the database is created, but don't delete it
                realDatabase.Database.EnsureCreated();

                // Retrieve and display all students
                var students = realDatabase.Students.ToList();
                Console.WriteLine("=== All Students ===");
                foreach (var student in students)
                {
                    Console.WriteLine($"{student.StudentId}: {student.FirstName} {student.LastName}, Born {student.DateOfBirth:yyyy-MM-dd}");
                }

                var instructors = realDatabase.Instructors.ToList();
                Console.WriteLine("=== All Instructors ===");
                foreach (var instructor in instructors)
                {
                    Console.WriteLine($"{instructor.FirstName}:  {instructor.LastName}");
                }
            }
        }
    }
}
