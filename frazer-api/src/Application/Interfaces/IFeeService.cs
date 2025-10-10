using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Fees;

namespace FrazerDealer.Application.Interfaces;

public interface IFeeService
{
    Task<Result<IReadOnlyCollection<FeeConfigurationDto>>> GetFeesAsync(CancellationToken cancellationToken);
    Task<Result> UpdateFeeAsync(Guid id, UpdateFeeConfigurationRequest request, CancellationToken cancellationToken);
}
