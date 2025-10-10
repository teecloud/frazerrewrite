using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Inventory;

namespace FrazerDealer.Application.Interfaces;

public interface IVehicleService
{
    Task<Result<IReadOnlyCollection<VehicleSummary>>> GetInventoryAsync(CancellationToken cancellationToken);
    Task<Result<VehicleDetail>> GetVehicleAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<VehicleDetail>> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken);
    Task<Result> UpdateVehicleAsync(Guid id, UpdateVehicleRequest request, CancellationToken cancellationToken);
    Task<Result> MarkVehicleSoldAsync(Guid id, CancellationToken cancellationToken);
}
