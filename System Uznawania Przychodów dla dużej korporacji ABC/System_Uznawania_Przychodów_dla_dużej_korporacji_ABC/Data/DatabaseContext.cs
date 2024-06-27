using Microsoft.EntityFrameworkCore;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<IndividualCustomer> IndividualCustomers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Payment> Payments { get; set; }


    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasDiscriminator<CustomerType>("CustomerType")
            .HasValue<IndividualCustomer>(CustomerType.INDIVIDUAL)
            .HasValue<Company>(CustomerType.COMPANY);

        modelBuilder.Entity<Product>()
            .HasDiscriminator<ProductType>("ProductType")
            .HasValue<Software>(ProductType.SOFTWARE);

        modelBuilder.Entity<IndividualCustomer>().HasData(
            new IndividualCustomer
            {
                Id = 1,
                FirstName = "First1",
                LastName = "Last1",
                Pesel = "12345678911",
                PhoneNumber = "1122334455",
                Address = "Address1",
                Email = "123@456.com",
                Discriminator = CustomerType.INDIVIDUAL,
                IsDeleted = false
            }
        );

        modelBuilder.Entity<Company>().HasData(
            new Company
            {
                Id = 2,
                Name = "Company1",
                Krs = "9876543210",
                PhoneNumber = "2233445566",
                Address = "CompanyAddress1",
                Email = "company@domain.com",
                Discriminator = CustomerType.COMPANY
            }
        );

        modelBuilder.Entity<Software>().HasData(
            new Software
            {
                Id = 1,
                Name = "Software1",
                Description = "Description of Software1",
                Price = 99.99,
                SoftwareVersion = "1.0",
                Category = "Category1",
                Discriminator = ProductType.SOFTWARE
            }
        );

        modelBuilder.Entity<Discount>().HasData(
            new Discount
            {
                Id = 1,
                Name = "Summer Sale",
                Percentage = 10,
                ProductId = 1,
                StartDate = new DateTime(2023, 6, 1),
                EndDate = new DateTime(2023, 6, 30)
            }
        );

        modelBuilder.Entity<Contract>().HasData(
            new Contract
            {
                Id = 1,
                CustomerId = 1,
                ProductId = 1,
                Price = 89.99,
                IsPaid = false,
                IsSigned = false,
                YearsOfUpdateSupport = 1,
                SoftwareVersion = "1.0",
                StartDate = new DateTime(2023, 6, 1),
                EndDate = new DateTime(2024, 6, 1)
            }
        );

        modelBuilder.Entity<Payment>().HasData(
            new Payment
            {
                Id = 1,
                CustomerId = 1,
                ContractId = 1,
                Amount = 89.99,
                PaymentDate = new DateTime(2023, 6, 4),
                IsRefunded = false
            }
        );
    }
}
