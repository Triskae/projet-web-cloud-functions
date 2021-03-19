using ProjetWeb.Models;

namespace ProjetWeb.Auth
{
    public interface IPasswordProvider
    {
        PasswordAndSalt GenerateNewSaltedPassword(string clearPassword);
        bool IsValidPassword(string supposedClearHistoricPassword, string dbSalt, string dbHashedPassword);
    }
}
