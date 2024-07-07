using Learning.Data;
using Learning.DTO;
using Learning.DTO.ElementDTO;
using Learning.DTO.PagesDTO;
using Learning.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Repository
{
    public class AdminRepository : Repository, IAdminRepository
    {
        public AdminRepository(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> ChangeUserRole(UserRoleDTO userrole)
        {
            var user = mapper.MappUser(userrole);
            if (user == null) { return false; }    
            return await userBusiness.UpdateUser(user.Id, user);
        }

        public async Task<bool> IsAdmin(string authorId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return false; }
            var author = mapper.MappUser(thisuser);
            if (author.Roles.Contains("ADMIN")) { return true; }
            return false;
        }

        public async Task<AdminPageDTO> GetAllUsers(string authorId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var users = await userBusiness.GetUsers();
            var adminpage = mapper.MappAdminPage(users, author);
            return adminpage;
        }

        public async Task<AdminPageDTO> SearchUsers(string authorId, string searchvalue)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var users = await userBusiness.GetUsers();
            var searchedusers = users.Where(u => u.FirstName.ToUpper().Contains(searchvalue.ToUpper()) 
                                              || u.LastName.ToUpper().Contains(searchvalue.ToUpper()) 
                                              || u.NormalizedEmail.Contains(searchvalue.ToUpper())
                                              || u.NormalizedUserName.Contains(searchvalue.ToUpper())).ToList();
            var adminpage = mapper.MappAdminPage(searchedusers, author);
            return adminpage;
        }
    }
}
