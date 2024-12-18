using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

using Server.Model;

namespace Server.Service
{
    public interface IEmailService
    {
        MimeMessage CreatedMessage(string to, string subject, string body, string bodyType = "html");
        Task SendMessage(MimeMessage messages);
        Task SendMessage(string to, string subject, string body);
        Task SendMessages(IList<MimeMessage> messages);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration? _emailConfig;
        private readonly SecureSocketOptions _secureSocketOptions = SecureSocketOptions.StartTls;

        public EmailService(IConfiguration configuration)
        {
            _emailConfig = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
        }

        public async Task SendMessage(string to, string subject, string body)
        {
            var message = CreatedMessage(to, subject, body);
            await SendMessage(message);
        }
        public async Task SendMessage(MimeMessage messages)
        {
            if (_emailConfig is null)
            {
                throw new ArgumentNullException(nameof(_emailConfig));
            }

            using var client = new SmtpClient();
            client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, _secureSocketOptions);

            client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

            await client.SendAsync(messages);
            await client.DisconnectAsync(true);
        }

        public async Task SendMessages(IList<MimeMessage> messages)
        {
            if (_emailConfig is null)
            {
                throw new ArgumentNullException(nameof(_emailConfig));
            }

            using var client = new SmtpClient();
            client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, _secureSocketOptions);

            client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

            Task[] tasks = new Task[messages.Count];

            for (int i = 0; i < messages.Count; i++)
            {
                tasks[i] = client.SendAsync(messages[i]);
            }

            await Task.WhenAll(tasks);
            await client.DisconnectAsync(true);
        }
        public MimeMessage CreatedMessage(string to, string subject, string body, string bodyType = "html")
        {
            if (_emailConfig is null)
            {
                throw new ArgumentNullException(nameof(_emailConfig));
            }

            var message = new MimeMessage();

            var fromAddress = new MailboxAddress(System.Text.Encoding.UTF8, _emailConfig.UserName, _emailConfig.From);
            var toAddress = new MailboxAddress(System.Text.Encoding.UTF8, to, to);

            message.From.Add(fromAddress);
            message.To.Add(toAddress);
            message.Subject = subject;
            message.Body = new TextPart(bodyType) { Text = body };
            return message;
        }


    }
}