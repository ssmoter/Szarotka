using DataBase.Model.EntitiesServer;

using Server.HtmlBody;

namespace Server.Service
{
    public interface IEmailConfirmService
    {
        Task SendVerificationEmailCode(User user);
    }

    public class EmailConfirmService : IEmailConfirmService
    {
        private readonly Random _random;
        private readonly IEmailService _emailService;
        private readonly IRegisterUserService _registerService;

        public EmailConfirmService(IEmailService emailService, IRegisterUserService registerService)
        {
            _random = new();
            _emailService = emailService;
            _registerService = registerService;
        }



        public async Task SendVerificationEmailCode(User user)
        {
            var code = _random.Next(9_999, 99_999);

            var saveCode = _registerService.InsertCodeEmailAndRemoveOld(new RegisterConfirmEmailUser(user.Id, code));
            var sendEmail = _emailService.SendMessage(user.Email, "Potwierdź swój email", Email.ConfirmEmail(code));

            await Task.WhenAll(saveCode, sendEmail);
        }
    }
}
