
using Learning.Model;

namespace Learning.Interfaces.BusinessLogic
{
    public interface IPasswordBusinessLogic
    {
        Task<bool> CreatePassword(Password password);
        Task<Password> GetIPassword(string passwordId);
        Task<ICollection<Password>> GetPasswords();
    }
}
