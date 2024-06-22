using MailKit.Net.Smtp;
using MimeKit;

namespace SecurityPriceAlert.Communication;

public class MailHandler
{
    private readonly SmtpCredentials _credentials;

    public MailHandler(SmtpCredentials credentials)
    {
        _credentials = credentials;
    }

    public async Task SendMailAsync(string to, string subject, string body)
    {
        MimeMessage msg = new MimeMessage()
        {
            From = { InternetAddress.Parse(_credentials.UserName) },
            To = { InternetAddress.Parse(to) },
            Subject = subject,
            Body = new TextPart {Text = body},
        };

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_credentials.Host, _credentials.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_credentials.UserName, _credentials.Password);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email could not be sent.\n{ex.Message}");
        }
    }
}

public record SmtpCredentials(
    string Host,
    int Port,
    string UserName,
    string Password
);