using SendGrid;
using SendGrid.Helpers.Mail;


public class SendgidEmailServices
{
    private readonly SendGridClient _client;
    private readonly EmailAddress _from;


    public SendgidEmailServices(IConfiguration config)
    {
        //Obtener las keys de las variables de entorno
        var apiKey = config["SendGrid"];

        //Por si no ponen
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new Exception("La API Key de SendGrid no está configurada");
        }

        //iniciamos coneccion 
        _client = new SendGridClient(apiKey);
        _from = new EmailAddress("payrtovv@gmail.com", "RoomMe");
    }

    //metodo para mandar el email
    public async Task SendEmailCode(string email, string codigo)
    {
        //Creamos el mensaje
        var msg = new SendGridMessage
        {
            From = _from,
            Subject = "Código de verificación",
            HtmlContent = $@"
                <h2>Tu código es: {codigo}</h2>
                <p>Expira en 5 minutos.</p>"
        };

        msg.AddTo(new EmailAddress(email));
        //mandamos el mensaje con el cliente que creamos antes
        var response = await _client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error enviando correo: {response.StatusCode}");
        }
    }

}

