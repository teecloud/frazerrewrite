using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Prospects;

namespace FrazerDealer.Application.Interfaces;

public interface IProspectService
{
    Task<Result<IReadOnlyCollection<ProspectSummary>>> GetProspectsAsync(CancellationToken cancellationToken);
    Task<Result<ProspectSummary>> GetProspectAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<ProspectSummary>> CreateProspectAsync(CreateProspectRequest request, CancellationToken cancellationToken);
}
