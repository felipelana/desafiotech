using PayFlow.Models;

namespace PayFlow.Services;

public class SecurePayService : IPaymentProvider
{
    public string Name => "SecurePay";

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
    {
        // simular tempo de resposta
        await Task.Delay(100);

        var fee = Math.Round(request.Amount * 0.0299m + 0.40m, 2);

        return new PaymentResponse
        {
            Id = 2,
            ExternalId = "SP-19283",
            Status = "approved",
            Provider = Name,
            GrossAmount = request.Amount,
            Fee = fee,
            NetAmount = Math.Round(request.Amount - fee, 2)
        };
    }
}
