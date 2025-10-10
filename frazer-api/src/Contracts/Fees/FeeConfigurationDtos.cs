namespace FrazerDealer.Contracts.Fees;

public record FeeConfigurationDto(Guid Id, string Code, string Description, decimal Amount, bool IsActive);

public record UpdateFeeConfigurationRequest(string Description, decimal Amount, bool IsActive);
