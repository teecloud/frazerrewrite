namespace FrazerDealer.Domain.Entities;

public class JobLog
{
    public Guid Id { get; set; }
    public Guid RecurringJobId { get; set; }
    public RecurringJob? RecurringJob { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime? CompletedOn { get; set; }
    public bool WasSuccessful { get; set; }
    public string? Message { get; set; }
}
