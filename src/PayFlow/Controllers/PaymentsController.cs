using Microsoft.AspNetCore.Mvc;
using PayFlow.Models;
using PayFlow.Services;

namespace PayFlow.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "PayFlow API rodando",
            timestamp = DateTime.UtcNow
        });
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PaymentRequest request)
    {
        try
        {
            var response = await _paymentService.ProcessPaymentAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
