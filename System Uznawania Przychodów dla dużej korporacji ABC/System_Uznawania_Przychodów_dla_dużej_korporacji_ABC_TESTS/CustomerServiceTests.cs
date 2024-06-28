using Microsoft.EntityFrameworkCore;
using Moq;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Data;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

namespace CustomerServiceTests;

public class CustomerServiceTests
{
    private readonly CustomerService _customerService;
    private readonly DatabaseContext _context;

    public CustomerServiceTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new DatabaseContext(options);
        _customerService = new CustomerService(_context);
    }

    [Fact]
    public async Task AddCustomerAsync_ShouldAddIndividualCustomer()
    {
        // Arrange
        var customerDto = new CustomerDTO
        {
            // populate properties
            FirstName = "John",
            LastName = "Doe",
            Pesel = "12345678901",
            PhoneNumber = "123456789",
            Address = "123 Main St",
            Email = "john.doe@example.com"
        };
        var customerType = CustomerType.INDIVIDUAL;

        // Act
        var result = await _customerService.AddCustomerAsync(customerDto, customerType);

        // Assert
        var customer = await _context.Customers.FindAsync(result.Id);
        Assert.NotNull(customer);
        Assert.Equal(customerDto.FirstName, ((IndividualCustomer)customer).FirstName);
    }

    [Fact]
    public async Task AddCustomerAsync_ShouldAddCompanyCustomer()
    {
        //Arrange
        var customerDto = new CustomerDTO
        {
            // populate properties
            CompanyName = "ABC Corp",
            Krs = "9876543210",
            PhoneNumber = "987654321",
            Address = "456 Corporate Blvd",
            Email = "info@abccorp.com"
        };
        var customerType = CustomerType.COMPANY;
        
        // Act
        var result = await _customerService.AddCustomerAsync(customerDto, customerType);
        
        // Assert
        var customer = await _context.Customers.FindAsync(result.Id);
        Assert.NotNull(customer);
        Assert.Equal(customerDto.CompanyName, ((Company)customer).Name);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldUpdateCustomer()
    {
        // Arrange
        var customer = new IndividualCustomer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Pesel = "12345678901",
            PhoneNumber = "123456789",
            Address = "123 Main St",
            Email = "john.doe@example.com"
        };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var updateClientDto = new UpdateClientDTO
        {
            PhoneNumber = "987654321",
            Address = "456 Elm St",
            Email = "jane.smith@example.com"
        };

        // Act
        var result = await _customerService.UpdateCustomerAsync(customer.Id, updateClientDto);

        // Assert
        var updatedCustomer = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(updatedCustomer);
        Assert.Equal(updateClientDto.PhoneNumber, ((IndividualCustomer)updatedCustomer).PhoneNumber);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldThrowExceptionIfCustomerNotFound()
    {
        // Arrange
        var updateClientDto = new UpdateClientDTO
        {
            // populate properties
            PhoneNumber = "987654321",
            Address = "456 Elm St",
            Email = "jane.smith@example.com"
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.UpdateCustomerAsync(-1, updateClientDto));
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldSoftDeleteIndividualCustomer()
    {
        // Arrange
        var customer = new IndividualCustomer
        {
            Id = 3,
            FirstName = "John",
            LastName = "Doe",
            Pesel = "12345678901",
            PhoneNumber = "123456789",
            Address = "123 Main St",
            Email = "john.doe@example.com",
            IsDeleted = false
        };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        await _customerService.DeleteCustomerAsync(customer.Id);

        // Assert
        var deletedCustomer = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(deletedCustomer);
        Assert.True(((IndividualCustomer)deletedCustomer).IsDeleted);
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldThrowExceptionIfCompanyCustomer()
    {
        // Arrange
        var customer = new Company
        {
            Id = 14,
            Name = "ABC Corp",
            Krs = "9876543210",
            PhoneNumber = "987654321",
            Address = "456 Corporate Blvd",
            Email = "info@abccorp.com"
        };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerService.DeleteCustomerAsync(customer.Id));
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldThrowExceptionIfCustomerNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.DeleteCustomerAsync(-1));
    }
}