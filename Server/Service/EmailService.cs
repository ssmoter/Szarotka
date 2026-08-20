using MailKit.Security;

using Microsoft.Extensions.Logging.Abstractions;

using MimeKit;

using Server.Model;

namespace Server.Service
{
    public interface IEmailService
    {
        MimeMessage CreatedMessage(string to, string subject, string body, string bodyType = "html");
        Task SendMessage(MimeMessage messages, CancellationToken token = default);
        Task SendMessage(string to, string subject, string body, CancellationToken token = default);
        Task SendMessages(IList<MimeMessage> messages, CancellationToken token = default);
    }

    public class EmailService(IConfiguration configuration, ILogger<EmailService>? logger = null) : IEmailService
    {
        private readonly EmailConfiguration? _emailConfig = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
        private readonly SecureSocketOptions _secureSocketOptions = SecureSocketOptions.StartTls;
        private readonly ILogger<EmailService> _logger = logger ?? NullLogger<EmailService>.Instance;

        public async Task SendMessage(string to, string subject, string body, CancellationToken token = default)
        {
            //_logger.LogInformation("DEBUG CONFIG: Host={Host}, User={User}, PassLength={Pass}", _emailConfig?.SmtpServer, _emailConfig?.UserName, _emailConfig?.Password?.Length ?? 0);
            _logger.LogInformation("SendMessage(to) started for to={To} subject={Subject}", to, subject);
            var message = CreatedMessage(to, subject, body);
            await SendMessage(message, token);
            _logger.LogInformation("SendMessage(to) completed for to={To}", to);
        }

        public async Task SendMessage(MimeMessage messages, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(_emailConfig, nameof(_emailConfig));

            using var client = new MailKit.Net.Smtp.SmtpClient();

            // 1. Ignorujemy błędne certyfikaty Ethereal
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            // 2. Wymuszamy IPv4 (zapobiega timeoutom routingu IPv6 w GCP)
            client.LocalEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);

            // 3. WYMUSZENIE NOWOCZESNYCH PROTOKOŁÓW TLS (Dodaj tę linię):
            client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;

            try
            {
                // 4. WAŻNE: Dla portu 587 MUSI być SecureSocketOptions.StartTls
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.StartTls, token);

                client.AuthenticationMechanisms.Remove("DIGEST-MD5");
                client.AuthenticationMechanisms.Remove("CRAM-MD5");

                var cleanUser = _emailConfig.UserName.Trim();
                var cleanPass = _emailConfig.Password.Trim();

                await client.AuthenticateAsync(cleanUser, cleanPass, token);

                await client.SendAsync(messages, token);
                await client.DisconnectAsync(true, token);

                _logger.LogInformation("Mail wysłany pomyślnie!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMessage failed on Cloud Run");
                throw;
            }
        }

        public async Task SendMessages(IList<MimeMessage> messages, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(_emailConfig, nameof(_emailConfig));

            if (messages == null || messages.Count == 0)
            {
                _logger.LogInformation("SendMessages: No messages to send.");
                return;
            }

            _logger.LogInformation("SendMessages: Starting batch send for {Count} messages.", messages.Count);
            using var client = new MailKit.Net.Smtp.SmtpClient();

            // 1. Konfiguracja sieciowa pod Cloud Run (Bypass SSL i IPv4)
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.LocalEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);
            client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;

            try
            {
                _logger.LogInformation("SendMessages: Connecting to SMTP server {Server}:{Port}", _emailConfig.SmtpServer, _emailConfig.Port);

                // Zmiana na StartTls i port 587 (zgodnie z działającą konfiguracją)
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.StartTls, token);

                client.AuthenticationMechanisms.Remove("DIGEST-MD5");
                client.AuthenticationMechanisms.Remove("CRAM-MD5");

                var cleanUser = _emailConfig.UserName.Trim();
                var cleanPass = _emailConfig.Password.Trim();

                await client.AuthenticateAsync(cleanUser, cleanPass, token);

                // 2. POPRAWKA: Wysyłamy sekwencyjnie (jedna po drugiej) przez to samo otwarte połączenie
                foreach (var message in messages)
                {
                    // Zabezpieczenie przed anulowaniem operacji w trakcie pętli
                    token.ThrowIfCancellationRequested();

                    await client.SendAsync(message, token);
                }

                // Zamykamy bezpiecznie sesję po wysłaniu całej paczki
                await client.DisconnectAsync(true, token);
                _logger.LogInformation("SendMessages: Successfully sent all {Count} messages.", messages.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMessages failed during batch execution");
                throw;
            }
        }


        public MimeMessage CreatedMessage(string to, string subject, string body, string bodyType = "html")
        {
            _logger.LogInformation("CreatedMessage called for to={To} subject={Subject}", to, subject);
            ArgumentNullException.ThrowIfNull(_emailConfig, nameof(_emailConfig));

            var message = new MimeMessage();

            //var fromAddress = new MailboxAddress(System.Text.Encoding.UTF8, _emailConfig.UserName, _emailConfig.From);
            var fromAddress = new MailboxAddress(System.Text.Encoding.UTF8, _emailConfig.From, _emailConfig.UserName);
            var toAddress = new MailboxAddress(System.Text.Encoding.UTF8, to, to);

            message.From.Add(fromAddress);
            message.To.Add(toAddress);
            message.Subject = subject;
            message.Body = new TextPart(bodyType) { Text = body };
            return message;
        }
    }
}