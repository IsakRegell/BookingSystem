using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem.Models;

namespace BookingSystem.Models
{
    public static class StudentManager
    {
        // Metod för att lägga till en student
        public static void AddStudent()
        {
            using (var context = new BookingSystemContext())
            {
                Console.Write("Enter Student First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter Student Last Name: ");
                string lastName = Console.ReadLine();

                var newStudent = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                };

                try
                {
                    context.Students.Add(newStudent);
                    context.SaveChanges();

                    Console.WriteLine("Student added successfully!");
                }
                catch (DbUpdateException ex)
                {
                    // Hantera databasfel
                    Console.WriteLine("An error occurred while saving the student to the database.");
                    Console.WriteLine($"Error details: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Hantera oväntade fel
                    Console.WriteLine("An unexpected error occurred.");
                    Console.WriteLine($"Error details: {ex.Message}");
                }
            }
        }

        // Metod för att visa alla studenter
        public static void ViewAllStudents()
        {
            using (var context = new BookingSystemContext())
            {
                var students = context.Students.ToList();

                Console.WriteLine("====== All Students ======");
                foreach (var student in students)
                {
                    Console.WriteLine(
                        $"ID: {student.Student_Id}, " +
                        $"Name: {student.FirstName} {student.LastName}"
                    );
                }
            }
        }

        // Metod för att ta bort en student
        public static void DeleteStudent()
        {
            using (var context = new BookingSystemContext())
            {
                Console.Write("Enter Student ID to delete: ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out int studentId))
                {
                    Console.WriteLine("Invalid ID. Please enter a numeric value.");
                    return;
                }

                var student = context.Students.Find(studentId);
                if (student == null)
                {
                    Console.WriteLine("Student not found.");
                    return;
                }

                context.Students.Remove(student);
                context.SaveChanges();
                Console.WriteLine("Student deleted successfully!");
            }
        }
    }
}

