using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<TaskModel>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<TaskModel>()
                .Property(t => t.Description)
                .HasMaxLength(500);

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
        }
    }
}
