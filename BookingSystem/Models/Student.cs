using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models;

public partial class Student
{
    [Key]
    public int Student_Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }
}
