namespace FrazerDealer.Contracts.Jobs;

public record JobEnqueueRequest(string JobType, IDictionary<string, string>? Arguments);

public record JobStatusDto(Guid Id, string JobType, string Status, DateTime CreatedOn, DateTime? CompletedOn, string? Message);

public record JobHistoryRequest(string? Type);
