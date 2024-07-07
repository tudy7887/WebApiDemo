using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning.Interfaces.Repository;
using Learning.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Learning.Extensions;
using Learning.DTO.ElementDTO;

namespace Learning.Controllers
{
    [Route("Demonstration/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;
        public AdminController(IAdminRepository repository)
        {
            adminRepository = repository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await adminRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isadmin = await adminRepository.IsAdmin(author.Identity);
            if(!isadmin) { return Forbid("Require Admin Rights!"); }
            var adminpage = await adminRepository.GetAllUsers(author.Identity);
            return Ok(adminpage);
        }

        [HttpGet]
        [Authorize]
        [Route("Search")]

        public async Task<IActionResult> Search([FromQuery] string searchvalue)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await adminRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isadmin = await adminRepository.IsAdmin(author.Identity);
            if (!isadmin) { return Forbid("Require Admin Rights!"); }
            var adminpage = await adminRepository.SearchUsers(author.Identity, searchvalue);
            return Ok(adminpage);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserRoleDTO userrole)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await adminRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isadmin = await adminRepository.IsAdmin(author.Identity);
            if (!isadmin) { return Forbid("Require Admin Rights!"); }
            var change = await adminRepository.ChangeUserRole(userrole);
            if (!change) { return StatusCode(500); }
            return Ok();
        }
    }
}
