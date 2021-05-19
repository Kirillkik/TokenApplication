using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.DatabaseContext
{
    public class DBContext : DbContext
    {
        
        public DBContext () : base()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasMany(x => x.Employees)
                .WithOne(x => x.Department);

            modelBuilder.Entity<Token>()
                .HasMany(x => x.Employees)
                .WithOne(x => x.Token);

            modelBuilder.Entity<Token>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Department>().Property(e => e.Id).ValueGeneratedNever();
        }
        virtual public DbSet<Employee> Employees { get; set; }
        virtual public DbSet<Department> Departments { get; set; }
        virtual public DbSet<Token> Tokens { get; set; }
    }
}
