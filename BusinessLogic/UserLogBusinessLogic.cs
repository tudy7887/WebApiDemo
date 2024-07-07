using Learning.Data;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Learning.BusinessLogic
{
    public class UserLogBusinessLogic : BusinessLogic, IUserLogBusinessLogic
    {
        public UserLogBusinessLogic(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> CreateUserLog(UserLog userlog)
        {
            return await Create<UserLog>(userlog);
        }

        public async Task<UserLog> GetUserLog(string userlogId)
        {
            return await context.UserLogs.FirstOrDefaultAsync(ul => ul.Id == userlogId);
        }

        public async Task<ICollection<UserLog>> GetUserLogs()
        {
            return await context.UserLogs.ToListAsync();
        }
    }
}
