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

        SecurityMonitor securityMonitor = new SecurityMonitor(settings.BrApiToken);
        // MailHandler mailHandler = new MailHandler(settings.SmtpCredentials);
        // await mailHandler.SendMailAsync(settings.DestinationEmailAddress, "email test", "body.......");
        Console.WriteLine(await securityMonitor.IsValidSymbol("PETR4"));
        Console.WriteLine(await securityMonitor.IsValidSymbol("PETRS4"));
    }
}

public record AppSettings(
    SmtpCredentials SmtpCredentials,
    string DestinationEmailAddress,
    string BrApiToken
);