using Learning.DTO.PagesDTO;
using Learning.DTO.ElementDTO;
using Microsoft.AspNetCore.Mvc;
using Learning.DTO;

namespace Learning.Interfaces.Repository
{
    public interface IProductrRepository
    {
        Task<bool> CreateProduct(string authorId, SingleProducerProductPageDTO product);
        Task<bool> ModifyProduct(string productId, SingleProducerProductPageDTO newdata);
        Task<bool> AddPictures(string productId, ICollection<string> images);
        Task<bool> RemovePicture(string productId, string imageId);
        Task<bool> DeleteProduct(string productId);
        Task<ProducerProductsPageDTO> GetMyProductsByType(string authorId, string type);
        Task<ProducerProductsPageDTO> GetMyProducts(string authorId);
        Task<ProducerProductsPageDTO> SearchMyProducts(string authorId, string searchvalue, string type = null);
        Task<SingleProducerProductPageDTO> GetProducerProduct(string authorId, string productId);
        Task<bool> AddOrRemoveProductCart(string authorId, string productId);
        Task<ConsumerProductsPageDTO> GetAllProducts(string authorId);
        Task<ConsumerProductsPageDTO> GetProductsByType(string authorId, string type);
        Task<ConsumerProductsPageDTO> SearchProducts(string authorId, string searchvalue, string type = null);
        Task<ConsumerProductsPageDTO> GetUserProducts(string authorId, string userId);
        Task<ConsumerProductsPageDTO> GetUserProductsByType(string authorId, string userId, string type);
        Task<ConsumerProductsPageDTO> SearchUserProducts(string authorId, string userId, string searchvalue, string type = null);
        Task<SingleConsumerProductPageDTO> GetConsumerProduct(string authorId, string productId);
        Task<OrdersPageDTO> GetProductOrders(string authorId, string productId);
        Task<bool> IfMyProduct(string authorId, string productId);
        Task<bool> IsProducer(string authorId);
        Task<BaseDTO> GetToken(string authorId);
        Task<bool> IfUserIsMe(BaseDTO author, string userId);
    }
}
