using Learning.Data;
using Learning.DTO.ElementDTO;
using Learning.DTO.PagesDTO;
using Learning.Interfaces.Repository;
using Learning.Model;
using Learning.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Learning.Repository
{
    public class AccountRepository : Repository, IAccountRepository
    {
        public AccountRepository(DataContext dataContext) : base(dataContext) { }

        public async Task<User> CreateNewUser(RegisterPageDTO data)
        {
            var user = mapper.MappNewUser(data);
            if (user == null) { return null; }
            if (await userBusiness.CreateUser(user))
            {
                var thisuser = GetUserbyUsername(user.UserName);
                return thisuser;
            };
            return null;
        }

        public async Task<User> ForgotPassword(string email)
        {
            var user = GetUserbyEmail(email);
            if (user == null) { return null; }
            return user;
        }

        public async Task<User> ResetPassword(string authorId, string password)
        {
            var user = await userBusiness.GetUser(authorId);
            if(user == null) { return null; }
            var encription = new PasswordEncription(password);
            user.CurrentHash = encription.GetHash();
            user.CurrentSalt = encription.GetSalt();
            user.Passwords.Add(new Password { Hash = encription.GetHash(), Salt = encription.GetSalt() });
            if (await userBusiness.UpdateUser(user.Id, user)) { return null; }
            return user;
        }

        public async Task<bool> IsPasswordNew(string authorId, string password)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return false; }
            var passwords = user.Passwords;
            foreach (var p in passwords)
            {
                if (PasswordCheck(password, p.Salt, p.Hash)) { return false; }
            }
            return true;
        }

        public async Task<User> Login(LoginPageDTO data)
        {
            var user = GetUserbyUsername(data.Username);
            if (user == null) { return null; }
            //return user;
            if (PasswordCheck(data.Password, user.CurrentSalt, user.CurrentHash)) 
            {
                user.Logs.Add(new UserLog() { LogDate = DateTime.Now });
                return user; 
            }
            return null;            
        }
        public async Task<AuthorDTO> GetToken(User user, string token)
        {
            if (user == null) { return null; }
            user.Logs.Add(new UserLog() { LogDate = DateTime.Now });
            var author = mapper.MappUser(user, token);
            return author;
        }

        public async Task<bool> IsUserNew(RegisterPageDTO data)
        {
            if (GetUserbyUsername(data.Username) != null) { return false; }
            if (GetUserbyEmail(data.Email) != null) { return false; }
            return true;
        }
    }
}
