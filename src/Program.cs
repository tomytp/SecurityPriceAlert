using Microsoft.Extensions.Configuration;
using SecurityPriceAlert.Communication;

namespace SecurityPriceAlert;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        string securitySymbol = "PETR4";
        double sellTriggerPrice = 40.0;
        double buyTriggerPrice = 38.0;
        
        AppSettings? settings = config.GetRequiredSection("AppSettings").Get<AppSettings>();
        if (settings == null) return;

        SecurityMonitor securityMonitor = new SecurityMonitor(settings.BrApiToken);
        MailHandler mailHandler = new MailHandler(settings.SmtpCredentials);
        
        securityMonitor.SellTrigger += async (_, quote) =>
        {
            string subject = $"Alerta de venda para {quote.Symbol}!";
            string body = $"O preco de {quote.LongName} ({quote.Symbol}) ultrapassou o limite de venda de {sellTriggerPrice}. Preco atual: {quote.RegularMarketPrice}";
            await mailHandler.SendMailAsync(settings.DestinationEmailAddress, subject, body);
        };

        securityMonitor.BuyTrigger += async (_, quote) =>
        {
            string subject = $"Alerta de compra para {quote.Symbol}!";
            string body = $"O preco de {quote.LongName} ({quote.Symbol}) ultrapassou o limite de compra de {buyTriggerPrice}. Preco atual: {quote.RegularMarketPrice}";
            await mailHandler.SendMailAsync(settings.DestinationEmailAddress, subject, body);
        };
        
        while (true)
        {
            await securityMonitor.CheckSecurity(securitySymbol, sellTriggerPrice, buyTriggerPrice);
            await Task.Delay(10000); // Refresh rate de 10 segundos para limitar o consumo da API
        }
    }
}

public record AppSettings(
    SmtpCredentials SmtpCredentials,
    string DestinationEmailAddress,
    string BrApiToken
);