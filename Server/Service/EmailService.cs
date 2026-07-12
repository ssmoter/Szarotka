using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

using Server.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Server.Service
{
    public interface IEmailService
    {
        MimeMessage CreatedMessage(string to, string subject, string body, string bodyType = "html");
        void Dispose();
        Task SendMessage(MimeMessage messages, CancellationToken token = default);
        Task SendMessage(string to, string subject, string body, CancellationToken token = default);
        Task SendMessages(IList<MimeMessage> messages, CancellationToken token = default);
    }

    public class EmailService(IConfiguration configuration, ISmtpClient client, ILogger<EmailService>? logger = null) : IDisposable, IEmailService
    {
        private readonly EmailConfiguration? _emailConfig = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
        private readonly SecureSocketOptions _secureSocketOptions = SecureSocketOptions.StartTls;
        private readonly ISmtpClient _client = client;
        private readonly ILogger<EmailService> _logger = logger ?? NullLogger<EmailService>.Instance;

        public async Task SendMessage(string to, string subject, string body, CancellationToken token = default)
        {
            _logger.LogInformation("SendMessage(to) started for to={To} subject={Subject}", to, subject);
            var message = CreatedMessage(to, subject, body);
            await SendMessage(message, token);
            _logger.LogInformation("SendMessage(to) completed for to={To}", to);
        }
        public async Task SendMessage(MimeMessage messages, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(_emailConfig, nameof(_emailConfig));
            _logger.LogInformation("SendMessage(MimeMessage) started for to={To}", string.Join(',', messages.To.Select(m => m.Name)));
            try
            {
                _logger.LogInformation("SendMessage(MimeMessage) connecting to SMTP server {Server}:{Port}", _emailConfig.SmtpServer, _emailConfig.Port);
                _client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, _secureSocketOptions, token);

                _client.Authenticate(_emailConfig.UserName, _emailConfig.Password, token);

                await _client.SendAsync(messages, token);
                await _client.DisconnectAsync(true, token);
                _logger.LogInformation("SendMessage(MimeMessage) sent message to {To}", string.Join(',', messages.To.Select(m => m.Name)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMessage failed");
                throw;
            }
        }

        public async Task SendMessages(IList<MimeMessage> messages, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(_emailConfig, nameof(_emailConfig));
            try
            {
                _logger.LogInformation("SendMessages: connecting to SMTP server {Server}:{Port}", _emailConfig.SmtpServer, _emailConfig.Port);

                _client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, _secureSocketOptions, token);

                _client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                Task[] tasks = new Task[messages.Count];

                for (int i = 0; i < messages.Count; i++)
                {
                    tasks[i] = _client.SendAsync(messages[i], token);
                }

                await Task.WhenAll(tasks);
                await _client.DisconnectAsync(true, token);
                _logger.LogInformation("SendMessages: sent {Count} messages", messages.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMessages failed");
                throw;
            }
        }
        public MimeMessage CreatedMessage(string to, string subject, string body, string bodyType = "html")
        {
            _logger.LogInformation("CreatedMessage called for to={To} subject={Subject}", to, subject);
            ArgumentNullException.ThrowIfNull(_emailConfig, nameof(_emailConfig));

            var message = new MimeMessage();

            var fromAddress = new MailboxAddress(System.Text.Encoding.UTF8, _emailConfig.UserName, _emailConfig.From);
            var toAddress = new MailboxAddress(System.Text.Encoding.UTF8, to, to);

            message.From.Add(fromAddress);
            message.To.Add(toAddress);
            message.Subject = subject;
            message.Body = new TextPart(bodyType) { Text = body };
            return message;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}