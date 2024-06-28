using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateContract(ContractDto contractDto, int additionalYears)
    {
        try
        {
            var contract = await _contractService.CreateContractAsync(contractDto, additionalYears);
            var responseContract = contract.CreateResponseContract();
            return CreatedAtAction(nameof(CreateContract), new { id = contract.Id }, responseContract);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{contractId}/pay")]
    public async Task<IActionResult> PayForContract(PaymentDTO paymentDto, int contractId)
    {
        try
        {
            var payment = await _contractService.PayForContractAsync(paymentDto, contractId);
            var responsePayment = payment.CreateResponsePayment();
            return CreatedAtAction(nameof(CreateContract), new { id = payment.Id }, responsePayment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}