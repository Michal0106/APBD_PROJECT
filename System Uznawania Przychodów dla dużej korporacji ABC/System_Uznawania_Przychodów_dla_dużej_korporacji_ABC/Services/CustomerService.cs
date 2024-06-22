using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Data;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

public interface ICustomerService
{
    Task<Customer> AddCustomerAsync(CustomerDTO customerDto, CustomerType customerType);
    Task<Customer> UpdateCustomerAsync(int id, UpdateClientDTO updateClientDto);
    Task DeleteCustomerAsync(int id);
}

public class CustomerService : ICustomerService
{
    private readonly DatabaseContext _context;

    public CustomerService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Customer> AddCustomerAsync(CustomerDTO customerDTO, CustomerType customerType)
    {
        Customer customer;
        if (customerType is CustomerType.INDIVIDUAL)
        {
            customer = customerDTO.CreateIndividualCustomer();
        }
        else
        {
            customer = customerDTO.CreateCompany();
        }
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
    public async Task<Customer> UpdateCustomerAsync(int id, UpdateClientDTO updateClientDto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");
        
        updateClientDto.UpdateEntity(customer);
        
        await _context.SaveChangesAsync();
        return customer;
    }
    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");

        if (customer is IndividualCustomer individualCustomer)
        {
            individualCustomer.IsDeleted = true;
        }
        else if (customer is Company)
        {
            throw new InvalidOperationException("Company customers cannot be deleted");
        }
        await _context.SaveChangesAsync();
    }
}