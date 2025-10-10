namespace FrazerDealer.Infrastructure.Adapters;

public class AdapterOptions
{
    public const string SectionName = "Adapters";

    public FrazerHubOptions FrazerHub { get; set; } = new();
    public TextMaxxOptions TextMaxx { get; set; } = new();
    public SiriusXmOptions SiriusXm { get; set; } = new();
    public CardPointeOptions CardPointe { get; set; } = new();

    public class FrazerHubOptions
    {
        public string BaseUrl { get; set; } = "https://frazerhub.local";
    }

    public class TextMaxxOptions
    {
        public string ApiKey { get; set; } = "test-key";
    }

    public class SiriusXmOptions
    {
        public string ApiKey { get; set; } = "test-key";
    }

    public class CardPointeOptions
    {
        public string MerchantId { get; set; } = "merchant";
        public string ApiUser { get; set; } = "user";
        public string ApiKey { get; set; } = "key";
    }
}
