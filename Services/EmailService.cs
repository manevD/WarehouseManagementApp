using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Logging;

namespace WarehouseManagement.Services;

public class EmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IConfiguration config,
        ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendDemoRequest(DemoRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var smtpHost = _config["Smtp:Host"] ?? "smtp.ionos.com";
            var smtpPort = _config.GetValue<int?>("Smtp:Port") ?? 587;
            var smtpEmail = _config["Smtp:Email"]?.Trim();
            var smtpPassword = _config["Smtp:Password"];
            var toEmail = _config["Smtp:ToEmail"]?.Trim();

            Console.WriteLine("========== SMTP CONFIG ==========");
            Console.WriteLine($"Host: {smtpHost}");
            Console.WriteLine($"Port: {smtpPort}");
            Console.WriteLine($"Email: [{smtpEmail}]");
            Console.WriteLine($"Password exists: {!string.IsNullOrEmpty(smtpPassword)}");
            Console.WriteLine($"Password length: {smtpPassword?.Length ?? 0}");
            Console.WriteLine($"ToEmail: [{toEmail}]");
            Console.WriteLine("==================================");


            if (string.IsNullOrWhiteSpace(smtpEmail))
            {
                throw new InvalidOperationException(
                    "Smtp:Email не е конфигурирано.");
            }

            if (string.IsNullOrWhiteSpace(smtpPassword))
            {
                throw new InvalidOperationException(
                    "Smtp:Password не е конфигурирано.");
            }

            if (string.IsNullOrWhiteSpace(toEmail))
            {
                throw new InvalidOperationException(
                    "Smtp:ToEmail не е конфигурирано.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new InvalidOperationException(
                    "Е-mail адресата од формуларот е празна.");
            }

            if (!new EmailAddressAttribute().IsValid(request.Email))
            {
                throw new InvalidOperationException(
                    $"Невалидна customer e-mail адреса: {request.Email}");
            }

            _logger.LogInformation(
                "Обид за испраќање e-mail преку {Host}:{Port} од {From} до {To}",
                smtpHost,
                smtpPort,
                smtpEmail,
                toEmail);

            using var smtp = new SmtpClient
            {
                Host = smtpHost,
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    smtpEmail!,
                    smtpPassword!),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000
            };

            using var message = new MailMessage
            {
                From = new MailAddress(
                    smtpEmail!,
                    "Warehouse Management",
                    Encoding.UTF8),

                Subject = $"Neue Demo-Anfrage von {request.Name}",
                Body = BuildDemoEmail(request),
                IsBodyHtml = true,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8
            };

            message.To.Add(new MailAddress(toEmail!));

            message.ReplyToList.Add(
                new MailAddress(
                    request.Email,
                    request.Name,
                    Encoding.UTF8));

            await smtp.SendMailAsync(message);


            _logger.LogInformation(
                "E-mail успешно испратен до {To}",
                toEmail);
        }
        catch (SmtpException ex)
        {
            _logger.LogError(
                ex,
                "SMTP грешка при испраќање e-mail. StatusCode: {StatusCode}",
                ex.StatusCode);

            throw new InvalidOperationException(
                $"SMTP грешка: {ex.Message}",
                ex);
        }
        catch (FormatException ex)
        {
            _logger.LogError(
                ex,
                "Грешка во форматот на e-mail адресата.");

            throw new InvalidOperationException(
                $"Невалидна e-mail адреса: {ex.Message}",
                ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Општа грешка при испраќање e-mail.");

            throw;
        }
    }

    private string BuildDemoEmail(DemoRequest request)
    {
        var name = Html(request.Name);
        var company = Html(request.Company);
        var email = Html(request.Email);
        var industry = Html(request.Industry);

        var message = Html(
            string.IsNullOrWhiteSpace(request.Message)
                ? "Keine Nachricht angegeben."
                : request.Message);

        return $"""
            <!DOCTYPE html>
            <html lang="de">
            <body style="font-family:Arial,sans-serif;">
                <h2>Neue Demo-Anfrage</h2>

                <p><strong>Name:</strong> {name}</p>
                <p><strong>Unternehmen:</strong> {company}</p>
                <p><strong>E-Mail:</strong> {email}</p>
                <p><strong>Branche:</strong> {industry}</p>

                <hr>

                <p><strong>Nachricht:</strong></p>
                <p>{message}</p>
            </body>
            </html>
            """;
    }

    private static string Html(string? value)
    {
        return WebUtility
            .HtmlEncode(value ?? string.Empty)
            .Replace("\r\n", "  ")
            .Replace("\n", "  ")
            .Replace("\r", "  ");
    }

    public sealed class DemoRequest
    {
        [Required(ErrorMessage = "Bitte Namen eingeben.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte Unternehmen eingeben.")]
        public string Company { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte E-Mail eingeben.")]
        [EmailAddress(ErrorMessage = "Bitte gültige E-Mail eingeben.")]
        public string Email { get; set; } = string.Empty;

        public string Industry { get; set; } = "Großhandel";

        public string Message { get; set; } = string.Empty;
    }
}
