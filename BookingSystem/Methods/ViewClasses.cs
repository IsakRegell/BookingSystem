using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Methods
{
    public static class ViewClasses
    {
        public static void ViewClassesByDate()
        {
            using (var dbcontext = new BookingSystemContext())
            {
                var classes = dbcontext.Classes

                        .Select(c => new
                        {
                            c.ClassName,
                            Schedules = c.ClassSchedules.OrderBy(s => s.StartDate).Select(s => new
                            {
                                s.StartDate,
                                s.EndDate,
                                Instructor = new
                                {
                                    s.Instructor.FirstName,
                                    s.Instructor.LastName
                                }
                            })
                          .ToList(),// Gör det till en lista för att undvika problem med expression trees
                            LevelName = c.Level.LevelName
                        })
                     .AsEnumerable(); // Konvertera till IEnumerable för att använda LINQ i minnet

                   


                if (!classes.Any())
                {
                    Console.WriteLine("No classes available in the system..");
                    return;
                }

                Console.WriteLine("All Classes (Ordered by Start Date):");
                foreach (var classObj in classes)
                {
                    Console.WriteLine($"Class Name: {classObj.ClassName}");

                    foreach (var schedule in classObj.Schedules)
                    {
                        Console.WriteLine($"  - Schedule: {schedule.StartDate:yyyy-MM-dd} to {schedule.EndDate:yyyy-MM-dd}");
                        Console.WriteLine($"  - Instructor: {schedule.Instructor.FirstName} {schedule.Instructor.LastName}");
                    }

                    Console.WriteLine($"  - Level: {classObj.LevelName}");




                }
        }   }
    }
}

