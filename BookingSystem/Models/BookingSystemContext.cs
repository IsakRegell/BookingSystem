using System;                                    // Basic system functionality
using System.Collections.Generic;                // Provides generic collection types like List<T>
using Microsoft.EntityFrameworkCore;             // Entity Framework Core functionality (DbContext, ModelBuilder, DbSet, etc.)

namespace BookingSystem.Models
{
    // This class serves as the DbContext for Entity Framework Core, 
    // acting as the main pipeline for querying and saving data to the database.
    public partial class BookingSystemContext : DbContext
    {
        // Parameterless constructor needed by EF if we want to instantiate 
        // the context without passing in DbContextOptions.
        public BookingSystemContext()
        {
        }

        // Overloaded constructor that can accept DbContextOptions (e.g., for dependency injection)
        public BookingSystemContext(DbContextOptions<BookingSystemContext> options)
            : base(options)
        {
        }

        // A DbSet for each table in our database. These properties allow 
        // LINQ queries and CRUD operations on each table.
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClassLevel> ClassLevels { get; set; }
        public virtual DbSet<ClassSchedule> ClassSchedules { get; set; }
        public virtual DbSet<DanceStyle> DanceStyles { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<User> Users { get; set; } = null!;
        // "= null!" is sometimes used to suppress nullable warnings, 
        // as EF will handle initialization internally.

        // This method configures the connection string and other 
        // options if not already configured outside this class.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Server=PC\\SQLEXPRESS;Database=BookingSystem;Trusted_Connection=True;TrustServerCertificate=true;");

        // This method is called by EF to further configure the model (table/column mappings, keys, constraints).
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This tells EF that the 'Student_Id' property on the Student entity 
            // should have values generated on add (e.g., identity/autoincrement).
            modelBuilder.Entity<Student>().Property(x => x.Student_Id).ValueGeneratedOnAdd();

            // Below are additional configurations for the Class entity
            modelBuilder.Entity<Class>(entity =>
            {
                // Specify the primary key and name it
                entity.HasKey(e => e.ClassId).HasName("PK__Classes__FDF479860A9E39EC");

                // Map the ClassId property to the "class_id" column in the database
                entity.Property(e => e.ClassId).HasColumnName("class_id");

                // Set properties for ClassName
                entity.Property(e => e.ClassName)
                    .HasMaxLength(50)              // Limit to 50 characters
                    .IsUnicode(false)              // Non-Unicode column
                    .HasColumnName("class_name");  // Actual column name

                // LevelId is stored in the "Level_id" column
                entity.Property(e => e.LevelId).HasColumnName("Level_id");

                // Configure the relationship: Class has one ClassLevel (Level) 
                // with many Classes referencing this ClassLevel
                // Also set the foreign key constraint name
                entity.HasOne(d => d.Level).WithMany(p => p.Classes)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull) // Means cannot delete a level that is still referenced
                    .HasConstraintName("FK__Classes__Level_i__5FB337D6");
            });

            modelBuilder.Entity<ClassLevel>(entity =>
            {
                entity.HasKey(e => e.LevelId).HasName("PK__ClassLev__C435321879BA2CAD");

                // Create a unique index on LevelName
                entity.HasIndex(e => e.LevelName, "UQ__ClassLev__9EF3BE7B272A7DD7").IsUnique();

                entity.Property(e => e.LevelId).HasColumnName("Level_id");
                entity.Property(e => e.LevelName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClassSchedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleId).HasName("PK__Class_sc__C46A8A6F3E50B6AA");

                // This entity maps to "Class_schedule" table
                entity.ToTable("Class_schedule");

                entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
                entity.Property(e => e.ClassId).HasColumnName("class_id");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
                entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
                entity.Property(e => e.StartDate).HasColumnName("start_date");

                // Relationships:
                // A schedule belongs to one Class, a Class can have many schedules
                entity.HasOne(d => d.Class).WithMany(p => p.ClassSchedules)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class_sch__class__68487DD7");

                // A schedule has exactly one Instructor, an Instructor can have many schedules
                entity.HasOne(d => d.Instructor).WithMany(p => p.ClassSchedules)
                    .HasForeignKey(d => d.InstructorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class_sch__instr__693CA210");
            });

            modelBuilder.Entity<DanceStyle>(entity =>
            {
                entity.HasKey(e => e.StyleId).HasName("PK__DanceSty__83BA879CF7473BFB");

                entity.ToTable("DanceStyle");

                // Create a unique index on StyleName
                entity.HasIndex(e => e.StyleName, "UQ__DanceSty__23564EE6FBA71EEB").IsUnique();

                entity.Property(e => e.StyleId).HasColumnName("Style_id");
                entity.Property(e => e.StyleName)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasKey(e => e.InstructorId).HasName("PK__Instruct__A1EF56E8CE755581");

                entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first_name");
                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("last_name");
                entity.Property(e => e.Style)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("style");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                // Student entity maps to a table named "Student"
                entity.ToTable("Student");
            });

            // This partial method allows for additional model customization 
            // that might be defined in another partial class
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
