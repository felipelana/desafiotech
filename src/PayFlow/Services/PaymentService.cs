using PayFlow.Models;

namespace PayFlow.Services;

public class PaymentService
{
    private readonly FastPayService _fast;
    private readonly SecurePayService _secure;

    public PaymentService(FastPayService fast, SecurePayService secure)
    {
        _fast = fast;
        _secure = secure;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
    {
        // seleciona o gateway de acordo com o valor
        IPaymentProvider preferred = request.Amount < 100m ? (IPaymentProvider)_fast : _secure;
        IPaymentProvider fallback = preferred == _fast ? _secure : _fast;

        Exception? lastError = null;

        foreach (var provider in new[] { preferred, fallback })
        {
            try
            {
                var res = await provider.ProcessPaymentAsync(request);
                return res;
            }
            catch (Exception ex)
            {
                lastError = ex;
            }
        }

        throw new InvalidOperationException("Erro nos gateways.", lastError);
    }
}
