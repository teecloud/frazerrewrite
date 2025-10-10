namespace FrazerDealer.Contracts.Customers;

public record CustomerSummary(Guid Id, string FirstName, string LastName, string Email, string Phone);

public record CustomerDetail(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Address,
    string City,
    string State,
    string PostalCode);

public record CreateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Address,
    string City,
    string State,
    string PostalCode);

public record UpdateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Address,
    string City,
    string State,
    string PostalCode);
