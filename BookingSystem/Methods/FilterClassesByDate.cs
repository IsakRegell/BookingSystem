using BookingSystem.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Methods
{
    public class FilterClassesByDate
    {
        private readonly BookingSystemContext dbContext;


        public FilterClassesByDate(BookingSystemContext context)
        {
            dbContext = context;
        }


        public void FilterClasses()
        {

            Console.WriteLine("Enter Start Date (yyyy-MM-dd):");
            string startDateInput = Console.ReadLine();
            if (!DateOnly.TryParse(startDateInput, out DateOnly startDate))
            {
                Console.WriteLine("Invalid start date format. Please try again.");
                return;
            }

            Console.WriteLine("Enter End Date (yyyy-MM-dd):");
            string endDateInput = Console.ReadLine();
            if (!DateOnly.TryParse(endDateInput, out DateOnly endDate))
            {
                Console.WriteLine("Invalid end date format. Please try again.");
                return;
            }

            if (endDate < startDate)
            {
                Console.WriteLine("End date cannot be earlier than start date. Please try again.");
                return;
            }

            var filteredClasses = dbContext.Classes
                .Where(c => c.ClassSchedules.Any(s => s.StartDate >= startDate && s.EndDate <= endDate))
                .Select(c => new
                {
                    c.ClassName,
                    Schedules = c.ClassSchedules.Where(s => s.StartDate >= startDate && s.EndDate <= endDate).Select(s => new
                    {
                        s.StartDate,
                        s.EndDate,
                        Instructor = new
                        {
                            s.Instructor.FirstName,
                            s.Instructor.LastName
                        }
                    }),
                    LevelName = c.Level.LevelName
                }).ToList();

            if (!filteredClasses.Any())
            {
                Console.WriteLine("No classes found in the specified date range.");
                return;
            }

            Console.WriteLine($"Classes between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}:");
            foreach (var classObj in filteredClasses)
            {
                Console.WriteLine($"Class Name: {classObj.ClassName}");

                foreach (var schedule in classObj.Schedules)
                {
                    Console.WriteLine($"  - Schedule: {schedule.StartDate:yyyy-MM-dd} to {schedule.EndDate:yyyy-MM-dd}");
                    Console.WriteLine($"  - Instructor: {schedule.Instructor.FirstName} {schedule.Instructor.LastName}");
                }

                Console.WriteLine($"  - Level: {classObj.LevelName}");
            }
        }
    }
}

