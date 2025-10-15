namespace FrazerDealer.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public string Vin { get; set; } = string.Empty;
    public string StockNumber { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Trim { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public bool IsSold { get; set; }
    public DateTime? DateArrived { get; set; }
    public DateTime? DateSold { get; set; }
    public Guid? CurrentSaleId { get; set; }
    public Sale? CurrentSale { get; set; }
    public List<Sale> SalesHistory { get; set; } = new();
    public ICollection<Prospect> Prospects { get; set; } = new List<Prospect>();
    public ICollection<ProspectVehicle> ProspectVehicles { get; set; } = new List<ProspectVehicle>();
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
