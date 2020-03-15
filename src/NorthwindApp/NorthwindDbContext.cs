using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
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

        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[]
            {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                       && level == LogLevel.Information, true)
            });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=..\..\data\Northwind_small.sqlite;");

            optionsBuilder.UseLoggerFactory(MyLoggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

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
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<EmployeeTerritory>().ToTable("EmployeeTerritory");
            modelBuilder.Entity<Order>().ToTable("Order").HasOne(x => x.Shipper).WithMany(x => x.Orders).HasForeignKey(p => p.ShipVia);
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Region>().ToTable("Region");
            modelBuilder.Entity<Shipper>().ToTable("Shipper");
            modelBuilder.Entity<Supplier>().ToTable("Supplier");
            modelBuilder.Entity<Territory>().ToTable("Territory");


            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(NorthwindDbContext).Assembly);
        }
    }
}
