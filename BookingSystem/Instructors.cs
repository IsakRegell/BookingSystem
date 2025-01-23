using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem
{
    public class Instructors
    {
        public int InstructorId { get; set; } // Primary Key
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Style { get; set; }
    }
}
}
