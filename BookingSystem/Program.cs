using BookingSystem.Models;

namespace BookingSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("====== Booking System Menu ======");
                Console.WriteLine("1. Add a Student"); // Balen
                Console.WriteLine("2. Add a Teacher"); // Tomas
                Console.WriteLine("3. Add a Lesson"); // Mikael
                Console.WriteLine("4. View All Students"); // Balen
                Console.WriteLine("5. View All Teachers"); // Tomas
                Console.WriteLine("6. View Lessons by Teacher"); // Isak
                Console.WriteLine("7. View All Lessons"); // Mikael
                Console.WriteLine("8. Delete a Student"); // Balen
                Console.WriteLine("9. Delete a Teacher"); // Tomas
                Console.WriteLine("10. Filter Lessons by Date"); // Mikael
                Console.WriteLine("11. Filter Students by Lessons"); // Isak
                Console.WriteLine("12. Exit"); // Isak
                Console.WriteLine("=================================");
                Console.Write("Select an option: ");

                var BookingSystemMenuChoice = Console.ReadLine();
                switch (BookingSystemMenuChoice)
                {
                    case "1":
                        //AddStudent();  Balen
                        break;
                    case "2":
                        //AddTeacher();  Tomas
                        break;
                    case "3":
                        //AddLesson();  Mikael
                        break;
                    case "4":
                        //ViewAllStudents();  Balen
                        break;
                    case "5":
                        //ViewAllTeachers();  Tomas
                        break;
                    case "6":
                        //ViewLessonsByTeacher();  Isak
                        break;
                    case "7":
                        //ViewAllLessons();  Mikael
                        break;
                    case "8":
                        //DeleteStudent();  Balen
                        break;
                    case "9":
                        //DeleteTeacher();  Tomas
                        break;
                    case "10":
                        //FilterLessonsByDate();  Mikael
                        break;
                    case "11":
                        //FilterStudentsByLessons();  Isak
                        break;
                    case "12":
                        // Console.WriteLine("Exiting..."); Isak
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }

            }
        }

    }
}


    

