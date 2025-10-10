using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Jobs;

namespace FrazerDealer.Application.Interfaces;

public interface IJobService
{
    Task<Result<JobStatusDto>> EnqueueJobAsync(JobEnqueueRequest request, CancellationToken cancellationToken);
    Task<Result<JobStatusDto>> GetStatusAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<IReadOnlyCollection<JobStatusDto>>> GetHistoryAsync(JobHistoryRequest request, CancellationToken cancellationToken);
}
