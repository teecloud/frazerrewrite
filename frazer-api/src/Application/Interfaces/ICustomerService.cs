using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Customers;

namespace FrazerDealer.Application.Interfaces;

public interface ICustomerService
{
    Task<Result<IReadOnlyCollection<CustomerSummary>>> GetCustomersAsync(CancellationToken cancellationToken);
    Task<Result<CustomerDetail>> GetCustomerAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<CustomerDetail>> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken);
    Task<Result> UpdateCustomerAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken);
}
