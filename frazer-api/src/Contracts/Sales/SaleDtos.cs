namespace FrazerDealer.Contracts.Sales;

public record SaleSummary(Guid Id, Guid VehicleId, Guid CustomerId, decimal Subtotal, decimal FeesTotal, decimal PaymentsTotal, string Status, DateTime CreatedOn);

public record SaleDetail(
    Guid Id,
    Guid VehicleId,
    Guid CustomerId,
    decimal Subtotal,
    decimal FeesTotal,
    decimal PaymentsTotal,
    decimal BalanceDue,
    string Status,
    DateTime CreatedOn,
    DateTime? CompletedOn,
    IEnumerable<SaleFeeDto> Fees,
    IEnumerable<SalePaymentDto> Payments);

public record SaleFeeDto(Guid Id, string Code, string Description, decimal Amount, bool IsRecurring);

public record SalePaymentDto(Guid Id, decimal Amount, DateTime CollectedOn, string Method, string Status, string? ExternalReference);

public record CreateSaleDraftRequest(Guid VehicleId, Guid CustomerId, decimal Subtotal);

public record AddSaleFeeRequest(string Code, string Description, decimal Amount, bool IsRecurring);

public record AddSalePaymentRequest(decimal Amount, string Method, DateTime? CollectedOn, string? ExternalReference);

public record SaleReceiptDto(SaleDetail Sale, CustomerInfo Customer, VehicleInfo Vehicle);

public record CustomerInfo(Guid Id, string Name, string Email, string Phone, string Address);

public record VehicleInfo(Guid Id, string StockNumber, string Vin, string Year, string Make, string Model, string Trim);

public record CompleteSaleRequest(DateTime? CompletedOn);
