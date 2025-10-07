using System.Text.Json;
using frazer_api.Models;

namespace frazer_api.Services;

public class FrazerRepository : IFrazerRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
    };

    private readonly ILogger<FrazerRepository> _logger;
    private readonly string _dataFilePath;
    private readonly SemaphoreSlim _mutex = new(1, 1);

    public FrazerRepository(IWebHostEnvironment environment, ILogger<FrazerRepository> logger)
    {
        _logger = logger;
        _dataFilePath = Path.Combine(environment.ContentRootPath, "Data", "frazer-data.json");
        Directory.CreateDirectory(Path.GetDirectoryName(_dataFilePath)!);
    }

    public async Task<IReadOnlyList<FrazerRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            var records = await ReadRecordsAsync(cancellationToken);
            return records
                .OrderByDescending(r => r.UpdatedAt)
                .ThenByDescending(r => r.CreatedAt)
                .ToList();
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task<FrazerRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            var records = await ReadRecordsAsync(cancellationToken);
            return records.FirstOrDefault(r => r.Id == id);
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task<FrazerRecord> CreateAsync(FrazerRecordCreateDto request, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            var records = await ReadRecordsAsync(cancellationToken);
            var now = DateTimeOffset.UtcNow;
            var nextId = records.Count == 0 ? 1 : records.Max(r => r.Id) + 1;

            var record = new FrazerRecord
            {
                Id = nextId,
                CustomerName = request.CustomerName.Trim(),
                ContactNumber = string.IsNullOrWhiteSpace(request.ContactNumber) ? null : request.ContactNumber.Trim(),
                Vehicle = string.IsNullOrWhiteSpace(request.Vehicle) ? null : request.Vehicle.Trim(),
                Status = request.Status.Trim(),
                Balance = decimal.Round(request.Balance, 2),
                Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
                CreatedAt = now,
                UpdatedAt = now,
            };

            records.Add(record);
            await WriteRecordsAsync(records, cancellationToken);
            return record;
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task<FrazerRecord?> UpdateAsync(int id, FrazerRecordUpdateDto request, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            var records = await ReadRecordsAsync(cancellationToken);
            var existing = records.FirstOrDefault(r => r.Id == id);
            if (existing == null)
            {
                return null;
            }

            existing.CustomerName = request.CustomerName.Trim();
            existing.ContactNumber = string.IsNullOrWhiteSpace(request.ContactNumber) ? null : request.ContactNumber.Trim();
            existing.Vehicle = string.IsNullOrWhiteSpace(request.Vehicle) ? null : request.Vehicle.Trim();
            existing.Status = request.Status.Trim();
            existing.Balance = decimal.Round(request.Balance, 2);
            existing.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await WriteRecordsAsync(records, cancellationToken);
            return existing;
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            var records = await ReadRecordsAsync(cancellationToken);
            var removed = records.RemoveAll(r => r.Id == id);
            if (removed == 0)
            {
                return false;
            }

            await WriteRecordsAsync(records, cancellationToken);
            return true;
        }
        finally
        {
            _mutex.Release();
        }
    }

    private async Task<List<FrazerRecord>> ReadRecordsAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_dataFilePath))
        {
            _logger.LogInformation("Frazer data file not found. Creating seed data at {Path}", _dataFilePath);
            await WriteRecordsAsync(CreateSeedData(), cancellationToken);
        }

        await using var stream = File.OpenRead(_dataFilePath);
        var records = await JsonSerializer.DeserializeAsync<List<FrazerRecord>>(stream, SerializerOptions, cancellationToken);
        return records ?? new List<FrazerRecord>();
    }

    private async Task WriteRecordsAsync(List<FrazerRecord> records, CancellationToken cancellationToken)
    {
        await using var stream = File.Create(_dataFilePath);
        await JsonSerializer.SerializeAsync(stream, records, SerializerOptions, cancellationToken);
    }

    private static List<FrazerRecord> CreateSeedData()
    {
        var now = DateTimeOffset.UtcNow;
        return new List<FrazerRecord>
        {
            new()
            {
                Id = 1,
                CustomerName = "Charlotte Reynolds",
                ContactNumber = "555-0134",
                Vehicle = "2019 Honda Civic",
                Status = "In Progress",
                Balance = 1250.00m,
                Notes = "Waiting on parts shipment from supplier.",
                CreatedAt = now.AddDays(-6),
                UpdatedAt = now.AddDays(-1)
            },
            new()
            {
                Id = 2,
                CustomerName = "James Thornton",
                ContactNumber = "555-0082",
                Vehicle = "2021 Ford F-150",
                Status = "Completed",
                Balance = 0m,
                Notes = "Delivered to customer 2 days ago.",
                CreatedAt = now.AddDays(-10),
                UpdatedAt = now.AddDays(-2)
            },
            new()
            {
                Id = 3,
                CustomerName = "Priya Desai",
                ContactNumber = "555-0220",
                Vehicle = "2020 Toyota RAV4",
                Status = "Pending",
                Balance = 580.75m,
                Notes = "Follow up with customer for approval of final work.",
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now
            }
        };
    }
}
