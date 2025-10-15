namespace FrazerDealer.Contracts.Photos;

public record PhotoSummary(Guid Id, Guid VehicleId, string Url, string? Caption, bool IsPrimary);

public record PhotoDetail(Guid Id, Guid VehicleId, string Url, string? Caption, bool IsPrimary);

public record CreatePhotoRequest(Guid VehicleId, string Url, string? Caption, bool IsPrimary);

public record UpdatePhotoRequest(string Url, string? Caption, bool IsPrimary);
