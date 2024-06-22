using SecurityPriceAlert.Communication;

namespace SecurityPriceAlert;

class Program
{
    static async Task Main(string[] args)
    {
        SmtpCredentials credentials =
            new SmtpCredentials("smtp.gmail.com", 587, "inoamailalert@gmail.com", "bbsacnyvffjygdur");
        MailHandler mailHandler = new MailHandler(credentials);
        await mailHandler.SendMailAsync("inoamailalert@gmail.com", "email test", "body.......");
    }
}