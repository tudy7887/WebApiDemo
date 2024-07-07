using Learning.Interfaces;
using Learning.BusinessLogic;
using Learning.Data;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Learning.DTO.PagesDTO;
using Learning.DTO;
using Learning.Services;

namespace Learning.Repository
{
    public class Repository
    {
        protected readonly DataContext context;
        protected readonly IMapper mapper;
        protected readonly IAdressBusinessLogic adressBusiness;
        protected readonly IImageBusinessLogic imageBusiness;
        protected readonly IOrderBusinessLogic orderBusiness;
        protected readonly IPasswordBusinessLogic passwordBusiness;
        protected readonly IProductBusinessLogic productBusiness;
        protected readonly IRoleBusinessLogic roleBusiness;
        protected readonly IUserBusinessLogic userBusiness;
        protected readonly IUserLogBusinessLogic userLogBusiness;
        public Repository(DataContext dataContext) 
        {
            context = dataContext;
            mapper = new Mapper(context);
            adressBusiness = new AdressBusinessLogic(context);
            imageBusiness = new ImageBusinessLogic(context);
            orderBusiness = new OrderBusinessLogic(context);
            passwordBusiness = new PasswordBusinessLogic(context);
            productBusiness = new ProductBusinessLogic(context);
            roleBusiness = new RoleBusinessLogic(context);
            userBusiness = new UserBusinessLogic(context);
            userLogBusiness = new UserLogBusinessLogic(context);
        }

        public async Task<BaseDTO> GetToken(string userId)
        {
            var user = await userBusiness.GetUser(userId);
            if (user == null) { return null; }
            var author = mapper.MappUser(user);
            return author;
        }

        public async Task<bool> IfUserIsMe(BaseDTO author, string userId)
        {
            if (author.Identity == userId) { return true; }
            return false;
        }

        protected User GetUserbyUsername(string username)
        {
            var users = userBusiness.GetUsers().Result;
            var user = users.FirstOrDefault(u => u.UserName == username);
            return user;
        }

        protected bool PasswordCheck(string password, byte[] salt, byte[] hash)
        {
            var encription = new PasswordEncription(password, salt);
            return hash.SequenceEqual(encription.GetHash());
        }
        protected User GetUserbyEmail(string email)
        {
            var users = userBusiness.GetUsers().Result;
            var user = users.FirstOrDefault(u => u.NormalizedEmail == email.ToUpper());
            return user;
        }

        protected ICollection<Product> FilterProductsByType(ICollection<Product> products, string type)
        {
            var result = products.Where(p => p.Type.ToUpper().Contains(type.ToUpper())).ToList();
            return result;
        }
        protected ICollection<Product> FilterProductsByUser(ICollection<Product> products, string userId)
        {
            var result = products.Where(p => p.User.Id == userId).ToList();
            return result;
        }
        protected ICollection<Order> FilterOrdersByUser(ICollection<Order> orders, string userId)
        {
            var result = orders.Where(o => o.Adress.User.Id == userId).ToList();
            return result;
        }

    }
}
