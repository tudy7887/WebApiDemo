using Learning.Model;

namespace Learning.Interfaces.BusinessLogic
{
    public interface IOrderBusinessLogic
    {
        Task<bool> CreateOrder(Order order);
        Task<bool> UpdateOrder(string orderId, Order newOrder);
        Task<bool> DeleteOrder(string orderId);
        Task<Order> GetOrder(string orderId);
        Task<ICollection<Order>> GetOrders();
        Task<string> SetOrderNumber();
    }
}
