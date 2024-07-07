using Learning.Data;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Learning.BusinessLogic
{
    public class UserBusinessLogic : BusinessLogic, IUserBusinessLogic
    {
        public UserBusinessLogic(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> CreateUser(User user)
        {
            user.Logs = new List<UserLog>();
            if(user.Adresses == null) { user.Adresses = new List<Adress>(); }
            user.Cart = new List<Cart>();
            user.Products = new List<Product>();
            return await Create<User>(user);
        }

        public async Task<bool> UpdateUser(string userId, User newUser)
        {
            return await Update<User>(userId, newUser);
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var user = await GetUser(userId);
            return await Delete<User>(user);
        }

        public async Task<User> GetUser(string userId)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await context.Users.ToListAsync();
        }
    }
}
