using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Insurance;

namespace FrazerDealer.Application.Interfaces;

public interface IInsuranceService
{
    Task<Result<IReadOnlyCollection<InsuranceProviderDto>>> GetProvidersAsync(CancellationToken cancellationToken);
    Task<Result<InsuranceProviderDto>> UpsertProviderAsync(Guid? id, UpsertInsuranceProviderRequest request, CancellationToken cancellationToken);
}
