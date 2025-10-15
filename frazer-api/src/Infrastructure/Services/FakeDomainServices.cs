using System.Linq;
using FrazerDealer.Application.Common;
using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Customers;
using FrazerDealer.Contracts.Fees;
using FrazerDealer.Contracts.Insurance;
using FrazerDealer.Contracts.Inventory;
using FrazerDealer.Contracts.Jobs;
using FrazerDealer.Contracts.Payments;
using FrazerDealer.Contracts.Prospects;
using FrazerDealer.Contracts.Photos;
using FrazerDealer.Contracts.Reports;
using FrazerDealer.Contracts.Sales;
using FrazerDealer.Domain.Entities;
using FrazerDealer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrazerDealer.Infrastructure.Services;

public class FakeVehicleService : IVehicleService
{
    private readonly AppDbContext _context;

    public FakeVehicleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyCollection<VehicleSummary>>> GetInventoryAsync(CancellationToken cancellationToken)
    {
        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Select(v => new VehicleSummary(
                v.Id,
                v.StockNumber,
                v.Vin,
                v.Year,
                v.Make,
                v.Model,
                v.IsSold,
                v.Photos
                    .Where(p => p.IsPrimary)
                    .Select(p => p.Url)
                    .FirstOrDefault()))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<VehicleSummary>>.Success(vehicles);
    }

    public async Task<Result<VehicleDetail>> GetVehicleAsync(Guid id, CancellationToken cancellationToken)
    {
        var vehicle = await _context.Vehicles
            .AsNoTracking()
            .Include(v => v.Photos)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        if (vehicle is null)
        {
            return Result<VehicleDetail>.Failure("Vehicle not found");
        }

        return Result<VehicleDetail>.Success(new VehicleDetail(
            vehicle.Id,
            vehicle.StockNumber,
            vehicle.Vin,
            vehicle.Year,
            vehicle.Make,
            vehicle.Model,
            vehicle.Trim,
            vehicle.Price,
            vehicle.Cost,
            vehicle.IsSold,
            vehicle.DateArrived,
            vehicle.DateSold,
            vehicle.CurrentSaleId,
            vehicle.Photos
                .OrderByDescending(p => p.IsPrimary)
                .ThenBy(p => p.Url)
                .Select(p => new PhotoSummary(p.Id, p.VehicleId, p.Url, p.Caption, p.IsPrimary))
                .ToList()));
    }

