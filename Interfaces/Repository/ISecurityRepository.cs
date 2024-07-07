using Learning.DTO.PagesDTO;
using Learning.DTO.ElementDTO;
using Microsoft.AspNetCore.Mvc;
using Learning.DTO;

namespace Learning.Interfaces.Repository
{
    public interface ISecurityRepository
    {
        Task<bool> ChangePassword(string authorId, string newpassword);
        Task<bool> IfUserIsMe(BaseDTO author, string userId);
        Task<bool> IsPasswordNew(string authorId, string password);
        Task<bool> UpdateSecurityInfo(string authorId, SecurityInfoPageDTO data);
        Task<bool> DeleteUser(string authorId);
        Task<SecurityInfoPageDTO> GetSecurityInfo(string authorId);
        Task<BaseDTO> GetToken(string authorId);
    }
}
