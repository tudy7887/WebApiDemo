using Learning.Data;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Learning.BusinessLogic
{
    public class OrderBusinessLogic : BusinessLogic, IOrderBusinessLogic
    {
        public OrderBusinessLogic(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> CreateOrder(Order order)
        {
            return await Create<Order>(order);
        }

        public async Task<bool> DeleteOrder(string orderId)
        {
            var order = await GetOrder(orderId);
            return await Delete<Order>(order);
        }

        public async Task<Order> GetOrder(string orderId)
        {
            return await context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<ICollection<Order>> GetOrders()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<bool> UpdateOrder(string orderId, Order newOrder)
        {
            return await Update<Order>(orderId, newOrder);
        }

        public async Task<string> SetOrderNumber()
        {
            return "order_" + context.Orders.Count();
        }
    }
}
