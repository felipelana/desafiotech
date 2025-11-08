using PayFlow.Models;

namespace PayFlow.Services;

public class FastPayService : IPaymentProvider
{
    public string Name => "FastPay";

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
    {
        // simular tempo de resposta
        await Task.Delay(100);

        var fee = Math.Round(request.Amount * 0.0349m, 2);

        return new PaymentResponse
        {
            Id = 1,
            ExternalId = "FP-884512",
            Status = "approved",
            Provider = Name,
            GrossAmount = request.Amount,
            Fee = fee,
            NetAmount = Math.Round(request.Amount - fee, 2)
        };
    }
}
