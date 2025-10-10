namespace FrazerDealer.Domain.Entities;

public class Sale
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public SaleStatus Status { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedOn { get; set; }
    public decimal Subtotal { get; set; }
    public decimal FeesTotal { get; set; }
    public decimal PaymentsTotal { get; set; }
    public decimal BalanceDue => Subtotal + FeesTotal - PaymentsTotal;
    public List<Fee> Fees { get; set; } = new();
    public List<Payment> Payments { get; set; } = new();
}

public enum SaleStatus
{
    Draft = 0,
    PendingPayment = 1,
    Completed = 2,
    Cancelled = 3
}
