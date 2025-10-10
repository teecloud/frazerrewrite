using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Sales;

namespace FrazerDealer.Application.Interfaces;

public interface ISaleService
{
    Task<Result<IReadOnlyCollection<SaleSummary>>> GetSalesAsync(CancellationToken cancellationToken);
    Task<Result<SaleDetail>> GetSaleAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<SaleDetail>> CreateDraftAsync(CreateSaleDraftRequest request, CancellationToken cancellationToken);
    Task<Result<SaleDetail>> AddFeeAsync(Guid id, AddSaleFeeRequest request, CancellationToken cancellationToken);
    Task<Result<SaleDetail>> AddPaymentAsync(Guid id, AddSalePaymentRequest request, CancellationToken cancellationToken);
    Task<Result<SaleDetail>> CompleteSaleAsync(Guid id, CompleteSaleRequest request, CancellationToken cancellationToken);
    Task<Result<SaleReceiptDto>> GetReceiptAsync(Guid id, CancellationToken cancellationToken);
}
