namespace FrazerDealer.Contracts.Reports;

public record InventoryReportRow(string StockNumber, string Vin, string Year, string Make, string Model, DateTime? DateArrived, bool IsSold, DateTime? DateSold);

public record TitlesPendingReportRow(string StockNumber, string Vin, string CustomerName, DateTime? SaleDate, string Status);

public record InsuranceReportRow(string Provider, int ActivePolicies, decimal PremiumsDue, DateTime? LastUpdated);
