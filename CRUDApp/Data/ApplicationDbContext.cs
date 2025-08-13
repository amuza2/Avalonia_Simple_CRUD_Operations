using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite(@"Data Source=CRUDApp.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Customer>().HasData(
            new Customer {Id = 1, FirstName = "Moh", LastName = "Chami", City = "Tipasa", Address = "Center"},
            new Customer {Id = 2, FirstName = "Amine", LastName = "Salah", City = "Bouismail", Address = "Addres2"},
            new Customer {Id = 3, FirstName = "Bilal", LastName = "Mou", City = "Alger", Address = "Addres3"}
            );
    }
}