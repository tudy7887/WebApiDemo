using Learning.Data;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Learning.BusinessLogic
{
    public class ProductBusinessLogic : BusinessLogic, IProductBusinessLogic
    {
        public ProductBusinessLogic(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> CreateCart(string userId, string productId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var product = await GetProduct(productId);
            user.Cart.Add(new Cart { Product = product });
            product.Stock--;
            await Update<Product>(productId, product);
            return await Update<User>(userId, user);
        }

        public async Task<bool> CreateProduct(Product product)
        {
            if(product.Pictures == null) { product.Pictures = new List<Model.Image>(); }
            product.ProductOrders = new List<ProductOrder>();
            product.Cart = new List<Cart>();
            return await Create<Product>(product);
        }

        public async Task<bool> DeleteCart(string userId, string productId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var product = await GetProduct(productId);
            var cart = await GetCart(userId, productId);
            user.Cart.Remove(cart);
            product.Stock++;
            await Update<Product>(productId, product);
            return await Update<User>(userId, user);
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            var product = await GetProduct(productId);
            return await Delete<Product>(product);
        }

        public async Task<Product> GetProduct(string productId)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }
        public async Task<Cart> GetCart(string userId, string productId)
        {
            return await context.Cart.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }

        public async Task<ICollection<Product>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<bool> UpdateProduct(string productId, Product newProduct)
        {
            return await Update<Product>(productId, newProduct);
        }

        public async Task<ICollection<Cart>> GetCarts()
        {
            return await context.Cart.ToListAsync();
        }
    }
}
