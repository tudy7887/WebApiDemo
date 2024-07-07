using Learning.Data;
using Learning.DTO;
using Learning.DTO.ElementDTO;
using Learning.DTO.PagesDTO;
using Learning.Interfaces.Repository;
using Learning.Model;
using Learning.Services;
using Microsoft.AspNetCore.Mvc;
using System.Buffers;
using static System.Net.Mime.MediaTypeNames;

namespace Learning.Repository
{
    public class ProductRepository : Repository, IProductrRepository
    {
        public ProductRepository(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> AddOrRemoveProductCart(string authorId, string productId)
        {
            var cart = await productBusiness.GetCart(authorId, productId);
            if (cart == null) { productBusiness.CreateCart(authorId, productId); }
            return await productBusiness.DeleteCart(authorId, productId);
        }

        public async Task<bool> AddPictures(string productId, ICollection<string> images)
        {
            var product = await productBusiness.GetProduct(productId);
            foreach (var image in images) 
            {
                product.Pictures.Add(new Model.Image { ImageData = new ImageManipulation().SetImage(image) });
            }
            return await productBusiness.UpdateProduct(productId, product);
        }

        public async Task<bool> IfMyProduct(string authorId, string productId)
        {
            var product = await productBusiness.GetProduct(productId);
            if (product == null) { return false; }
            if (product.User.Id == authorId) { return true; }
            return false;
        }

        public async Task<bool> IsProducer(string authorId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return false; }
            var author = mapper.MappUser(thisuser);
            if (author.Roles.Contains("PRODUCER")) { return true; }
            return false;
        }

        public async Task<OrdersPageDTO> GetProductOrders(string authorId, string productId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var product = await productBusiness.GetProduct(productId);
            var orders = mapper.MappOrderPageDTO(product.ProductOrders, author);
            return orders;
        }

        public async Task<bool> CreateProduct(string authorId, SingleProducerProductPageDTO product)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return false; }
            return await productBusiness.CreateProduct(new Product
            {
                Description = product.Product.Description,
                Name = product.Product.Name,
                Price = product.Product.Price,
                Stock = product.Product.Stock,
                Type = product.Product.Type,
                User = user,
                Pictures = mapper.MappProductImages(product),
            });
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            return await productBusiness.DeleteProduct(productId);
        }     

        public async Task<ProducerProductsPageDTO> GetMyProducts(string authorId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var products = await productBusiness.GetProducts();
            products = FilterProductsByUser(products, authorId);
            var productpage = mapper.MappProducerProductPageDTO(products, author);
            return productpage;
        }

        public async Task<SingleProducerProductPageDTO> GetProducerProduct(string authorId, string productId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var product = await productBusiness.GetProduct(productId);
            var productdto = mapper.MappProducerProduct(product, author);
            return productdto;
        }

        public async Task<SingleConsumerProductPageDTO> GetConsumerProduct(string authorId, string productId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var product = await productBusiness.GetProduct(productId);
            if (product == null) { return null; }
            var productdto = mapper.MappConsumerProduct(product, author);
            return productdto;
        }

        public async Task<ConsumerProductsPageDTO> GetUserProducts(string authorId, string userId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var products = await productBusiness.GetProducts();
            products = FilterProductsByUser(products, userId);
            var productpage = mapper.MappConsumerProductPage(products, author);
            return productpage;
        }

        public async Task<ConsumerProductsPageDTO> SearchProducts(string authorId, string searchvalue, string type = null)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var products = await productBusiness.GetProducts();
            if (type != null) { products = FilterProductsByType(products, type); }
            var searchproducts = products.Where(p => p.Name.ToUpper().Contains(searchvalue.ToUpper())
                                                  || p.Type.ToUpper().Contains(searchvalue.ToUpper())
                                                  || p.Description.ToUpper().Contains(searchvalue.ToUpper())).ToList();
            var productspage = mapper.MappConsumerProductPage(products, author);
            return productspage;
        }

        public async Task<ConsumerProductsPageDTO> SearchUserProducts(string authorId, string userId, string searchvalue, string type = null)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var products = await productBusiness.GetProducts();
            products = FilterProductsByUser(products, userId);
            if (type != null) { products = FilterProductsByType(products, type); }
            var searchproducts = products.Where(p => p.Name.ToUpper().Contains(searchvalue.ToUpper())
                                                  || p.Type.ToUpper().Contains(searchvalue.ToUpper())
                                                  || p.Description.ToUpper().Contains(searchvalue.ToUpper())).ToList();
            var productspage = mapper.MappConsumerProductPage(products, author);
            return productspage;
        }

        public async Task<ConsumerProductsPageDTO> GetUserProductsByType(string authorId, string userId, string type)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var products = await productBusiness.GetProducts();
            products = FilterProductsByUser(products, userId);
            products = FilterProductsByType(products, type);
            var productpage = mapper.MappConsumerProductPage(products, author);
            return productpage;
        }

        public async Task<ConsumerProductsPageDTO> GetProductsByType(string authorId, string type)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var products = await productBusiness.GetProducts();
            products = FilterProductsByType(products, type);
            var productpage = mapper.MappConsumerProductPage(products, author);
            return productpage;
        }

        public async Task<ConsumerProductsPageDTO> GetAllProducts(string authorId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var products = await productBusiness.GetProducts();
            var productpage = mapper.MappConsumerProductPage(products, author);
            return productpage;
        }

        public async Task<ProducerProductsPageDTO> GetMyProductsByType(string authorId, string type)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var products = await productBusiness.GetProducts();
            products = FilterProductsByUser(products, authorId);
            products = FilterProductsByType(products, type);
            var productpage = mapper.MappProducerProductPageDTO(products, author);
            return productpage;
        }

        public async Task<bool> ModifyProduct(string productId, SingleProducerProductPageDTO newdata)
        {
            var newproduct = mapper.MappProduct(newdata);
            return await productBusiness.UpdateProduct(productId, newproduct);
        }

        public async Task<bool> RemovePicture(string productId, string imageId)
        {
            var product = await productBusiness.GetProduct(productId);
            foreach (var picture in product.Pictures)
            {
                if(picture.Id == imageId)
                    product.Pictures.Remove(picture);
            }
            return await productBusiness.UpdateProduct(productId, product);
        }

        public async Task<ProducerProductsPageDTO> SearchMyProducts(string authorId, string searchvalue, string type = null)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var products = await productBusiness.GetProducts();
            products = FilterProductsByUser(products, authorId);
            if (type != null) { products = FilterProductsByType(products, type); }
            var searchproducts = products.Where(p => p.Name.ToUpper().Contains(searchvalue.ToUpper())
                                                  || p.Type.ToUpper().Contains(searchvalue.ToUpper())
                                                  || p.Description.ToUpper().Contains(searchvalue.ToUpper())).ToList();
            var productpage = mapper.MappProducerProductPageDTO(searchproducts, author);
            return productpage;
        }
    }
}
