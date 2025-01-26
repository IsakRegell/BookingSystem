using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Models
{
    public static class InstructorManager
    {
        public static void AddInstructor()
        {
            using (var context = new BookingSystemContext())
            {
                Console.Write("Enter Instructor First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter Instructor Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Enter Dance Style: ");
                string style = Console.ReadLine();

                var newInstructor = new Instructor
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Style = style
                };

                context.Instructors.Add(newInstructor);
                context.SaveChanges();

                Console.WriteLine("Instructor added successfully!");
            }
        }

        public static void ViewAllInstructors()
        {
            using (var context = new BookingSystemContext())
            {
                var instructors = context.Instructors.ToList();

                Console.WriteLine("====== All Instructors ======");
                foreach (var instructor in instructors)
                {
                    Console.WriteLine(
                        $"ID: {instructor.InstructorId}, " +
                        $"Name: {instructor.FirstName} {instructor.LastName}, " +
                        $"Style: {instructor.Style}"
                    );
                }
            }
        }

        public static void DeleteInstructor()
        {
            using (var context = new BookingSystemContext())
            {
                Console.Write("Enter Instructor ID to delete: ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out int instructorId))
                {
                    Console.WriteLine("Invalid ID. Please enter a numeric value.");
                    return;
                }

                var instructor = context.Instructors.Find(instructorId);
                if (instructor == null)
                {
                    Console.WriteLine("Instructor not found.");
                    return;
                }

                context.Instructors.Remove(instructor);
                context.SaveChanges();
                Console.WriteLine("Instructor deleted successfully!");
            }
        }
    }
}
    

