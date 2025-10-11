namespace FrazerDealer.Domain.Entities;

public class Prospect
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public List<Vehicle> Vehicles { get; set; } = new();
}
