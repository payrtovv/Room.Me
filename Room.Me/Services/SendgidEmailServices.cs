using SendGrid;
using SendGrid.Helpers.Mail;


public class SendgidEmailServices
{
    private readonly SendGridClient _client;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public SendgidEmailServices(IConfiguration config)
    {
        _client = new SendGridClient(config["SendGrid:ApiKey"]);
        _fromEmail = config["SendGrid:FromEmail"];
        _fromName = config["SendGrid:FromName"];
    }

    public async Task SendEmailCode(string Email, string codigo)
    {
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(_fromEmail, _fromName),
            Subject = "Código de verificación",
            HtmlContent = $"<h2>Tu código es: {codigo}</h2><p>Expira en 5 minutos.</p>"
        };
        msg.AddTo(new EmailAddress(Email));

        var response = await _client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error enviando correo: {response.StatusCode}");
        }
    }
}

