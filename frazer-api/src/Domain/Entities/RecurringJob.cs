namespace FrazerDealer.Domain.Entities;

public class RecurringJob
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CronExpression { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? LastRun { get; set; }
    public bool IsActive { get; set; } = true;
}
