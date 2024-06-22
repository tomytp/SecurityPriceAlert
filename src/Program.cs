using Microsoft.Extensions.Configuration;
using SecurityPriceAlert.Communication;

namespace SecurityPriceAlert;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        SmtpCredentials? credentials = config.GetRequiredSection("SmtpCredentials").Get<SmtpCredentials>();
        if (credentials == null) return;
        MailHandler mailHandler = new MailHandler(credentials);
        await mailHandler.SendMailAsync("inoamailalert@gmail.com", "email test", "body.......");
    }
}