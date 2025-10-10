namespace FrazerDealer.Contracts.Inventory;

public record VehicleSummary(Guid Id, string StockNumber, string Vin, string Year, string Make, string Model, bool IsSold);

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
    Guid? CurrentSaleId);

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
