using Learning.DTO.ElementDTO;
using Learning.DTO.PagesDTO;
using Learning.DTO;
using Learning.Model;

namespace Learning.Interfaces
{
    public interface IMapper
    {
        // Modell
        Adress MappAdress(SingleAdressPageDTO adress);
        Product MappProduct(SingleProducerProductPageDTO product);
        ICollection<Image> MappProductImages(SingleProducerProductPageDTO product);
        ICollection<ProductOrder> MappOrderProducts(SingleOrderPageDTO order);
        Order MappOrder(SingleOrderPageDTO order);
        User MappUser(SecurityInfoPageDTO usersecurityinfo);
        User MappUser(UserProfilePageDTO userprofileinfo);
        User MappNewUser(RegisterPageDTO data);
        User MappUser(UserRoleDTO userrole);

        // Element DTO
        SingleAdressPageDTO MappAdress(Adress adress, BaseDTO author);
        SingleConsumerProductPageDTO MappConsumerProduct(Product product, BaseDTO author);
        SingleOrderPageDTO MappOrder(Order order, BaseDTO author);
        SingleProducerProductPageDTO MappProducerProduct(Product product, BaseDTO author);
        AuthorDTO MappUser(User user, string token);
        BaseDTO MappUser(User user);
        UserRoleDTO MappUserRole(User user);

        // Pages DTO
        AdminPageDTO MappAdminPage(ICollection<User> users, BaseDTO author);
        CartPageDTO MappCartPage(ICollection<Cart> cart, BaseDTO author);
        ConsumerProductsPageDTO MappConsumerProductPage(ICollection<Product> products, BaseDTO author);
        LoginPageDTO MappLoginPage(string username, string password);
        AdressesPageDTO MappAdressesPageDTO(ICollection<Adress> adresses, BaseDTO author);
        OrdersPageDTO MappOrderPageDTO(ICollection<Order> orders, BaseDTO author);
        OrdersPageDTO MappOrderPageDTO(ICollection<ProductOrder> productorders, BaseDTO author);
        ProducerProductsPageDTO MappProducerProductPageDTO(ICollection<Product> products, BaseDTO author);
        RegisterPageDTO MappRegisterPageDTO(string email, string username, string password, string firstname, string lastname);
        SecurityInfoPageDTO MappSecurityInfoPageDTO(User author);
        UserProfilePageDTO MappUserProfilePageDTO(User author);
    }
}