    public async Task<Result<VehicleDetail>> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            StockNumber = request.StockNumber,
            Vin = request.Vin,
            Year = request.Year,
            Make = request.Make,
            Model = request.Model,
            Trim = request.Trim,
            Price = request.Price,
            Cost = request.Cost,
            DateArrived = request.DateArrived
        };

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync(cancellationToken);

        return await GetVehicleAsync(vehicle.Id, cancellationToken);
    }

    public async Task<Result> UpdateVehicleAsync(Guid id, UpdateVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        if (vehicle is null)
        {
            return Result.Failure("Vehicle not found");
        }

        vehicle.StockNumber = request.StockNumber;
        vehicle.Year = request.Year;
        vehicle.Make = request.Make;
        vehicle.Model = request.Model;
        vehicle.Trim = request.Trim;
        vehicle.Price = request.Price;
        vehicle.Cost = request.Cost;
        vehicle.DateArrived = request.DateArrived;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> MarkVehicleSoldAsync(Guid id, CancellationToken cancellationToken)
    {
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        if (vehicle is null)
        {
            return Result.Failure("Vehicle not found");
        }

        vehicle.IsSold = true;
        vehicle.DateSold = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

public class FakePhotoService : IPhotoService
{
    private readonly AppDbContext _context;

    public FakePhotoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyCollection<PhotoSummary>>> GetPhotosAsync(Guid? vehicleId, CancellationToken cancellationToken)
    {
        var query = _context.Photos.AsNoTracking();

        if (vehicleId.HasValue)
        {
            query = query.Where(p => p.VehicleId == vehicleId.Value);
        }

        var photos = await query
            .OrderByDescending(p => p.IsPrimary)
            .ThenBy(p => p.Url)
            .Select(p => new PhotoSummary(p.Id, p.VehicleId, p.Url, p.Caption, p.IsPrimary))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<PhotoSummary>>.Success(photos);
    }

    public async Task<Result<PhotoDetail>> GetPhotoAsync(Guid id, CancellationToken cancellationToken)
    {
        var photo = await _context.Photos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (photo is null)
        {
            return Result<PhotoDetail>.Failure("Photo not found");
        }

        return Result<PhotoDetail>.Success(new PhotoDetail(photo.Id, photo.VehicleId, photo.Url, photo.Caption, photo.IsPrimary));
    }

    public async Task<Result<PhotoDetail>> CreatePhotoAsync(CreatePhotoRequest request, CancellationToken cancellationToken)
    {
        var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == request.VehicleId, cancellationToken);
        if (!vehicleExists)
        {
            return Result<PhotoDetail>.Failure("Vehicle not found");
        }

        if (request.IsPrimary)
        {
            await _context.Photos
                .Where(p => p.VehicleId == request.VehicleId && p.IsPrimary)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsPrimary, _ => false), cancellationToken);
        }

        var photo = new Photo
        {
            Id = Guid.NewGuid(),
            VehicleId = request.VehicleId,
            Url = request.Url,
            Caption = request.Caption,
            IsPrimary = request.IsPrimary,
        };

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<PhotoDetail>.Success(new PhotoDetail(photo.Id, photo.VehicleId, photo.Url, photo.Caption, photo.IsPrimary));
    }

    public async Task<Result> UpdatePhotoAsync(Guid id, UpdatePhotoRequest request, CancellationToken cancellationToken)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (photo is null)
        {
            return Result.Failure("Photo not found");
        }

        if (request.IsPrimary)
        {
            await _context.Photos
                .Where(p => p.VehicleId == photo.VehicleId && p.IsPrimary && p.Id != id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsPrimary, _ => false), cancellationToken);
        }

        photo.Url = request.Url;
        photo.Caption = request.Caption;
        photo.IsPrimary = request.IsPrimary;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeletePhotoAsync(Guid id, CancellationToken cancellationToken)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (photo is null)
        {
            return Result.Failure("Photo not found");
        }

        var wasPrimary = photo.IsPrimary;
        var vehicleId = photo.VehicleId;

        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync(cancellationToken);

        if (wasPrimary)
        {
            var nextPrimary = await _context.Photos
                .Where(p => p.VehicleId == vehicleId)
                .OrderBy(p => p.Url)
                .FirstOrDefaultAsync(cancellationToken);

            if (nextPrimary is not null)
            {
                nextPrimary.IsPrimary = true;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        return Result.Success();
    }
}

public class FakeCustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public FakeCustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyCollection<CustomerSummary>>> GetCustomersAsync(CancellationToken cancellationToken)
    {
        var customers = await _context.Customers.AsNoTracking()
            .Select(c => new CustomerSummary(c.Id, c.FirstName, c.LastName, c.Email, c.Phone))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<CustomerSummary>>.Success(customers);
    }

    public async Task<Result<CustomerDetail>> GetCustomerAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (customer is null)
        {
            return Result<CustomerDetail>.Failure("Customer not found");
        }

        return Result<CustomerDetail>.Success(new CustomerDetail(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.Phone,
            customer.Address,
            customer.City,
            customer.State,
            customer.PostalCode));
    }

    public async Task<Result<CustomerDetail>> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return await GetCustomerAsync(customer.Id, cancellationToken);
    }

    public async Task<Result> UpdateCustomerAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (customer is null)
        {
            return Result.Failure("Customer not found");
        }

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.Address = request.Address;
        customer.City = request.City;
        customer.State = request.State;
        customer.PostalCode = request.PostalCode;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

public class FakeProspectService : IProspectService
{
    private readonly AppDbContext _context;

    public FakeProspectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyCollection<ProspectSummary>>> GetProspectsAsync(CancellationToken cancellationToken)
    {
        var prospects = await _context.Prospects.AsNoTracking()
            .Select(p => new ProspectSummary(
                p.Id,
                p.Name,
                p.Email,
                p.Phone,
                p.Vehicles.Select(v => new ProspectVehicleSummary(v.Id, v.StockNumber, v.Year, v.Make, v.Model)).ToList()))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<ProspectSummary>>.Success(prospects);
    }

