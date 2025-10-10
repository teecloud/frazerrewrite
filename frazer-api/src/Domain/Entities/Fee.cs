namespace FrazerDealer.Domain.Entities;

public class Fee
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public Sale? Sale { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool IsRecurring { get; set; }
}
