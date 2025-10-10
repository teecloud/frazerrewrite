using FrazerDealer.Application.Interfaces.Adapters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FrazerDealer.Infrastructure.Adapters;

public class FakeFrazerHubClient : IFrazerHubClient
{
    private readonly ILogger<FakeFrazerHubClient> _logger;
    private readonly AdapterOptions _options;

    public FakeFrazerHubClient(IOptions<AdapterOptions> options, ILogger<FakeFrazerHubClient> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task PushInventoryAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[FAKE] Syncing inventory to FrazerHub at {BaseUrl}", _options.FrazerHub.BaseUrl);
        return Task.CompletedTask;
    }
}

public class FakeTextMaxxSms : ITextMaxxSms
{
    private readonly ILogger<FakeTextMaxxSms> _logger;
    private readonly AdapterOptions _options;

    public FakeTextMaxxSms(IOptions<AdapterOptions> options, ILogger<FakeTextMaxxSms> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task SendMessageAsync(string phoneNumber, string message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[FAKE] Sending SMS to {Phone} via TextMaxx using key {Key}", phoneNumber, _options.TextMaxx.ApiKey);
        return Task.CompletedTask;
    }
}

public class FakeSiriusXmApi : ISiriusXmApi
{
    private readonly ILogger<FakeSiriusXmApi> _logger;
    private readonly AdapterOptions _options;

    public FakeSiriusXmApi(IOptions<AdapterOptions> options, ILogger<FakeSiriusXmApi> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task RegisterTrialAsync(string vin, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[FAKE] Registering SiriusXM trial for {Vin} using key {Key}", vin, _options.SiriusXm.ApiKey);
        return Task.CompletedTask;
    }
}

public class FakeCardPointePayments : ICardPointePayments
{
    private readonly ILogger<FakeCardPointePayments> _logger;
    private readonly AdapterOptions _options;

    public FakeCardPointePayments(IOptions<AdapterOptions> options, ILogger<FakeCardPointePayments> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task<string> AuthorizeAsync(decimal amount, CancellationToken cancellationToken)
    {
        var authorization = Guid.NewGuid().ToString("N");
        _logger.LogInformation("[FAKE] Authorizing ${Amount} with CardPointe merchant {MerchantId} => {Authorization}", amount, _options.CardPointe.MerchantId, authorization);
        return Task.FromResult(authorization);
    }

    public Task<bool> CaptureAsync(string authorizationCode, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[FAKE] Capturing authorization {Authorization} with CardPointe user {User}", authorizationCode, _options.CardPointe.ApiUser);
        return Task.FromResult(true);
    }
}