    public async Task<Result<ProspectSummary>> GetProspectAsync(Guid id, CancellationToken cancellationToken)
    {
        var prospect = await _context.Prospects.AsNoTracking()
            .Select(p => new ProspectSummary(
                p.Id,
                p.Name,
                p.Email,
                p.Phone,
                p.Vehicles.Select(v => new ProspectVehicleSummary(v.Id, v.StockNumber, v.Year, v.Make, v.Model)).ToList()))
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (prospect is null)
        {
            return Result<ProspectSummary>.Failure("Prospect not found");
        }

        return Result<ProspectSummary>.Success(prospect);
    }

    public async Task<Result<ProspectSummary>> CreateProspectAsync(CreateProspectRequest request, CancellationToken cancellationToken)
    {
        var vehicles = await _context.Vehicles
            .Where(v => request.VehicleIds.Contains(v.Id))
            .ToListAsync(cancellationToken);

        var prospect = new Prospect
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Vehicles = vehicles
        };

        _context.Prospects.Add(prospect);
        await _context.SaveChangesAsync(cancellationToken);

        return await GetProspectAsync(prospect.Id, cancellationToken);
    }
}

public class FakeSaleService : ISaleService
{
    private readonly AppDbContext _context;

    public FakeSaleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyCollection<SaleSummary>>> GetSalesAsync(CancellationToken cancellationToken)
    {
        var sales = await _context.Sales.AsNoTracking()
            .Select(s => new SaleSummary(s.Id, s.VehicleId, s.CustomerId, s.Subtotal, s.FeesTotal, s.PaymentsTotal, s.Status.ToString(), s.CreatedOn))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<SaleSummary>>.Success(sales);
    }

    public async Task<Result<SaleDetail>> GetSaleAsync(Guid id, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales.AsNoTracking()
            .Include(s => s.Fees)
            .Include(s => s.Payments)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (sale is null)
        {
            return Result<SaleDetail>.Failure("Sale not found");
        }

        return Result<SaleDetail>.Success(MapSaleDetail(sale));
    }

    public async Task<Result<SaleDetail>> CreateDraftAsync(CreateSaleDraftRequest request, CancellationToken cancellationToken)
    {
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            VehicleId = request.VehicleId,
            CustomerId = request.CustomerId,
            Subtotal = request.Subtotal,
            Status = SaleStatus.Draft
        };

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync(cancellationToken);

