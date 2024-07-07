using Learning.DTO.PagesDTO;
using Learning.DTO.ElementDTO;
using Microsoft.AspNetCore.Mvc;
using Learning.Model;
using Learning.DTO;

namespace Learning.Interfaces.Repository
{
    public interface IAccountRepository
    {
        Task<bool> IsUserNew(RegisterPageDTO data);
        Task<User> CreateNewUser(RegisterPageDTO data);
        Task<User> Login(LoginPageDTO data);
        Task<AuthorDTO> GetToken(User user, string token);
        Task<User> ForgotPassword(string email);
        Task<User> ResetPassword(string authorId, string password);
        Task<bool> IsPasswordNew(string authorId, string password);
        Task<BaseDTO> GetToken(string authorId);
        Task<bool> IfUserIsMe(BaseDTO author, string userId);
    }
}
