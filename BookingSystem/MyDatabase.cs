using Microsoft.EntityFrameworkCore;

namespace BookingSystem
{
    public class MyDatabase : DbContext
    {
        public DbSet<Student> Student { get; set; } // Table for students

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connection string for the database
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-SFTN8V0\SQLEXPRESS;Database=BookingSystem;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
