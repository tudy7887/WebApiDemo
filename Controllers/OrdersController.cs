using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning.Interfaces.Repository;
using Learning.Repository;
using Learning.DTO.PagesDTO;
using Learning.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Learning.Controllers
{
    [Route("Demonstration/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        public OrdersController(IOrderRepository repository)
        {
            orderRepository = repository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] SingleOrderPageDTO order)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await orderRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyadress = await orderRepository.IfMyAdress(authorId, order.Order.Adress);
            if (!ismyadress) { return Forbid(); }
            var create = await orderRepository.CreateOrder(authorId, order);
            if (!create) { return StatusCode(500); }
            return Ok();
        }

        [HttpPut("{OrderId}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(string OrderId, [FromBody] SingleOrderPageDTO order)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await orderRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyorder = await orderRepository.IfMyOrder(authorId, OrderId);
            if (!ismyorder) { return Forbid(); }
            var change = await orderRepository.ModifyOrder(OrderId, order);
            if (!change) { return StatusCode(500); }
            return Ok();
        }

        [HttpPut("{OrderId}/Cancel")]
        [Authorize]
        public async Task<IActionResult> CancelOrder(string OrderId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await orderRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyorder = await orderRepository.IfMyOrder(authorId, OrderId);
            if (!ismyorder) { return Forbid(); }
            var delete = await orderRepository.CancelOrder(authorId, OrderId);
            if (!delete) { return StatusCode(500); }
            return Ok();
        }

        [HttpPut("{OrderId}/{ProductId}")]
        [Authorize]
        public async Task<IActionResult> ChangeProductStatus(string OrderId, string ProductId, [FromBody] string status)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await orderRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyproduct = await orderRepository.IfMyProduct(authorId, ProductId);
            if (!ismyproduct) { return Forbid(); }
            var validstatus = await orderRepository.IfStatusValid(status);
            if (!validstatus) { return BadRequest(); }
            var change = await orderRepository.ChangeOrderProductStatus(OrderId, ProductId,status);
            if (!change) { return StatusCode(500); }
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllOrders()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await orderRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var orders = await orderRepository.GetUserOrders(authorId);
            return Ok(orders);
        }

        [HttpGet("{OrderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrder(string OrderId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await orderRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyorder = await orderRepository.IfMyOrder(authorId, OrderId);
            if (!ismyorder) { return Forbid(); }
            var order = await orderRepository.GetOrder(authorId, OrderId);
            return Ok(order);
        }

    }
}
