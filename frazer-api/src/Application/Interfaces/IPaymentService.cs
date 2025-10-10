using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Payments;

namespace FrazerDealer.Application.Interfaces;

public interface IPaymentService
{
    Task<Result<IReadOnlyCollection<PaymentDashboardItem>>> GetDashboardAsync(CancellationToken cancellationToken);
    Task<Result> RetryPaymentAsync(Guid id, PaymentRetryRequest request, CancellationToken cancellationToken);
}
