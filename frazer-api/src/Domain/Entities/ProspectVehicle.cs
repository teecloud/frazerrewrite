namespace FrazerDealer.Domain.Entities;

public class ProspectVehicle
{
    public Guid ProspectId { get; set; }
    public Prospect Prospect { get; set; } = null!;

    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
