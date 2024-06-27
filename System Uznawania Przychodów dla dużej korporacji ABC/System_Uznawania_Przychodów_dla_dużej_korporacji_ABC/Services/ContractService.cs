using Microsoft.EntityFrameworkCore;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Data;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

public interface IContractService
{
    Task<Contract> CreateContractAsync(ContractDto contractDto, int additionalYears);
    Task<Payment> PayForContractAsync(PaymentDTO paymentDto, int contractId);
}

public class ContractService : IContractService
{
    private readonly DatabaseContext _context;
    private const int FeeForAdditionalYears = 1000;
    private const double DiscountForReturningCustomers = 5;

    public ContractService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Contract> CreateContractAsync(ContractDto contractDto, int additionalYearsOfUpdateSupport)
    {
        if (AdditionalYearsOfUpdateSupportNotCorrect(additionalYearsOfUpdateSupport))
        {
            throw new InvalidOperationException("Maximum 3 years can be added for Update support");
        }
        var customer = await _context.Customers.FindAsync(contractDto.CustomerId);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");

        if (customer is IndividualCustomer individualCustomer && individualCustomer.IsDeleted)
        {
            throw new InvalidOperationException("Cannot create contract for a deleted individual customer.");
        }
        
        var product = await _context.Products.FindAsync(contractDto.ProductId);
        if (product == null)
            throw new KeyNotFoundException("Product not found");

        bool hasActiveSubscriptionOrContract = await CustomerHasActiveSubscriptionOrContract(contractDto.CustomerId, contractDto.ProductId);
        if (hasActiveSubscriptionOrContract)
        {
            throw new InvalidOperationException("Customer already has an active subscription or contract for this product");
        }
        
        // var biggestDiscount = await BiggestDiscountForProduct(product);
        var biggestDiscountForProductForSpecificDate =
            await BiggestDiscountForProductForSpecificDate(product, contractDto.StartDate);
        
        var contract = contractDto.CreateContract(additionalYearsOfUpdateSupport);
        contract.Price += FeeForAdditionalYears * additionalYearsOfUpdateSupport;

        contract.Price *= 1 - biggestDiscountForProductForSpecificDate;
        
        if (await IsReturningCustomer(customer))
        {
            contract.Price *= 1 - DiscountForReturningCustomers/100;
        }
        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();
        return contract;
    }
    
    public async Task<Payment> PayForContractAsync(PaymentDTO paymentDto, int contractId)
    {
        var contract = await _context.Contracts.FindAsync(contractId);
        if (contract == null)
            throw new KeyNotFoundException("Contract not found");

        if (paymentDto.PaymentDate > contract.EndDate)
        {
            await RefundPayments(contract.Id);

            await PrepareNewOfferForCustomer(contract,contract.YearsOfUpdateSupport - 1);
            
            throw new InvalidOperationException("Payment is past the due date. A new offer has been prepared for the customer.");
        }
        
        var payment = paymentDto.CreatePayment(contractId);

        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        var totalPayments = await CountTotalPayments(contractId);
        
        if (totalPayments >= contract.Price)
        {
            contract.IsPaid = true;
            contract.IsSigned = true;
            await _context.SaveChangesAsync();
        }
        
        return payment;
    }
    
    private async Task PrepareNewOfferForCustomer(Contract contract, int additionalYearsOfUpdateSupport)
    {
        var newContractDto = new ContractDto
        {
            CustomerId = contract.CustomerId,
            ProductId = contract.ProductId,
            Price = contract.Price,
            SoftwareVersion = contract.SoftwareVersion,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow + (contract.EndDate - contract.StartDate)
        };
        _context.Contracts.Remove(contract);
        await _context.SaveChangesAsync();
        
        await CreateContractAsync(newContractDto, additionalYearsOfUpdateSupport);
    }
    
    private async Task RefundPayments(int contractId)
    {
        var payments = await _context.Payments
            .Where(p => p.ContractId == contractId)
            .ToListAsync();

        foreach (var pay in payments)
        {
            pay.IsRefunded = true;
        }

        await _context.SaveChangesAsync();
    }
    
    private async Task<bool> CustomerHasActiveSubscriptionOrContract(int customerId, int productId)
    {
        return await _context.Contracts
            .AnyAsync(c => c.CustomerId == customerId && c.ProductId == productId && !c.IsSigned);
    }
    
    private async Task<double> CountTotalPayments(int contractId)
    {
        var totalPayments = await _context.Payments
            .Where(p => p.ContractId == contractId)
            .SumAsync(p => p.Amount);
        return totalPayments;
    }
    
    private bool AdditionalYearsOfUpdateSupportNotCorrect(int additionalYears)
    {
        return additionalYears > 3 || additionalYears < 0;
    }
    
    private async Task<bool> IsReturningCustomer(Customer customer)
    {
        var contract = await _context.Contracts
            .Where(c => c.IsSigned == true)
            .FirstOrDefaultAsync(c => c.CustomerId == customer.Id);
        
        return contract is not null;
    }
    
    private async Task<double> BiggestDiscountForProduct(Product product)
    {
        var hasDiscounts = await _context.Discounts.AnyAsync(d => d.ProductId == product.Id);

        if (hasDiscounts)
        {
            var discount = await _context.Discounts
                .Where(d => d.ProductId == product.Id)
                .MaxAsync(d => d.Percentage);

            return discount/100;
        }

        return 0;
    }
    
    private async Task<double> BiggestDiscountForProductForSpecificDate(Product product, DateTime dateTime)
    {
        var hasDiscounts = await _context.Discounts
            .AnyAsync(d => d.ProductId == product.Id && d.StartDate <= dateTime && d.EndDate >= dateTime);

        if (hasDiscounts)
        {
            var discount = await _context.Discounts
                .Where(d => d.ProductId == product.Id && d.StartDate <= dateTime && d.EndDate >= dateTime)
                .MaxAsync(d => d.Percentage);

            return discount / 100;
        }

        return 0;
    }
}