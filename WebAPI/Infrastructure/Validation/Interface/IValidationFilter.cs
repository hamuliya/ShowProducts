namespace WebAPI.Infrastructure.Validation.Interface
{
    public interface IValidationFilter
    {
        bool PasswordValidator(string password);
        bool UserNameValidator(string userName);
    }
}