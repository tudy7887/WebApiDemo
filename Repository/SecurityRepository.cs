using Learning.Data;
using Learning.DTO;
using Learning.DTO.PagesDTO;
using Learning.Interfaces.Repository;
using Learning.Model;
using Learning.Services;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Repository
{
    public class SecurityRepository : Repository, ISecurityRepository
    {
        public SecurityRepository(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> IsPasswordNew(string authorId, string newpassword)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return false; }
            var passwords = user.Passwords;
            foreach (var p in passwords)
            {
                if (PasswordCheck(newpassword, p.Salt, p.Hash)) { return false; }
            }
            return true;
        }

        public async Task<bool> ChangePassword(string authorId, string newpassword)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return false; }
            var encription = new PasswordEncription(newpassword);
            user.CurrentHash = encription.GetHash();
            user.CurrentSalt = encription.GetSalt();
            user.Passwords.Add(new Password { Hash = encription.GetHash(), Salt = encription.GetSalt() });
            return await userBusiness.UpdateUser(authorId, user);
        }

        public async Task<bool> DeleteUser(string authorId)
        {
            return await userBusiness.DeleteUser(authorId);
        }

        public async Task<SecurityInfoPageDTO> GetSecurityInfo(string authorId)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return null; }
            var securityinfo = mapper.MappSecurityInfoPageDTO(user);
            return securityinfo;
        }

        public async Task<bool> UpdateSecurityInfo(string authorId, SecurityInfoPageDTO data)
        {
            var user = mapper.MappUser(data);
            if (user == null) { return false; }
            return await userBusiness.UpdateUser(authorId, user);
        }
    }
}
