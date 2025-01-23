using System;

namespace BookingSystem
{
    public class Class_schedule
    {
        public int schedule_id { get; set; } // Primary Key
        public int class_id { get; set; }    
        public int instructor_id { get; set; } 
        public DateTime start_date { get; set; } 
        public DateTime end_date { get; set; }
    }
}