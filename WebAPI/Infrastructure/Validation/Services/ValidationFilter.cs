using WebAPI.Infrastructure.Validation.Interface;

namespace WebAPI.Infrastructure.Validation.Services
{
    public class ValidationFilter : IValidationFilter
    {
        public ValidationFilter()
        {
        }

        public bool PasswordValidator(string password)
        {

            bool result = false;

            //Check if the password length is between 8 and 50
            if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 50)
                return result;

            // Check if the password includes at least one capital letter
            bool hasCapitalLetter = false;
            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasCapitalLetter = true;
                    break;
                }
            }


            if (!hasCapitalLetter)
            {
                Console.WriteLine("Password must include at least one capital letter");
                return result;
            }

            // Check if the password includes at least one special character
            bool hasSpecialCharacter = false;
            foreach (char c in password)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    hasSpecialCharacter = true;
                    break;
                }
            }

            if (!hasSpecialCharacter)
            {
                Console.WriteLine("Password must include at least one special character");
                return result;
            }



            result = true;


            return result;

        }

        public bool UserNameValidator(string userName)
        {

            bool result = false;
            if (string.IsNullOrEmpty(userName) || userName.Length < 3 || userName.Length > 50)
            {
                return result;
            }
            result = true;
            return result;

        }



    }
}
