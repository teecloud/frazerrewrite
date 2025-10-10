namespace FrazerDealer.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public Sale? Sale { get; set; }
    public decimal Amount { get; set; }
    public DateTime CollectedOn { get; set; }
    public string Method { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; }
    public string? ExternalReference { get; set; }
}

public enum PaymentStatus
{
    Pending = 0,
    Settled = 1,
    Failed = 2,
    Cancelled = 3
}
