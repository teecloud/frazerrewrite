using FrazerDealer.Contracts.Photos;

namespace FrazerDealer.Contracts.Inventory;

public record VehicleSummary(
    Guid Id,
    string StockNumber,
    string Vin,
    string Year,
    string Make,
    string Model,
    bool IsSold,
    string? PrimaryPhotoUrl = null);

public record VehicleDetail(
    Guid Id,
    string StockNumber,
    string Vin,
    string Year,
    string Make,
    string Model,
    string Trim,
    decimal Price,
    decimal Cost,
    bool IsSold,
    DateTime? DateArrived,
    DateTime? DateSold,
    Guid? CurrentSaleId,
    IReadOnlyCollection<PhotoSummary>? Photos = null);

public record CreateVehicleRequest(
    string StockNumber,
    string Vin,
    string Year,
    string Make,
    string Model,
    string Trim,
    decimal Price,
    decimal Cost,
    DateTime? DateArrived);

public record UpdateVehicleRequest(
    string StockNumber,
    string Year,
    string Make,
    string Model,
    string Trim,
    decimal Price,
    decimal Cost,
    DateTime? DateArrived);
