namespace FrazerDealer.Contracts.Prospects;

public record ProspectVehicleSummary(Guid Id, string StockNumber, string Year, string Make, string Model);

public record ProspectSummary(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    IReadOnlyCollection<ProspectVehicleSummary> Vehicles);

public record CreateProspectRequest(
    string Name,
    string Email,
    string Phone,
    IReadOnlyCollection<Guid> VehicleIds);
