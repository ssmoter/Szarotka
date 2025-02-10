using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

using Server.Model;

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

    public class EmailService : IDisposable, IEmailService
    {
        private readonly EmailConfiguration? _emailConfig;
        private readonly SecureSocketOptions _secureSocketOptions = SecureSocketOptions.StartTls;
        private readonly ISmtpClient _client;

        public EmailService(IConfiguration configuration, ISmtpClient client)
        {
            _emailConfig = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
            _client = client;
        }

        public async Task SendMessage(string to, string subject, string body, CancellationToken token = default)
        {
            var message = CreatedMessage(to, subject, body);
            await SendMessage(message, token);
        }
        public async Task SendMessage(MimeMessage messages, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(_emailConfig, nameof(_emailConfig));

            _client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, _secureSocketOptions, token);

            _client.Authenticate(_emailConfig.UserName, _emailConfig.Password, token);

            await _client.SendAsync(messages, token);
            await _client.DisconnectAsync(true, token);
        }

        public async Task SendMessages(IList<MimeMessage> messages, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(_emailConfig, nameof(_emailConfig));

            _client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, _secureSocketOptions, token);

            _client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

            Task[] tasks = new Task[messages.Count];

            for (int i = 0; i < messages.Count; i++)
            {
                tasks[i] = _client.SendAsync(messages[i], token);
            }

            await Task.WhenAll(tasks);
            await _client.DisconnectAsync(true, token);
        }
        public MimeMessage CreatedMessage(string to, string subject, string body, string bodyType = "html")
        {
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