        return await GetSaleAsync(sale.Id, cancellationToken);
    }

    public async Task<Result<SaleDetail>> AddFeeAsync(Guid id, AddSaleFeeRequest request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales.Include(s => s.Fees).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (sale is null)
        {
            return Result<SaleDetail>.Failure("Sale not found");
        }

        var fee = new Fee
        {
            Id = Guid.NewGuid(),
            SaleId = sale.Id,
            Code = request.Code,
            Description = request.Description,
            Amount = request.Amount,
            IsRecurring = request.IsRecurring
        };

        sale.Fees.Add(fee);
        sale.FeesTotal = sale.Fees.Sum(f => f.Amount);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<SaleDetail>.Success(MapSaleDetail(sale));
    }

    public async Task<Result<SaleDetail>> AddPaymentAsync(Guid id, AddSalePaymentRequest request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales.Include(s => s.Payments).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (sale is null)
        {
            return Result<SaleDetail>.Failure("Sale not found");
        }

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            SaleId = sale.Id,
            Amount = request.Amount,
            CollectedOn = request.CollectedOn ?? DateTime.UtcNow,
            Method = request.Method,
            Status = PaymentStatus.Settled,
            ExternalReference = request.ExternalReference
        };

        sale.Payments.Add(payment);
        sale.PaymentsTotal = sale.Payments.Sum(p => p.Amount);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<SaleDetail>.Success(MapSaleDetail(sale));
    }

    public async Task<Result<SaleDetail>> CompleteSaleAsync(Guid id, CompleteSaleRequest request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales.Include(s => s.Fees).Include(s => s.Payments).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (sale is null)
        {
            return Result<SaleDetail>.Failure("Sale not found");
        }

        sale.Status = SaleStatus.Completed;
        sale.CompletedOn = request.CompletedOn ?? DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return Result<SaleDetail>.Success(MapSaleDetail(sale));
    }

    public async Task<Result<SaleReceiptDto>> GetReceiptAsync(Guid id, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales.AsNoTracking()
            .Include(s => s.Fees)
            .Include(s => s.Payments)
            .Include(s => s.Customer)
            .Include(s => s.Vehicle)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (sale is null)
        {
            return Result<SaleReceiptDto>.Failure("Sale not found");
        }

        var detail = MapSaleDetail(sale);
        var customer = sale.Customer ?? new Customer();
        var vehicle = sale.Vehicle ?? new Vehicle();

        var receipt = new SaleReceiptDto(
            detail,
            new CustomerInfo(customer.Id, $"{customer.FirstName} {customer.LastName}".Trim(), customer.Email, customer.Phone, customer.Address),
            new VehicleInfo(vehicle.Id, vehicle.StockNumber, vehicle.Vin, vehicle.Year, vehicle.Make, vehicle.Model, vehicle.Trim));

        return Result<SaleReceiptDto>.Success(receipt);
    }

    private static SaleDetail MapSaleDetail(Sale sale)
    {
        return new SaleDetail(
            sale.Id,
            sale.VehicleId,
            sale.CustomerId,
            sale.Subtotal,
            sale.FeesTotal,
            sale.PaymentsTotal,
            sale.BalanceDue,
            sale.Status.ToString(),
            sale.CreatedOn,
            sale.CompletedOn,
            sale.Fees.Select(f => new SaleFeeDto(f.Id, f.Code, f.Description, f.Amount, f.IsRecurring)).ToList(),
            sale.Payments.Select(p => new SalePaymentDto(p.Id, p.Amount, p.CollectedOn, p.Method, p.Status.ToString(), p.ExternalReference)).ToList());
    }
}

public class FakeFeeService : IFeeService
{
    private readonly AppDbContext _context;

    public FakeFeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyCollection<FeeConfigurationDto>>> GetFeesAsync(CancellationToken cancellationToken)
    {
        var fees = await _context.Fees.AsNoTracking()
            .GroupBy(f => f.Code)
            .Select(group => new FeeConfigurationDto(Guid.NewGuid(), group.Key, group.First().Description, group.Sum(f => f.Amount), true))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<FeeConfigurationDto>>.Success(fees);
    }

    public Task<Result> UpdateFeeAsync(Guid id, UpdateFeeConfigurationRequest request, CancellationToken cancellationToken)
    {
        // Placeholder: in a full implementation we would update reference data.
        return Task.FromResult(Result.Success());
    }
}

public class FakeInsuranceService : IInsuranceService
{
    private readonly AppDbContext _context;

    public FakeInsuranceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyCollection<InsuranceProviderDto>>> GetProvidersAsync(CancellationToken cancellationToken)
    {
        var providers = await _context.InsuranceProviders.AsNoTracking()
            .Select(p => new InsuranceProviderDto(p.Id, p.Name, p.Phone, p.Email, p.Notes, p.IsActive))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<InsuranceProviderDto>>.Success(providers);
    }

    public async Task<Result<InsuranceProviderDto>> UpsertProviderAsync(Guid? id, UpsertInsuranceProviderRequest request, CancellationToken cancellationToken)
    {
        InsuranceProvider provider;
        if (id.HasValue)
        {
            provider = await _context.InsuranceProviders.FirstOrDefaultAsync(p => p.Id == id.Value, cancellationToken);
            if (provider is null)
            {
                return Result<InsuranceProviderDto>.Failure("Provider not found");
            }
        }
        else
        {
            provider = new InsuranceProvider { Id = Guid.NewGuid() };
            _context.InsuranceProviders.Add(provider);
        }

        provider.Name = request.Name;
        provider.Phone = request.Phone;
        provider.Email = request.Email;
        provider.Notes = request.Notes;
        provider.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<InsuranceProviderDto>.Success(new InsuranceProviderDto(provider.Id, provider.Name, provider.Phone, provider.Email, provider.Notes, provider.IsActive));
    }
}

public class FakePaymentService : IPaymentService
{
    private readonly AppDbContext _context;

