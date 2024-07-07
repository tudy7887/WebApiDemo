using Learning.Data;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Learning.BusinessLogic
{
    public class RoleBusinessLogic : BusinessLogic, IRoleBusinessLogic
    {
        public RoleBusinessLogic(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> CreateUserRole(RoleUser userRole)
        {
            context.Users.FirstOrDefaultAsync(u => u.Id == userRole.UserId).Result.RoleUser.Add(userRole);
            return await Save();
        }

        public async Task<bool> DeleteUserRole(string userId, string roleId)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r=>r.Id==roleId);
            var userrole = context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result.RoleUser.FirstOrDefault(ur => ur.Role == role);
            context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result.RoleUser.Remove(userrole);
            return await Save();
        }

        public async Task<ICollection<RoleUser>> GetUserRoles()
        {
            return await context.RoleUser.ToListAsync();
        }

        public async Task<RoleUser> GetUserRole(string userId, string roleId)
        {
            return await context.RoleUser.FirstOrDefaultAsync(u => u.UserId == userId && u.RoleId == roleId);
        }

        public async Task<ICollection<Role>> GetRoles()
        {
            return await context.Roles.ToListAsync();
        }

        public async Task<Role> GetRole(string role)
        {
            return await context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == role.ToUpper()); 
        }
    }
}
