using PayFlow.Models;

namespace PayFlow.Services;

public interface IPaymentProvider
{
    Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
    string Name { get; }
}
