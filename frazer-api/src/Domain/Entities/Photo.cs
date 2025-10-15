namespace FrazerDealer.Domain.Entities;

public class Photo
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public bool IsPrimary { get; set; }
    public Vehicle? Vehicle { get; set; }
}
