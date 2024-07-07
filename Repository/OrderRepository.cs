using Learning.Data;
using Learning.DTO;
using Learning.DTO.ElementDTO;
using Learning.DTO.PagesDTO;
using Learning.Interfaces.Repository;
using Learning.Model;
using Microsoft.AspNetCore.Mvc;
using System.Buffers;

namespace Learning.Repository
{
    public class OrderRepository : Repository, IOrderRepository
    {
        public OrderRepository(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> ChangeOrderProductStatus(string orderId, string productId, string status)
        {
            var order = await orderBusiness.GetOrder(orderId);
            if (order == null) { return false; }
            var productorder = order.ProductOrders.FirstOrDefault(po => po.ProductId == productId);
            if (productorder == null) { return false; }
            order.ProductOrders.FirstOrDefault(po => po.ProductId == productId).Status = status;
            if (status == "Delivered")
            {
                order.Devivered++;
                if (order.Devivered == order.Total) { order.Status = "Delivered"; }
            }
            return await orderBusiness.UpdateOrder(orderId, order);
        }

        public async Task<bool> IfMyOrder(string authorId, string orderId)
        {
            var order = await orderBusiness.GetOrder(orderId);
            if (order == null) { return false; }
            if (order.Adress.User.Id == authorId) { return true; }
            return false;
        }

        public async Task<bool> IfMyAdress(string authorId, AdressDTO adress)
        {
            var thisadress = await adressBusiness.GetAdress(adress.Id);
            if (thisadress == null) { return false; }
            if (thisadress.User.Id == authorId) { return true; }
            return false;
        }
        public async Task<bool> CancelOrder(string authorId, string orderId)
        {
            var order = await orderBusiness.GetOrder(orderId);
            if (order == null) { return false; }
            order.Status = "Canceled";
            return await orderBusiness.UpdateOrder(orderId, order);
        }

        public async Task<bool> CreateOrder(string authorId, SingleOrderPageDTO order)
        {
            foreach(var p in order.Order.Products)
            {
                p.Status = "Requested";
            }
            var adress = await adressBusiness.GetAdress(order.Order.Adress.Id);
            return await orderBusiness.CreateOrder(new Order()
            {
                Cost = order.Order.Cost,
                DateCreated = order.Order.DateCreated,
                DateModified = order.Order.DateModified,
                OrderNumber = await orderBusiness.SetOrderNumber(),
                Status = "Requested",
                Devivered = 0,
                Total = order.Order.Products.Count,
                Transport = order.Order.Transport,
                Adress = adress,
                ProductOrders = mapper.MappOrderProducts(order)
            });
        }

        public async Task<SingleOrderPageDTO> GetOrder(string authorId, string orderId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var order = await orderBusiness.GetOrder(orderId);
            if (order == null) { return null; }
            var orderdto = mapper.MappOrder(order, author);
            return orderdto;
        }

        

        public async Task<OrdersPageDTO> GetUserOrders(string authorId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var orders = await orderBusiness.GetOrders();
            orders = FilterOrdersByUser(orders, authorId);
            var orderpage = mapper.MappOrderPageDTO(orders, author);
            return orderpage;
        }

        public async Task<bool> ModifyOrder(string authorId, SingleOrderPageDTO newdata)
        {
            var order = mapper.MappOrder(newdata);
            if (order == null) { return false; }
            return await orderBusiness.UpdateOrder(newdata.Order.Id, order);
        }

        public async Task<bool> IfMyProduct(string authorId, string productId)
        {
            var product = await productBusiness.GetProduct(productId);
            if (product == null) { return false; }
            if (product.User.Id == authorId) { return true; }
            return false;
        }

        public async Task<bool> IfStatusValid(string status)
        {
            if (status == "Requested" ||
                status == "Processing" ||
                status == "Delivering" ||
                status == "Delivered") { return true; }
            return false;
        }
    }
}
