using BookingSystem.Models;
using System;
using System.Linq;

namespace BookingSystem.Methods 
{
    public class StudentService
    {
        private readonly BookingSystemContext _dbContext;

        public StudentService(BookingSystemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddStudent()
        {
            Console.Write("Enter First Name: ");
            var firstName = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            var lastName = Console.ReadLine();

            Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var dateOfBirth))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                return;
            }

            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth
            };

            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();
            Console.WriteLine("Student added successfully.");
        }

        public void ViewAllStudents()
        {
            var students = _dbContext.Students.ToList();

            if (!students.Any())
            {
                Console.WriteLine("No students found.");
                return;
            }

            Console.WriteLine("====== Students ======");
            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}, DOB: {student.DateOfBirth.ToShortDateString()}");
            }
        }

        public void DeleteStudent()
        {
            Console.Write("Enter Student ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var studentId))
            {
                Console.WriteLine("Invalid ID. Please try again.");
                return;
            }

            var student = _dbContext.Students.FirstOrDefault(s => s.StudentId == studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();
            Console.WriteLine("Student deleted successfully.");
        }
    }
}
