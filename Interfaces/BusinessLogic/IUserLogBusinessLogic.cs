using Learning.Model;

namespace Learning.Interfaces.BusinessLogic
{
    public interface IUserLogBusinessLogic
    {
        Task<bool> CreateUserLog(UserLog userlog);
        Task<UserLog> GetUserLog(string userlogId);
        Task<ICollection<UserLog>> GetUserLogs();
    }
}
