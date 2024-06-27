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
        try
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
        } catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}