using Learning.Model;

namespace Learning.Interfaces.BusinessLogic
{
    public interface IProductBusinessLogic
    {
        Task<bool> CreateProduct(Product product);
        Task<bool> UpdateProduct(string productId, Product newProduct);
        Task<bool> DeleteProduct(string productId);
        Task<Product> GetProduct(string productId);
        Task<ICollection<Product>> GetProducts();
        Task<ICollection<Cart>> GetCarts();
        Task<bool> CreateCart(string userId, string productId);
        Task<bool> DeleteCart(string userId, string productId);
        Task<Cart> GetCart(string userId, string productId);
    }
}
