﻿using BookingSystem.Models;
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
            using (var dbcontext = new BookingSystemContext())
            {
                Console.WriteLine("Enter Class Name:");
                string inputClassName = Console.ReadLine();
                if (string.IsNullOrEmpty(inputClassName))
                {
                    Console.WriteLine("Class name cannot be empty. Please try again.");
                    return;
                }
                else
                {
                    Console.WriteLine($"{inputClassName} is added");
                }

                Console.WriteLine("Enter Start Date (yyyy-MM-dd):");
                string startDateInput = Console.ReadLine();
                if (!DateOnly.TryParse(startDateInput, out DateOnly startDate))
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                    return;
                }
                else
                {
                    Console.WriteLine($"{startDate}: is added");
                }

                Console.WriteLine("Enter End Date (yyyy-MM-dd):");
                string endDateInput = Console.ReadLine();
                if (!DateOnly.TryParse(endDateInput, out DateOnly endDate))
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                    return;
                }
                else if (endDate < startDate)
                {
                    Console.WriteLine("End date cannot be earlier than start date. Please try again.");
                    return;
                }
                else
                {
                    Console.WriteLine($"{endDate} is added.");
                }

                Console.WriteLine("Available Teachers:");
                var instructors = dbcontext.Instructors.ToList();

                if (!instructors.Any())
                {
                    Console.WriteLine("No teachers available. Please add teachers to the system first.");
                    return;
                }
                foreach (var instructor in instructors)
                {
                    Console.WriteLine($"{instructor.InstructorId}: {instructor.FirstName} {instructor.LastName}");
                }

                Console.WriteLine("Select a Teacher by ID:");
                string instructorIdInput = Console.ReadLine();
                if (!int.TryParse(instructorIdInput, out int instructorId) || !instructors.Any(i => i.InstructorId == instructorId))
                {
                    Console.WriteLine("Invalid Teacher ID. Please try again.");
                    return;
                }

                var selectedInstructor = instructors.First(i => i.InstructorId == instructorId);

                Console.WriteLine($"{selectedInstructor.InstructorId}: {selectedInstructor.FirstName} {selectedInstructor.LastName} will teach the class");


                Console.WriteLine("Enter Level ID:");
                string levelIdInput = Console.ReadLine();
                if (!int.TryParse(levelIdInput, out int levelId))
                {
                    Console.WriteLine("Invalid Level ID. Please try again.");
                    return;
                }


                Console.WriteLine("Enter Level Name:");
                string levelName = Console.ReadLine();
                if (string.IsNullOrEmpty(levelName))
                {
                    Console.WriteLine("Level name cannot be empty. Please try again.");
                    return;
                }
                else
                {
                    Console.WriteLine($"{levelName} is added");
                }
                

                var level = new ClassLevel
                {
                    LevelId = levelId,
                    LevelName = levelName
                };

                ClassSchedule schedule = new ClassSchedule
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    InstructorId = selectedInstructor.InstructorId
                };

                Class newClass = new Class
                {
                    ClassName = inputClassName,
                    ClassSchedules = new List<ClassSchedule> { schedule },
                    Level = level
                };

                dbcontext.Classes.Add(newClass);
                dbcontext.SaveChanges();

                Console.WriteLine($"Lesson '{inputClassName}' added successfully for {startDate:yyyy-MM-dd} with teacher {selectedInstructor.FirstName} {selectedInstructor.LastName}.");
            }
        }

    }
}
