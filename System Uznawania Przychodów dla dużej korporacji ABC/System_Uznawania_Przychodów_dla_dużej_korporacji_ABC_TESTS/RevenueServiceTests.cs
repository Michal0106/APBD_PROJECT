using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Data;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

namespace RevenueServiceTests;

/*public class RevenueServiceTests
    {
        private readonly DatabaseContext _context;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly RevenueService _revenueService;

        public RevenueServiceTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DatabaseContext(options);
            _httpClientFactory = new Mock<IHttpClientFactory>();

            _revenueService = new RevenueService(_context, _httpClientFactory.Object);
        }

        private void SeedDatabase()
        {
            // Clear existing data
            _context.Contracts.RemoveRange(_context.Contracts);
            _context.SaveChanges();

            // Add test contracts
            _context.Contracts.AddRange(
                new Contract
                {
                    CustomerId = 1,
                    ProductId = 1,
                    Price = 300,
                    SoftwareVersion = "1.0",
                    YearsOfUpdateSupport = 3,
                    IsPaid = true,
                    IsSigned = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(10),
                },
                new Contract
                {
                    CustomerId = 2,
                    ProductId = 2,
                    Price = 200,
                    SoftwareVersion = "1.0",
                    YearsOfUpdateSupport = 3,
                    IsPaid = true,
                    IsSigned = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(10),
                },
                new Contract
                {
                    CustomerId = 3,
                    ProductId = 3,
                    Price = 150,
                    SoftwareVersion = "1.0",
                    YearsOfUpdateSupport = 3,
                    IsPaid = true,
                    IsSigned = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(10),
                }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task CalculateCurrentRevenueAsync_ShouldReturnCorrectRevenue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var revenue = await _revenueService.CalculateCurrentRevenueAsync("EUR");

            // Assert
            Assert.Equal((300 + 200 + 150) * await GetExchangeRateAsync("PLN", "EUR"), revenue); // Use actual exchange rate
        }

        [Fact]
        public async Task CalculateCurrentRevenueForProductAsync_ShouldReturnCorrectRevenue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var revenue = await _revenueService.CalculateCurrentRevenueForProductAsync(1, "USD");

            // Assert
            Assert.Equal(300 * await GetExchangeRateAsync("PLN", "USD"), revenue); // Use actual exchange rate
        }

        [Fact]
        public async Task CalculateExpectedRevenueAsync_ShouldReturnCorrectRevenue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var revenue = await _revenueService.CalculateExpectedRevenueAsync("USD");

            // Assert
            Assert.Equal((300 + 200 + 150) * await GetExchangeRateAsync("PLN", "USD"), revenue); // Use actual exchange rate
        }

        [Fact]
        public async Task CalculateExpectedRevenueForProductAsync_ShouldReturnCorrectRevenue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var revenue = await _revenueService.CalculateExpectedRevenueForProductAsync(1, "USD");

            // Assert
            Assert.Equal(300 * await GetExchangeRateAsync("PLN", "USD"), revenue); // Use actual exchange rate
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
    }*/