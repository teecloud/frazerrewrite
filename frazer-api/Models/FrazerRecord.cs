using System.ComponentModel.DataAnnotations;

namespace frazer_api.Models;

public class FrazerRecord
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? ContactNumber { get; set; }

    [StringLength(120)]
    public string? Vehicle { get; set; }

    [Required]
    [StringLength(40)]
    public string Status { get; set; } = "Pending";

    [Range(0, double.MaxValue)]
    public decimal Balance { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
