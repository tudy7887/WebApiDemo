using Learning.BusinessLogic;
using Learning.Data;
using Learning.DTO;
using Learning.DTO.ElementDTO;
using Learning.DTO.PagesDTO;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using Learning.Interfaces;
using System.Data;
using System.Security.Principal;

namespace Learning.Services
{
    public class Mapper : IMapper
    {
        private ImageManipulation ImageManipulation;
        private readonly DataContext context;
        private readonly IOrderBusinessLogic orderBusiness;
        private readonly IProductBusinessLogic productBusiness;
        private readonly IUserBusinessLogic userBusiness;
        private readonly IRoleBusinessLogic roleBusiness;
        private readonly IAdressBusinessLogic adressBusiness;

        public Mapper(DataContext dataContext)
        {
            context = dataContext;
            ImageManipulation = new ImageManipulation();
            orderBusiness = new OrderBusinessLogic(context);
            productBusiness = new ProductBusinessLogic(context);
            userBusiness = new UserBusinessLogic(context);
            roleBusiness = new RoleBusinessLogic(context);
            adressBusiness = new AdressBusinessLogic(context);
        }


        // Model
        public Adress MappAdress(SingleAdressPageDTO adress)
        {
            var thisadress = adressBusiness.GetAdress(adress.Adress.Id).Result;
            if (thisadress == null) { return null; }
            thisadress.Number = adress.Adress.Number;
            thisadress.State = adress.Adress.State;
            thisadress.City = adress.Adress.City;
            thisadress.Country = adress.Adress.Country;
            thisadress.Phone = adress.Adress.Phone;
            thisadress.Street = adress.Adress.Street;
            return thisadress;
        }
        public Product MappProduct(SingleProducerProductPageDTO product)
        {
            var thisproduct = productBusiness.GetProduct(product.Product.Id).Result;
            if (thisproduct == null) { return null; }
            thisproduct.Name = product.Product.Name;
            thisproduct.Description = product.Product.Description;
            thisproduct.Price = product.Product.Price;
            thisproduct.Stock = product.Product.Stock;
            thisproduct.Type = product.Product.Type;
            return thisproduct;
        }
        public ICollection<Image> MappProductImages(SingleProducerProductPageDTO product)
        {
            var imagesList = new List<Image>();
            foreach (var p in product.Product.Pictures)
            {
                imagesList.Add(new Image() { ProductId = product.Product.Id, ImageData = ImageManipulation.SetImage(p.Data)});
            }
            return imagesList;
        }
        public ICollection<ProductOrder> MappOrderProducts(SingleOrderPageDTO order)
        {
            var productsList = new List<ProductOrder>();
            foreach (var p in order.Order.Products)
            {
                productsList.Add(new ProductOrder() { OrderId = order.Order.Id, ProductId = p.Id, Status = p.Status});
            }
            return productsList;
        }
        public Order MappOrder(SingleOrderPageDTO order)
        {
            var thisorder = orderBusiness.GetOrder(order.Order.Id).Result;
            if (thisorder == null) { return null; }
            thisorder.DateModified = order.Order.DateModified;
            thisorder.Status = order.Order.Status;
            thisorder.Devivered = order.Order.Delivered;
            thisorder.Total = order.Order.Total;
            return thisorder;
        }
        public User MappUser(SecurityInfoPageDTO usersecurityinfo)
        {
            var user = userBusiness.GetUser(usersecurityinfo.Identity).Result;
            if (user == null) { return null; }
            user.Email = usersecurityinfo.Email;
            user.NormalizedEmail = usersecurityinfo.Email.ToUpper();
            user.UserName = usersecurityinfo.Username;
            user.NormalizedUserName = usersecurityinfo.Username.ToUpper();
            user.TwoFactorEnabled = usersecurityinfo.TwoFactorEnabled;
            user.EmailConfirmed = usersecurityinfo.EmailConfirmed;
            user.PhoneNumber = usersecurityinfo.PhoneNumber;
            user.PhoneNumberConfirmed = usersecurityinfo.PhoneNumberConfirmed;
            return user;
        }
        public User MappUser(UserProfilePageDTO userprofileinfo)
        {
            var user = userBusiness.GetUser(userprofileinfo.Identity).Result;
            if (user == null) { return null; }
            user.FirstName = userprofileinfo.FirstName;
            user.LastName = userprofileinfo.LastName;
            return user;
        }
        public User MappNewUser(RegisterPageDTO data)
        {
            var encription = new PasswordEncription(data.Password);
            return new User
            {
                UserName = data.Username,
                NormalizedUserName = data.Username.ToUpper(),
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                NormalizedEmail = data.Email.ToUpper(),
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                DateCreated = DateTime.Now,
                CurrentSalt = encription.GetSalt(),
                CurrentHash = encription.GetHash(),
                Passwords = new List<Password>() { new Password { Hash = encription.GetHash(), Salt = encription.GetSalt() } },
                RoleUser = new List<RoleUser>()
                {
                    new RoleUser {Role = roleBusiness.GetRole("USER").Result}
                }
            };
        }
        public User MappUser(UserRoleDTO userrole)
        {
            var user = userBusiness.GetUser(userrole.UserId).Result;
            if (user == null) { return null; }
            user.RoleUser = new List<RoleUser>();
            foreach (var role in userrole.UserRoles)
            {
                user.RoleUser.Add(new RoleUser() { Role = roleBusiness.GetRole(role).Result });
            }
            return user;
        }


