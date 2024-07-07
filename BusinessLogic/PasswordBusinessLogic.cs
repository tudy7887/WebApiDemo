using Learning.Data;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Learning.BusinessLogic
{
    public class PasswordBusinessLogic : BusinessLogic, IPasswordBusinessLogic
    {
        public PasswordBusinessLogic(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> CreatePassword(Password password)
        {
            return await Create<Password>(password);
        }

        public async Task<Password> GetIPassword(string passwordId)
        {
            return await context.Passwords.FirstOrDefaultAsync(p => p.Id == passwordId);
        }

        public async Task<ICollection<Password>> GetPasswords()
        {
            return await context.Passwords.ToListAsync();
        }
    }
}
