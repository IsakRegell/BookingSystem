using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Models;

public partial class BookingSystemContext : DbContext
{
    public BookingSystemContext()
    {
    }

    public BookingSystemContext(DbContextOptions<BookingSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassLevel> ClassLevels { get; set; }

    public virtual DbSet<ClassSchedule> ClassSchedules { get; set; }

    public virtual DbSet<DanceStyle> DanceStyles { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-M80AGJE\\SQLEXPRESS;Database=BookingSytem;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__FDF479860A9E39EC");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("class_name");
            entity.Property(e => e.LevelId).HasColumnName("Level_id");

            entity.HasOne(d => d.Level).WithMany(p => p.Classes)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Classes__Level_i__5FB337D6");
        });

        modelBuilder.Entity<ClassLevel>(entity =>
        {
            entity.HasKey(e => e.LevelId).HasName("PK__ClassLev__C435321879BA2CAD");

            entity.HasIndex(e => e.LevelName, "UQ__ClassLev__9EF3BE7B272A7DD7").IsUnique();

            entity.Property(e => e.LevelId).HasColumnName("Level_id");
            entity.Property(e => e.LevelName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ClassSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Class_sc__C46A8A6F3E50B6AA");

            entity.ToTable("Class_schedule");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassSchedules)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Class_sch__class__68487DD7");

            entity.HasOne(d => d.Instructor).WithMany(p => p.ClassSchedules)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Class_sch__instr__693CA210");
        });

        modelBuilder.Entity<DanceStyle>(entity =>
        {
            entity.HasKey(e => e.StyleId).HasName("PK__DanceSty__83BA879CF7473BFB");

            entity.ToTable("DanceStyle");

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
            entity.ToTable("Student");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