    public FakePaymentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyCollection<PaymentDashboardItem>>> GetDashboardAsync(CancellationToken cancellationToken)
    {
        var payments = await _context.Payments.AsNoTracking()
            .OrderByDescending(p => p.CollectedOn)
            .Select(p => new PaymentDashboardItem(p.Id, p.SaleId, p.Amount, p.Method, p.Status.ToString(), p.CollectedOn, p.ExternalReference))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<PaymentDashboardItem>>.Success(payments);
    }

    public Task<Result> RetryPaymentAsync(Guid id, PaymentRetryRequest request, CancellationToken cancellationToken)
    {
        // Placeholder for integration with payment gateway.
        return Task.FromResult(Result.Success());
    }
}

public class FakeReportsService : IReportsService
{
    private readonly AppDbContext _context;

    public FakeReportsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyCollection<InventoryReportRow>>> GetInventoryReportAsync(CancellationToken cancellationToken)
    {
        var data = await _context.Vehicles.AsNoTracking()
            .Select(v => new InventoryReportRow(v.StockNumber, v.Vin, v.Year, v.Make, v.Model, v.DateArrived, v.IsSold, v.DateSold))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<InventoryReportRow>>.Success(data);
    }

    public async Task<Result<IReadOnlyCollection<TitlesPendingReportRow>>> GetTitlesPendingReportAsync(string? filter, CancellationToken cancellationToken)
    {
        var data = await _context.Sales.AsNoTracking()
            .Where(s => s.Status == SaleStatus.Completed)
            .Select(s => new TitlesPendingReportRow(
                s.Vehicle != null ? s.Vehicle.StockNumber : string.Empty,
                s.Vehicle != null ? s.Vehicle.Vin : string.Empty,
                s.Customer != null ? $"{s.Customer.FirstName} {s.Customer.LastName}".Trim() : string.Empty,
                s.CompletedOn,
                s.BalanceDue == 0 ? "Ready" : "Pending"))
            .ToListAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(filter))
        {
            data = data.Where(row => row.CustomerName.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return Result<IReadOnlyCollection<TitlesPendingReportRow>>.Success(data);
    }

    public async Task<Result<IReadOnlyCollection<InsuranceReportRow>>> GetInsuranceReportAsync(CancellationToken cancellationToken)
    {
        var data = await _context.InsuranceProviders.AsNoTracking()
            .Select(p => new InsuranceReportRow(p.Name, 0, 0, DateTime.UtcNow))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<InsuranceReportRow>>.Success(data);
    }
}

public class FakeJobService : IJobService
{
    private readonly AppDbContext _context;

    public FakeJobService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<JobStatusDto>> EnqueueJobAsync(JobEnqueueRequest request, CancellationToken cancellationToken)
    {
        var log = new JobLog
        {
            Id = Guid.NewGuid(),
            RecurringJobId = Guid.Empty,
            StartedOn = DateTime.UtcNow,
            CompletedOn = DateTime.UtcNow,
            WasSuccessful = true,
            Message = $"Enqueued job {request.JobType}"
        };

        _context.JobLogs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<JobStatusDto>.Success(new JobStatusDto(log.Id, request.JobType, "Completed", log.StartedOn, log.CompletedOn, log.Message));
    }

    public async Task<Result<JobStatusDto>> GetStatusAsync(Guid id, CancellationToken cancellationToken)
    {
        var log = await _context.JobLogs.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        if (log is null)
        {
            return Result<JobStatusDto>.Failure("Job not found");
        }

        return Result<JobStatusDto>.Success(new JobStatusDto(log.Id, "Ad-hoc", log.WasSuccessful ? "Completed" : "Failed", log.StartedOn, log.CompletedOn, log.Message));
    }

    public async Task<Result<IReadOnlyCollection<JobStatusDto>>> GetHistoryAsync(JobHistoryRequest request, CancellationToken cancellationToken)
    {
        var logs = await _context.JobLogs.AsNoTracking()
            .OrderByDescending(l => l.StartedOn)
            .Take(50)
            .Select(l => new JobStatusDto(l.Id, request.Type ?? "Recurring", l.WasSuccessful ? "Completed" : "Failed", l.StartedOn, l.CompletedOn, l.Message))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<JobStatusDto>>.Success(logs);
    }
}
