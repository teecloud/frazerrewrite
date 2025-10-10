namespace FrazerDealer.Contracts.Payments;

public record PaymentDashboardItem(Guid Id, Guid SaleId, decimal Amount, string Method, string Status, DateTime CollectedOn, string? ExternalReference);

public record PaymentRetryRequest(string Reason);
