using DataBase.Model.EntitiesServer;
using DataBase.Translated;

namespace DataBaseUnitTest.Translated
{
    public class EnumValidationExtensionTests
    {
        [Theory]
        [InlineData(EnumsList.Validation.PasswordIsNull, "Hasło nie może być puste")]
        [InlineData(EnumsList.Validation.PasswordLength8, "Hasło musi mieć co najmniej 8 znaków")]
        [InlineData(EnumsList.Validation.PasswordNoUpper, "Hasło musi zawierać co najmniej jedną wielką literę")]
        [InlineData(EnumsList.Validation.PasswordNoLower, "Hasło musi zawierać co najmniej małą literę")]
        [InlineData(EnumsList.Validation.PasswordNoDigit, "Hasło musi zawierać co najmniej jedną cyfrę")]
        [InlineData(EnumsList.Validation.PasswordNoSpecial, "Hasło musi zawierać co najmniej jeden znak specjalny")]
        [InlineData(EnumsList.Validation.PasswordContainEmail, "Hasło nie może zawierać adresu email")]
        [InlineData(EnumsList.Validation.RegisterUserNull, "Nie podano danych do rejestracji")]
        [InlineData(EnumsList.Validation.EmailValidFormat, "Niepoprawny format adresu email")]
        [InlineData(EnumsList.Validation.EmailExist, "Adres email jest już zajęty")]
        [InlineData(EnumsList.Validation.CodeIsExpire, "Kod weryfikacyjny wygasł")]
        [InlineData(EnumsList.Validation.CodeNotExist, "Kod weryfikacyjny nie istnie")]
        [InlineData(EnumsList.Validation.LoginIsNull, "Login nie może być pusty")]
        [InlineData(EnumsList.Validation.EmailIsNull, "Email nie może być pusty")]
        [InlineData(EnumsList.Validation.AccountNotFound, "Konto nie istnieje")]
        [InlineData(EnumsList.Validation.AccountWasDelete, "Konto zostało usunięte")]
        [InlineData(EnumsList.Validation.AccountEmailIsNotConfirm, "Email nie został potwierdzony")]
        [InlineData(EnumsList.Validation.NameRequired, "Imię jest wymagane")]
        [InlineData(EnumsList.Validation.PhoneNumberRequired, "Numer telefonu jest wymagany")]
        [InlineData(EnumsList.Validation.PhoneNumberInvalid, "Niepoprawny numer telefonu")]
        public void ValidationToPolish_ShouldReturnCorrectMessage(EnumsList.Validation validation, string expectedMessage)
        {
            // Act
            var result = validation.ValidationToPolish();

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Theory]
        [InlineData(EnumsList.Validation.PasswordIsNull, "", "Hasło nie może być puste")]
        [InlineData(EnumsList.Validation.PasswordIsNull, "Existing message", "Existing message\r\nHasło nie może być puste")]
        public void PasswordsValidation_ShouldReturnCorrectMessage(EnumsList.Validation validation, string initialMessage, string expectedMessage)
        {
            // Act
            var result = validation.PasswordsValidation(initialMessage);

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Theory]
        [InlineData(EnumsList.Validation.EmailValidFormat, "", "Niepoprawny format adresu email")]
        [InlineData(EnumsList.Validation.EmailValidFormat, "Existing message", "Existing message\r\nNiepoprawny format adresu email")]
        public void EmailValidation_ShouldReturnCorrectMessage(EnumsList.Validation validation, string initialMessage, string expectedMessage)
        {
            // Act
            var result = validation.EmailValidation(initialMessage);

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Theory]
        [InlineData(EnumsList.Validation.PhoneNumberRequired, "", "Numer telefonu jest wymagany")]
        [InlineData(EnumsList.Validation.PhoneNumberRequired, "Existing message", "Existing message\r\nNumer telefonu jest wymagany")]
        public void PhoneNumberValidation_ShouldReturnCorrectMessage(EnumsList.Validation validation, string initialMessage, string expectedMessage)
        {
            // Act
            var result = validation.PhoneNumberValidation(initialMessage);

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Theory]
        [InlineData(EnumsList.Validation.NameRequired, "", "Imię jest wymagane")]
        [InlineData(EnumsList.Validation.NameRequired, "Existing message", "Existing message\r\nImię jest wymagane")]
        public void NameValidation_ShouldReturnCorrectMessage(EnumsList.Validation validation, string initialMessage, string expectedMessage)
        {
            // Act
            var result = validation.NameValidation(initialMessage);

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Theory]
        [InlineData(EnumsList.Validation.RegisterUserNull, "", "Nie podano danych do rejestracji")]
        [InlineData(EnumsList.Validation.RegisterUserNull, "Existing message", "Existing message\r\nNie podano danych do rejestracji")]
        [InlineData(EnumsList.Validation.CodeIsExpire, "", "Kod weryfikacyjny wygasł")]
        [InlineData(EnumsList.Validation.CodeIsExpire, "Existing message", "Existing message\r\nKod weryfikacyjny wygasł")]
        [InlineData(EnumsList.Validation.LoginIsNull, "", "Login nie może być pusty")]
        [InlineData(EnumsList.Validation.LoginIsNull, "Existing message", "Existing message\r\nLogin nie może być pusty")]
        [InlineData(EnumsList.Validation.AccountNotFound, "", "Konto nie istnieje")]
        [InlineData(EnumsList.Validation.AccountNotFound, "Existing message", "Existing message\r\nKonto nie istnieje")]
        public void UnclassifiedValidation_ShouldReturnCorrectMessage(EnumsList.Validation validation, string initialMessage, string expectedMessage)
        {
            // Act
            var result = validation.UnclassifiedValidation(initialMessage);

            // Assert
            Assert.Equal(expectedMessage, result);
        }
    }
}
