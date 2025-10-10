namespace FrazerDealer.Contracts.Insurance;

public record InsuranceProviderDto(Guid Id, string Name, string Phone, string Email, string Notes, bool IsActive);

public record UpsertInsuranceProviderRequest(string Name, string Phone, string Email, string Notes, bool IsActive);
