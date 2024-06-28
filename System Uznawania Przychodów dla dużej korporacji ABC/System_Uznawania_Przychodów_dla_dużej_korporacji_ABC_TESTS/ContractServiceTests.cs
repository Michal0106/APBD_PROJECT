using Microsoft.EntityFrameworkCore;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Data;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

namespace ContractServiceTests;

public class ContractServiceTests
{
    private readonly ContractService _contractService;
    private readonly DatabaseContext _context;

    public ContractServiceTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new DatabaseContext(options);
        _contractService = new ContractService(_context);
    }

    [Fact]
    public async Task CreateContractAsync_ShouldCreateContract()
    {
        // Arrange
        var customer = new IndividualCustomer
        {
            Id = 20,
            FirstName = "John",
            LastName = "Doe",
            Pesel = "12345678901",
            PhoneNumber = "123456789",
            Address = "123 Main St",
            Email = "john.doe@example.com",
            IsDeleted = false
        };
        var product = new Software
        {
            Id = 20,
            Name = "Software1",
            Description = "Description of Software1",
            Price = 99.99,
            SoftwareVersion = "1.0",
            Category = "Category1",
            Discriminator = ProductType.SOFTWARE
        };
        await _context.Customers.AddAsync(customer);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var contractDto = new ContractDto
        {
            CustomerId = customer.Id,
            ProductId = product.Id,
            Price = 1000,
            SoftwareVersion = "1.0",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(20)
        };

        // Act
        var contract = await _contractService.CreateContractAsync(contractDto, 2);

        // Assert
        var savedContract = await _context.Contracts.FindAsync(contract.Id);
        Assert.NotNull(savedContract);
        Assert.Equal(contractDto.CustomerId, savedContract.CustomerId);
        Assert.Equal(contractDto.ProductId, savedContract.ProductId);
    }

    [Fact]
    public async Task CreateContractAsync_ShouldThrowExceptionForInvalidAdditionalYears()
    {
        // Arrange
        var contractDto = new ContractDto
        {
            CustomerId = 1,
            ProductId = 1,
            Price = 1000,
            SoftwareVersion = "1.0",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(20)
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _contractService.CreateContractAsync(contractDto, 4));
    }

    [Fact]
    public async Task PayForContractAsync_ShouldPayForContract()
    {
        // Arrange
        var customer = new IndividualCustomer
        {
            Id = 2,
            FirstName = "John",
            LastName = "Doe",
            Pesel = "12345678901",
            PhoneNumber = "123456789",
            Address = "123 Main St",
            Email = "john.doe@example.com",
            IsDeleted = false
        };
        var product = new Software
        {
            Id = 2,
            Name = "Software1",
            Description = "Description of Software1",
            Price = 99.99,
            SoftwareVersion = "1.0",
            Category = "Category1",
            Discriminator = ProductType.SOFTWARE
        };
        await _context.Customers.AddAsync(customer);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var contractDto = new ContractDto
        {
            CustomerId = customer.Id,
            ProductId = product.Id,
            Price = 500,
            SoftwareVersion = "1.0",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(20)
        };
        var contract = await _contractService.CreateContractAsync(contractDto, 2);

        var paymentDto = new PaymentDTO
        {
            CustomerId = customer.Id,
            Amount = 500,
            PaymentDate = DateTime.UtcNow.AddDays(3)
        };
        
        // Act
        var result = await _contractService.PayForContractAsync(paymentDto, contract.Id);

        // Assert
        var payments = await _context.Payments.Where(p => p.ContractId == contract.Id).ToListAsync();
        Assert.NotNull(result);
        Assert.Single(payments);
        Assert.Equal(paymentDto.Amount, payments.First().Amount);
    }

    [Fact]
    public async Task PayForContractAsync_ShouldThrowExceptionIfContractNotFound()
    {
        // Arrange
        var paymentDto = new PaymentDTO
        {
            CustomerId = 1,
            Amount = 500,
            PaymentDate = DateTime.UtcNow
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _contractService.PayForContractAsync(paymentDto, 999));
    }

    [Fact]
    public async Task PayForContractAsync_ShouldRefundPaymentsAndRemoveContractIfPastDueDate()
    {
        // Arrange
        var customer = new IndividualCustomer
        {
            Id = 23,
            FirstName = "John",
            LastName = "Doe",
            Pesel = "12345678901",
            PhoneNumber = "123456789",
            Address = "123 Main St",
            Email = "john.doe@example.com",
            IsDeleted = false
        };
        var product = new Software
        {
            Id = 23,
            Name = "Software1",
            Description = "Description of Software1",
            Price = 99.99,
            SoftwareVersion = "1.0",
            Category = "Category1",
            Discriminator = ProductType.SOFTWARE
        };
        await _context.Customers.AddAsync(customer);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        
        var contractDto = new ContractDto
        {
            CustomerId = customer.Id,
            ProductId = product.Id,
            Price = 500,
            SoftwareVersion = "1.0",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(20)
        };
        var contract = await _contractService.CreateContractAsync(contractDto, 2);
        var contractId = contract.Id;
        var paymentDto1 = new PaymentDTO
        {
            CustomerId = customer.Id,
            Amount = 250,
            PaymentDate = DateTime.UtcNow.AddDays(15)
        };
        var paymentDto2 = new PaymentDTO
        {
            CustomerId = customer.Id,
            Amount = 250,
            PaymentDate = DateTime.UtcNow.AddYears(1)
        };

        // Act & Assert
        await _contractService.PayForContractAsync(paymentDto1, contract.Id);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _contractService.PayForContractAsync(paymentDto2, contract.Id));
        
        var savedContract = await _context.Contracts.FindAsync(contractId);
        Assert.Null(savedContract);
    }
}