        // Element DTO
        public SingleAdressPageDTO MappAdress(Adress adress, BaseDTO author)
        {
            return new SingleAdressPageDTO()
            {
                Adress = new AdressDTO()
                {
                    Id = adress.Id,
                    City = adress.City,
                    Country = adress.Country,
                    Number = adress.Number,
                    State = adress.State,
                    Street = adress.Street,
                    Phone = adress.Phone,
                },
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles
            };
        }
        public SingleConsumerProductPageDTO MappConsumerProduct(Product product, BaseDTO author)
        {
            return new SingleConsumerProductPageDTO()
            {
                Product = new ConsumerProductDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Type = product.Type,
                    Description = product.Description,
                    Pictures = MappImages(product.Pictures),
                    Price = product.Price,
                    Stock = product.Stock,
                    ProducerId = product.Id,
                    ProducerFirstName = product.User.FirstName,
                    ProducerLastName = product.User.LastName,
                    ProducerProfilePicture = MappImage(product.User.ProfilePicture),
                },
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles
            };
        }
        public SingleOrderPageDTO MappOrder(Order order, BaseDTO author)
        {
            return new SingleOrderPageDTO()
            {
                Order = new OrderDTO()
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    Cost = order.Cost,
                    DateCreated = order.DateCreated,
                    DateModified = order.DateModified,
                    Status = order.Status,
                    Delivered = order.Devivered,
                    Total = order.Total,
                    Transport = order.Transport,
                    Adress = MappAdress(order.Adress, author).Adress,
                    Products = MappOrderProducts(order.ProductOrders)
                },
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles
            };
        }
        public SingleProducerProductPageDTO MappProducerProduct(Product product, BaseDTO author)
        {
            return new SingleProducerProductPageDTO()
            {
                Product = new ProducerProductDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Type = product.Type,
                    Description = product.Description,
                    Pictures = MappImages(product.Pictures),
                    Price = product.Price,
                    Stock = product.Stock,
                    Orders = MappOrders(product.ProductOrders, author)
                },
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles
            };
        }
        public AuthorDTO MappUser(User user, string token)
        {
            return new AuthorDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = MappImage(user.ProfilePicture),
                Identity = user.Id,
                Token = token,
                Roles = MappRoles(user.RoleUser)
            };
        }
        public BaseDTO MappUser(User user)
        {
            return new AuthorDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = MappImage(user.ProfilePicture),
                Identity = user.Id,
                Roles = MappRoles(user.RoleUser)
            };
        }
        public UserRoleDTO MappUserRole(User user)
        {
            return new UserRoleDTO()
            {
                UserEmail = user.Email,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserRoles = MappRoles(user.RoleUser),
                UserUsername = user.UserName,
                UserId = user.Id
            };
        }



        // Pages DTO
        public AdminPageDTO MappAdminPage(ICollection<User> users, BaseDTO author)
        {
            return new AdminPageDTO()
            {
                UserRoles = MappUserRoles(users),
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles
            };
        }
        public CartPageDTO MappCartPage(ICollection<Cart> cart, BaseDTO author)
        {
            return new CartPageDTO()
            {
                Products = MappConsumerProducts(cart, author),
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles
            };
        }
        public ConsumerProductsPageDTO MappConsumerProductPage(ICollection<Product> products, BaseDTO author)
        {
            return new ConsumerProductsPageDTO()
            {
                Products = MappConsumerProducts(products, author),
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles
            };
        }
        public LoginPageDTO MappLoginPage(string username, string password)
        {
            return new LoginPageDTO()
            {
                Username = username,
                Password = password
            };
        }
        public AdressesPageDTO MappAdressesPageDTO(ICollection<Adress> adresses, BaseDTO author)
        {
            return new AdressesPageDTO()
            {
                Adresses = MappAdresses(adresses, author),
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles,
            };
        }
        public OrdersPageDTO MappOrderPageDTO(ICollection<Order> orders, BaseDTO author)
        {
            return new OrdersPageDTO()
            {
                Orders = MappOrders(orders, author),
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles,
            };
        }
        public OrdersPageDTO MappOrderPageDTO(ICollection<ProductOrder> productorders, BaseDTO author)
        {
            var orders = new List<Order>();
            foreach (var po in productorders)
            {
                orders.Add(po.Order);
            }
            return new OrdersPageDTO()
            {
                Orders = MappOrders(orders, author),
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles,
            };
        }
        public ProducerProductsPageDTO MappProducerProductPageDTO(ICollection<Product> products, BaseDTO author)
        {
            return new ProducerProductsPageDTO()
            {
                Products = MappProducerProducts(products, author),
                Identity = author.Identity,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = author.ProfilePicture,
                Roles = author.Roles
            };
        }
        public RegisterPageDTO MappRegisterPageDTO(string email, string username, string password, string firstname, string lastname)
        {
            return new RegisterPageDTO()
            {
                Email = email,
                Username = username,
                Password = password,
                FirstName = firstname,
                LastName = lastname
            };
        }
        public SecurityInfoPageDTO MappSecurityInfoPageDTO(User author)
        {
            return new SecurityInfoPageDTO()
            {
                Email = author.Email,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePicture = MappImage(author.ProfilePicture),
                PhoneNumber = author.PhoneNumber,
                Roles = MappRoles(author.RoleUser),
                Identity = author.Id,
                TwoFactorEnabled = author.TwoFactorEnabled,
                PhoneNumberConfirmed = author.PhoneNumberConfirmed,
                EmailConfirmed = author.EmailConfirmed,
                Username = author.UserName,
                Logs = MappLogs(author.Logs)
            };
        }
        public UserProfilePageDTO MappUserProfilePageDTO(User author)
        {
            var baseuser = MappBaseUser(author);
            return new UserProfilePageDTO()
            {
                Adresses = MappAdresses(author.Adresses, baseuser),
                DateCreated = author.DateCreated,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Identity = author.Id,
                Roles = MappRoles(author.RoleUser),
                ProfilePicture = MappImage(author.ProfilePicture),
                Orders = MappOrders(author.Adresses, baseuser)
            };
        }



        // Private Methods
        private ICollection<AdressDTO> MappAdresses(ICollection<Adress> adresses, BaseDTO author)
        {
            var adressesList = new List<AdressDTO>();
            foreach (var adress in adresses)
            {
                adressesList.Add(MappAdress(adress, author).Adress);
            }
            return adressesList;
        }
        private ICollection<UserRoleDTO> MappUserRoles(ICollection<User> userroles)
        {
            var userrolesList = new List<UserRoleDTO>();
            foreach (var useerrole in userroles)
            {
                userrolesList.Add(MappUserRole(useerrole));
            }
            return userrolesList;
        }
        private ICollection<ConsumerProductDTO> MappConsumerProducts(ICollection<Product> products, BaseDTO author)
        {
            var productsList = new List<ConsumerProductDTO>();
            foreach (var product in products)
            {
                productsList.Add(MappConsumerProduct(product, author).Product);
            }
            return productsList;
        }
        private ICollection<ConsumerProductDTO> MappConsumerProducts(ICollection<Cart> cart, BaseDTO author)
        {
            var productsList = new List<ConsumerProductDTO>();
            foreach (var product in cart)
            {
                productsList.Add(MappConsumerProduct(product.Product, author).Product);
            }
            return productsList;
        }
        private ICollection<ConsumerProductDTO> MappConsumerProducts(ICollection<ProductOrder> productorders, BaseDTO author)
        {
            var productsList = new List<ConsumerProductDTO>();
            foreach (var productorder in productorders)
            {
                productsList.Add(MappConsumerProduct(productorder.Product, author).Product);
            }
            return productsList;
        }
        private ICollection<OrderProductDTO> MappOrderProducts(ICollection<ProductOrder> productorders)
        {
            var productsList = new List<OrderProductDTO>();
            foreach (var productorder in productorders)
            {
                productsList.Add(MappOrderProduct(productorder.Product, productorder.Status));
            }
            return productsList;
        }
        private OrderProductDTO MappOrderProduct(Product product, string status)
        {
            return new OrderProductDTO()
            {

                Id = product.Id,
                Name = product.Name,
                Type = product.Type,
                Description = product.Description,
                Pictures = MappImages(product.Pictures),
                Price = product.Price,
                Stock = product.Stock,
                ProducerId = product.Id,
                ProducerFirstName = product.User.FirstName,
                ProducerLastName = product.User.LastName,
                ProducerProfilePicture = MappImage(product.User.ProfilePicture),
                Status = status
            };
        }
        private ICollection<OrderDTO> MappOrders(ICollection<Order> orders, BaseDTO author)
        {
            var ordersList = new List<OrderDTO>();
            foreach (var order in orders)
            {
                ordersList.Add(MappOrder(order, author).Order);
            }
            return ordersList;
        }
        private ICollection<OrderDTO> MappOrders(ICollection<Adress> adresses, BaseDTO author)
        {
            var ordersList = new List<OrderDTO>();
            foreach (var adress in adresses)
            {
                foreach (var order in adress.Orders)
                {
                    ordersList.Add(MappOrder(order, author).Order);
                }
            }
            return ordersList;
        }
        private ICollection<OrderDTO> MappOrders(ICollection<ProductOrder> productorders, BaseDTO author)
        {
            var ordersList = new List<OrderDTO>();
            foreach (var productorder in productorders)
            {
                ordersList.Add(MappOrder(productorder.Order, author).Order);
            }
            return ordersList;
        }
        private ICollection<ProducerProductDTO> MappProducerProducts(ICollection<Product> products, BaseDTO author)
        {
            var productsList = new List<ProducerProductDTO>();
            foreach (var product in products)
            {
                productsList.Add(MappProducerProduct(product, author).Product);
            }
            return productsList;
        }
        private ICollection<string> MappRoles(ICollection<RoleUser> userroles)
        {
            var userrolesList = new List<string>();
            foreach (var userrole in userroles)
            {
                userrolesList.Add(userrole.Role.NormalizedName);
            }
            return userrolesList;
        }
        private ICollection<DateTime> MappLogs(ICollection<UserLog> logs)
        {
            var logsList = new List<DateTime>();
            foreach (var log in logs)
            {
                logsList.Add(log.LogDate);
            }
            return logsList;
        }
        private ICollection<ProductImageDTO> MappImages(ICollection<Image> images)
        {
            var imagesList = new List<ProductImageDTO>();
            foreach (var image in images)
            {
                imagesList.Add(new ProductImageDTO() { Id = image.Id, Data = MappImage(image) });
            }
            return imagesList;
        }
        private string MappImage(Image image)
        {
            if(image == null) { return null; }
            return ImageManipulation.GetImage(image.ImageData);
        }
        private BaseDTO MappBaseUser(User user)
        {
            return new BaseDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = MappImage(user.ProfilePicture),
                Identity = user.Id,
                Roles = MappRoles(user.RoleUser)
            };
        }
    }
}
