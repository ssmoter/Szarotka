using DataBase.Model.EntitiesServer;

using Server.HtmlBody;
using HtmlEmail = Server.HtmlBody.Email;
using Server.Model;

namespace Server.Service
{
    public interface IEmailConfirmService
    {
        Task SendResetPasswordEmailCode(User user, CancellationToken token = default);
        Task SendVerificationEmailCode(User user, CancellationToken token = default);
    }

    public class EmailConfirmService : IEmailConfirmService
    {
        private readonly Random _random;
        private readonly IEmailService _emailService;
        private readonly IRegisterUserService _registerService;
        private readonly EmailConfiguration _emailConfig = new();

        public EmailConfirmService(IEmailService emailService, IRegisterUserService registerService, IConfiguration configuration)
        {
            _random = new();
            _emailService = emailService;
            _registerService = registerService;
            var section = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
            if (section is not null)
            {
                _emailConfig = section;
            }
        }



        public async Task SendVerificationEmailCode(User user, CancellationToken token = default)
        {
            var code = _random.Next(9_999, 99_999);

            var saveCode = _registerService.InsertCodeEmailAndRemoveOld(new ConfirmCode(user.Id, code));
            var sendEmail = _emailService.SendMessage(user.Email, "Potwierdź swój email", HtmlEmail.ConfirmEmail(code, new TimeSpan(0, _emailConfig.ExpireDateMinutes, 0)), token);

            await Task.WhenAll(saveCode, sendEmail);
        }
        public async Task SendResetPasswordEmailCode(User user, CancellationToken token = default)
        {
            var code = _random.Next(9_999, 99_999);

            var saveCode = _registerService.InsertCodeEmailAndRemoveOld(new ConfirmCode(user.Id, code));
            var sendEmail = _emailService.SendMessage(user.Email, "Reset hasła", HtmlEmail.ResetPassword(code, new TimeSpan(0, _emailConfig.ExpireDateMinutes, 0)), token);

            await Task.WhenAll(saveCode, sendEmail);
        }

    }
}
