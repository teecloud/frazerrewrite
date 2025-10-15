using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Photos;

namespace FrazerDealer.Application.Interfaces;

public interface IPhotoService
{
    Task<Result<IReadOnlyCollection<PhotoSummary>>> GetPhotosAsync(Guid? vehicleId, CancellationToken cancellationToken);
    Task<Result<PhotoDetail>> GetPhotoAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<PhotoDetail>> CreatePhotoAsync(CreatePhotoRequest request, CancellationToken cancellationToken);
    Task<Result> UpdatePhotoAsync(Guid id, UpdatePhotoRequest request, CancellationToken cancellationToken);
    Task<Result> DeletePhotoAsync(Guid id, CancellationToken cancellationToken);
}
