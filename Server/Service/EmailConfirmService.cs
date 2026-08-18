using DataBase.Model.EntitiesServer;

using Microsoft.Extensions.Logging.Abstractions;

using Server.Helper;
using Server.Model;

using HtmlEmail = Server.HtmlBody.Email;

namespace Server.Service
{
    public interface IEmailConfirmService
    {
        Task SendResetPasswordEmailCode(User user, CancellationToken token = default);
        Task<ConfirmCode> SendVerificationEmailCode(User user, CancellationToken token = default);
    }

    public class EmailConfirmService : IEmailConfirmService
    {
        private readonly Random _random;
        private readonly IEmailService _emailService;
        private readonly IRegisterUserService _registerService;
        private readonly EmailConfiguration _emailConfig = new();
        private readonly ILogger<EmailConfirmService> _logger;

        public EmailConfirmService(IEmailService emailService, IRegisterUserService registerService, IConfiguration configuration, ILogger<EmailConfirmService>? logger = null)
        {
            _random = new();
            _emailService = emailService;
            _registerService = registerService;
            _logger = logger ?? NullLogger<EmailConfirmService>.Instance;
            var section = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
            if (section is not null)
            {
                _emailConfig = section;
            }
        }



        public async Task<ConfirmCode> SendVerificationEmailCode(User user, CancellationToken token = default)
        {
            _logger.LogInformation("SendVerificationEmailCode started for userId={UserId}", user.Id);
            var code = _random.Next(9_999, 99_999);

            var saveCode = _registerService.CreatedConfirmCode(new ConfirmCode(user.Id, code));
            var sendEmail = _emailService.SendMessage(user.Email, "Potwierdź swój email", HtmlEmail.ConfirmEmail(code, new TimeSpan(0, _emailConfig.ExpireDateMinutes, 0)), token);

            try
            {
                await sendEmail;
                _logger.LogInformation("SendVerificationEmailCode succeeded for userId={UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendVerificationEmailCode failed for userId={UserId}", user.Id);
                throw;
            }
            return saveCode;
        }
        public async Task SendResetPasswordEmailCode(User user, CancellationToken token = default)
        {
            _logger.LogInformation("SendResetPasswordEmailCode started for userId={UserId}", user.Id);
            var code = _random.Next(9_999, 99_999);

            var confirmCode = _registerService.CreatedConfirmCode(new ConfirmCode(user.Id, code));
            var sendEmail = _emailService.SendMessage(user.Email, "Reset hasła", HtmlEmail.ResetPassword(code, new TimeSpan(0, _emailConfig.ExpireDateMinutes, 0)), token);

            try
            {
                await sendEmail;
                DictionaryList.ResetPasswordCodes.TryAdd(code, confirmCode);
                _logger.LogInformation("SendResetPasswordEmailCode succeeded for userId={UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendResetPasswordEmailCode failed for userId={UserId}", user.Id);
                throw;
            }
        }

    }
}
