using Learning.Model;

namespace Learning.Interfaces.BusinessLogic
{
    public interface IUserBusinessLogic
    {
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(string userId, User newUser);
        Task<bool> DeleteUser(string userId);
        Task<User> GetUser(string userId);
        Task<ICollection<User>> GetUsers();
    }
}
