using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevenuesController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenuesController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }

    /*[HttpGet("current")]
    public async Task<IActionResult> GetCurrentRevenue([FromQuery] string currency = "PLN")
    {
        var revenue = await _revenueService.CalculateCurrentRevenueAsync(currency);
        return Ok(revenue);
    }

    [HttpGet("current/products/{productId}")]
    public async Task<IActionResult> GetCurrentRevenueForProduct(int productId, [FromQuery] string currency = "PLN")
    {
        var revenue = await _revenueService.CalculateCurrentRevenueForProductAsync(productId, currency);
        return Ok(revenue);
    }
    
    [HttpGet("expected")]
    public async Task<IActionResult> GetExpectedRevenue([FromQuery] string currency = "PLN")
    {
        var revenue = await _revenueService.CalculateExpectedRevenueAsync(currency);
        return Ok(revenue);
    }
    
    [HttpGet("expected/products/{productId}")]
    public async Task<IActionResult> GetExpectedRevenueForProduct(int productId, [FromQuery] string currency = "PLN")
    {
        var revenue = await _revenueService.CalculateExpectedRevenueForProductAsync(productId, currency);
        return Ok(revenue);
    }*/
    
    
    [HttpGet]
    public async Task<IActionResult> GetRevenue(bool countExpectedRevenue,[FromQuery] string currency = "PLN")
    {
        if (countExpectedRevenue)
        {
            var revenue = await _revenueService.CalculateExpectedRevenueAsync(currency);
            return Ok(revenue);
        }
        else
        {
            var revenue = await _revenueService.CalculateCurrentRevenueAsync(currency);
            return Ok(revenue);
        }
    }

    [HttpGet("products/{productId}")]
    public async Task<IActionResult> GetRevenueForProduct(int productId, bool countExpectedRevenue, [FromQuery] string currency = "PLN")
    {
        if (countExpectedRevenue)
        {
            var revenue = await _revenueService.CalculateExpectedRevenueForProductAsync(productId, currency);
            return Ok(revenue);
        }
        else
        {
            var revenue = await _revenueService.CalculateCurrentRevenueForProductAsync(productId, currency);
            return Ok(revenue);
        }
    }
}