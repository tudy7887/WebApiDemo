using Learning.Model;

namespace Learning.Interfaces.BusinessLogic
{
    public interface IRoleBusinessLogic
    {
        Task<bool> CreateUserRole(RoleUser userRole);
        Task<bool> DeleteUserRole(string userId, string roleId);
        Task<ICollection<RoleUser>> GetUserRoles();
        Task<RoleUser> GetUserRole(string userId, string roleId);
        Task<Role> GetRole(string role);
        Task<ICollection<Role>> GetRoles();
    }
}
