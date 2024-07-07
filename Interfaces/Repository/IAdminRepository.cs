using Learning.DTO.PagesDTO;
using Learning.DTO.ElementDTO;
using Microsoft.AspNetCore.Mvc;
using Learning.DTO;

namespace Learning.Interfaces.Repository
{
    public interface IAdminRepository
    {
        Task<bool> IsAdmin(string authorId);
        Task<bool> ChangeUserRole(UserRoleDTO userrole);
        Task<AdminPageDTO> GetAllUsers(string authorId);
        Task<AdminPageDTO> SearchUsers(string authorId, string searchvalue);
        Task<BaseDTO> GetToken(string authorId);
        Task<bool> IfUserIsMe(BaseDTO author, string userId);
    }
}
