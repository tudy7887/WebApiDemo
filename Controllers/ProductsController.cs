using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning.Interfaces.Repository;
using Learning.Extensions;
using Learning.Repository;
using Microsoft.AspNetCore.Authorization;
using Learning.DTO.PagesDTO;
using Learning.Model;
using System.Buffers;

namespace Learning.Controllers
{
    [Route("Demonstration/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductrRepository productRepository;
        public ProductsController(IProductrRepository repository)
        {
            productRepository = repository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] SingleProducerProductPageDTO product)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isproducer = await productRepository.IsProducer(author.Identity);
            if (!isproducer) { return Forbid("Require Producer Rights!"); }
            var create = await productRepository.CreateProduct(authorId, product);
            if (!create) { return StatusCode(500); }
            return Ok();
        }

        [HttpPut("{ProductId}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(string ProductId, [FromBody] SingleProducerProductPageDTO product)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyproduct = await productRepository.IfMyProduct(authorId, ProductId);
            if (!ismyproduct) { return Forbid(); }
            var create = await productRepository.ModifyProduct(ProductId, product);
            if (!create) { return StatusCode(500); }
            return Ok();
        }

        [HttpDelete("{ProductId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(string ProductId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyproduct = await productRepository.IfMyProduct(authorId, ProductId);
            if (!ismyproduct) { return Forbid(); }
            var create = await productRepository.DeleteProduct(ProductId);
            if (!create) { return StatusCode(500); }
            return Ok();
        }

        [HttpPost("{ProductId}")]
        [Authorize]
        public async Task<IActionResult> AddPictures(string ProductId, [FromBody] ICollection<string> images)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyproduct = await productRepository.IfMyProduct(authorId, ProductId);
            if (!ismyproduct) { return Forbid(); }
            var create = await productRepository.AddPictures(ProductId, images);
            if (!create) { return StatusCode(500); }
            return Ok();
        }

        [HttpDelete("{ProductId}/{ImageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(string ProductId, string ImageId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyproduct = await productRepository.IfMyProduct(authorId, ProductId);
            if (!ismyproduct) { return Forbid(); }
            var create = await productRepository.RemovePicture(ProductId, ImageId);
            if (!create) { return StatusCode(500); }
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var products = await productRepository.GetAllProducts(authorId);
            return Ok(products);
        }

        [HttpGet("Type={Type}")]
        [Authorize]
        public async Task<IActionResult> GetProductsByType(string Type)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var products = await productRepository.GetProductsByType(authorId, Type);
            return Ok(products);
        }

        [HttpGet("User={UserId}")]
        [Authorize]
        public async Task<IActionResult> GetUserProducts(string UserId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await productRepository.IfUserIsMe(author, UserId);
            if (isme)
            {
                var products = await productRepository.GetMyProducts(authorId);
                return Ok(products);
            }
            else
            {
                var products = await productRepository.GetUserProducts(authorId, UserId);
                return Ok(products);
            }
        }

        [HttpGet("User={UserId}/Type={Type}")]
        [Authorize]
        public async Task<IActionResult> GetUserProductsByType(string UserId, string Type)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await productRepository.IfUserIsMe(author, UserId);
            if (isme)
            {
                var products = await productRepository.GetMyProductsByType(authorId, Type);
                return Ok(products);
            }
            else
            {
                var products = await productRepository.GetUserProductsByType(authorId, UserId, Type);
                return Ok(products);
            }
        }

        [HttpGet("Search")]
        [Authorize]
        public async Task<IActionResult> SearchProducts([FromQuery] string searchvalue)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var products = await productRepository.SearchProducts(authorId, searchvalue);
            return Ok(products);
        }

        [HttpGet("Type={Type}/Search")]
        [Authorize]
        public async Task<IActionResult> SearchProductsByType(string Type, [FromQuery] string searchvalue)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var products = await productRepository.SearchProducts(authorId, searchvalue, Type);
            return Ok(products);
        }

        [HttpGet("User={UserId}/Search")]
        [Authorize]
        public async Task<IActionResult> SearchUserProducts(string UserId, [FromQuery] string searchvalue)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await productRepository.IfUserIsMe(author, UserId);
            if (isme)
            {
                var products = await productRepository.SearchMyProducts(authorId, searchvalue);
                return Ok(products);
            }
            else
            {
                var products = await productRepository.SearchUserProducts(authorId, UserId, searchvalue);
                return Ok(products);
            }
        }

        [HttpGet("User={UserId}/Type={Type}/Search")]
        [Authorize]
        public async Task<IActionResult> SearchUserProductsByType(string UserId, string Type, [FromQuery] string searchvalue)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await productRepository.IfUserIsMe(author, UserId);
            if (isme)
            {
                var products = await productRepository.SearchMyProducts(authorId, searchvalue, Type);
                return Ok(products);
            }
            else
            {
                var products = await productRepository.SearchUserProducts(authorId, UserId, searchvalue, Type);
                return Ok(products);
            }
        }

        [HttpGet("{ProductId}")]
        [Authorize]
        public async Task<IActionResult> GetProduct(string ProductId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyproduct = await productRepository.IfMyProduct(authorId, ProductId);
            if (ismyproduct)
            {
                var product = await productRepository.GetProducerProduct(authorId, ProductId);
                return Ok(product);
            }
            else
            {
                var product = await productRepository.GetConsumerProduct(authorId, ProductId);
                return Ok(product);
            }
        }

        [HttpGet("{ProductId}/Orders")]
        [Authorize]
        public async Task<IActionResult> GetProductOrders(string ProductId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var ismyproduct = await productRepository.IfMyProduct(authorId, ProductId);
            if (!ismyproduct) { return Forbid(); }
            var orders = await productRepository.GetProductOrders(authorId, ProductId);
            return Ok(orders);
        }

        [HttpPut("{ProductId}/Cart")]
        [Authorize]
        public async Task<IActionResult> AddOrRemoveProductCart(string ProductId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await productRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var change = await productRepository.AddOrRemoveProductCart(authorId, ProductId);
            if (!change) { return StatusCode(500); }
            return Ok();
        }

    }
}
