using Microsoft.EntityFrameworkCore;
using Northwind.Domain.Entities;

namespace NorthwindApp
{
    public class NorthwindDbContext : DbContext
    {

        public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options)
            : base(options)
        {
        }

        public NorthwindDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=D:\repos\predicate-builder\data\Northwind_small.sqlite;");
            base.OnConfiguring(optionsBuilder);
        }

        //public DbSet<Category> Categories { get; set; }

        //public DbSet<Customer> Customers { get; set; }

        //public DbSet<Employee> Employees { get; set; }

        //public DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }

        //public DbSet<OrderDetail> OrderDetails { get; set; }

        //public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        //public DbSet<Region> Region { get; set; }

        //public DbSet<Shipper> Shippers { get; set; }

        //public DbSet<Supplier> Suppliers { get; set; }

        //public DbSet<Territory> Territories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NorthwindDbContext).Assembly);
        }
    }
}
