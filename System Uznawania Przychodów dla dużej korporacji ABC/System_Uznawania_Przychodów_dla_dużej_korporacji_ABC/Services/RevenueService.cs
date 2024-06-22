using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Data;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

public interface IRevenueService
{
    Task<double> CalculateCurrentRevenueAsync(string currency = "PLN");
    Task<double> CalculateCurrentRevenueForProductAsync(int productId, string currency = "PLN");
    Task<double> CalculateExpectedRevenueAsync(string currency = "PLN");
    Task<double> CalculateExpectedRevenueForProductAsync(int productId, string currency = "PLN");
}

public class RevenueService : IRevenueService
{
    private readonly DatabaseContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public RevenueService(DatabaseContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<double> CalculateCurrentRevenueAsync(string currency = "PLN")
    {
        var totalPayments = await _context.Contracts
            .Where(p => p.IsPaid)
            .SumAsync(p => p.Price);
        
        return await ConvertCurrencyAsync(totalPayments, currency);
    }

    public async Task<double> CalculateCurrentRevenueForProductAsync(int productId, string currency = "PLN")
    {
        var totalPayments = await _context.Contracts
            .Where(p => p.IsPaid && p.ProductId == productId)
            .SumAsync(p => p.Price);
        
        return await ConvertCurrencyAsync(totalPayments, currency);
    }

    public async Task<double> CalculateExpectedRevenueAsync(string currency = "PLN")
    {
        var totalContracts = await _context.Contracts
            .SumAsync(c => c.Price);

        return await ConvertCurrencyAsync(totalContracts, currency);
    }

    public async Task<double> CalculateExpectedRevenueForProductAsync(int productId, string currency = "PLN")
    {
        var totalContracts = await _context.Contracts
            .Where(c => c.ProductId == productId)
            .SumAsync(c => c.Price);

        return await ConvertCurrencyAsync(totalContracts, currency);
    }

    private async Task<double> ConvertCurrencyAsync(double amount, string currency)
    {
        if (currency == "PLN")
            return amount;

        var exchangeRate = await GetExchangeRateAsync("PLN", currency);
        return amount * exchangeRate;
    }

    private async Task<double> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetStringAsync($"https://api.exchangerate-api.com/v4/latest/{fromCurrency}");
        var data = JObject.Parse(response);
        var rates = data["rates"];
        var rate = rates[toCurrency].Value<double>();
        return rate;
    }
}