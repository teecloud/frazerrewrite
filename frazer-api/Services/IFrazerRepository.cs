using frazer_api.Models;

namespace frazer_api.Services;

public interface IFrazerRepository
{
    Task<IReadOnlyList<FrazerRecord>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<FrazerRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<FrazerRecord> CreateAsync(FrazerRecordCreateDto request, CancellationToken cancellationToken = default);

    Task<FrazerRecord?> UpdateAsync(int id, FrazerRecordUpdateDto request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
