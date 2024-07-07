using Learning.DTO.PagesDTO;
using Learning.DTO.ElementDTO;
using Microsoft.AspNetCore.Mvc;
using Learning.DTO;

namespace Learning.Interfaces.Repository
{
    public interface IOrderRepository
    {
        Task<bool> CreateOrder(string authorId, SingleOrderPageDTO order);
        Task<bool> ModifyOrder(string authorId, SingleOrderPageDTO newdata);
        Task<bool> CancelOrder(string authorId, string orderId);
        Task<OrdersPageDTO> GetUserOrders(string authorId);
        Task<SingleOrderPageDTO> GetOrder(string authorId, string orderId);
        Task<bool> ChangeOrderProductStatus(string orderId, string productId, string status);
        Task<bool> IfMyOrder(string authorId, string orderId);
        Task<bool> IfMyAdress(string authorId, AdressDTO adress);
        Task<BaseDTO> GetToken(string authorId);
        Task<bool> IfUserIsMe(BaseDTO author, string userId);
        Task<bool> IfStatusValid(string status);
        Task<bool> IfMyProduct(string authorId, string productId);
    }
}
