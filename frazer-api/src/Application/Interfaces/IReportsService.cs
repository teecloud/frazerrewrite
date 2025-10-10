using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Reports;

namespace FrazerDealer.Application.Interfaces;

public interface IReportsService
{
    Task<Result<IReadOnlyCollection<InventoryReportRow>>> GetInventoryReportAsync(CancellationToken cancellationToken);
    Task<Result<IReadOnlyCollection<TitlesPendingReportRow>>> GetTitlesPendingReportAsync(string? filter, CancellationToken cancellationToken);
    Task<Result<IReadOnlyCollection<InsuranceReportRow>>> GetInsuranceReportAsync(CancellationToken cancellationToken);
}
