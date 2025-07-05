using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskModel> Tasks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region TaskModel Configuration
            modelBuilder.Entity<TaskModel>()
                .ToTable("Tasks");

            modelBuilder.Entity<TaskModel>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<TaskModel>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<TaskModel>()
                .Property(t => t.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<TaskModel>()
                .Property(t => t.DueDate)
                .IsRequired();

            modelBuilder.Entity<TaskModel>()
                .Property(t => t.IsCompleted)
                .HasDefaultValue(false)
                .IsRequired();
            modelBuilder.Entity<TaskModel>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<TaskModel>()
                .Property(t => t.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<TaskModel>()
                .HasIndex(t => t.DueDate);
            #endregion
        }
    }
}
