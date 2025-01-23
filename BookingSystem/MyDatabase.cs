using Microsoft.EntityFrameworkCore;

namespace BookingSystem
{
    public class MyDatabase : DbContext
    {
        public DbSet<Student> Student { get; set; } // Table for students
        public DbSet<Bookings> Bookings { get; set; } //Table for Bookings

        public DbSet<Classes> Classes { get; set; } //Table for Classes
        public DbSet<Instructors> Instructors { get; set; } //Table for Instructors

        public DbSet<Class_schedule> Class_schedule { get; set; } // Table for class_schedule

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connection string for the database
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-SFTN8V0\SQLEXPRESS;Database=BookingSystem;Trusted_Connection=True;TrustServerCertificate=True;");
        }


    }
}
