using Microsoft.Extensions.Configuration;
using SecurityPriceAlert.Communication;

namespace SecurityPriceAlert;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        AppSettings? settings = config.GetRequiredSection("AppSettings").Get<AppSettings>();
        if (settings == null) return;
        BrApiHandler brApiHandler = new BrApiHandler();
        // MailHandler mailHandler = new MailHandler(settings.SmtpCredentials);
        // await mailHandler.SendMailAsync(settings.DestinationEmailAddress, "email test", "body.......");
        (await brApiHandler.GetSecuritiesBySymbol("PETR4", settings.BrApiToken))?.stocks.ForEach(Console.WriteLine);
        Console.WriteLine("--------");
        (await brApiHandler.GetSecuritiesBySymbol("PETRS4", settings.BrApiToken))?.stocks.ForEach(Console.WriteLine);
    }
}

public record AppSettings(
    SmtpCredentials SmtpCredentials,
    string DestinationEmailAddress,
    string BrApiToken
);