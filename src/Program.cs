using Microsoft.Extensions.Configuration;
using SecurityPriceAlert.Communication;

namespace SecurityPriceAlert;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        AppSettings? settings = config.GetRequiredSection("AppSettings").Get<AppSettings>();
        if (settings == null) 
        {
            Console.WriteLine("Invalid Configuration file! Check appsettings.json.");
            return;
        }
        
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: StockQuoteAlert <securitySymbol> <sellTriggerPrice> <buyTriggerPrice>");
            return;
        }
        
        string securitySymbol = args[0];
        if (!double.TryParse(args[1], out double sellTriggerPrice))
        {
            Console.WriteLine("Invalid Sell Trigger. Please provide a valid number.");
            return;
        }

        if (!double.TryParse(args[2], out double buyTriggerPrice))
        {
            Console.WriteLine("Invalid Buy Trigger. Please provide a valid number.");
            return;
        }

        SecurityMonitor securityMonitor = new SecurityMonitor(settings.BrApiToken);
        MailHandler mailHandler = new MailHandler(settings.SmtpCredentials);
        
        bool? isValidSymbol = await securityMonitor.IsValidSymbol(securitySymbol);
        if (isValidSymbol == null)
        {
            Console.WriteLine("Could not validate symbol!");
            return;
        } 
        if ((bool)!isValidSymbol)
        {
            Console.WriteLine("Security symbol is invalid!");
            return;
        }
        
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