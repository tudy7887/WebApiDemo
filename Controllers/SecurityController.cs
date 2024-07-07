using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning.Interfaces.Repository;
using Learning.Extensions;
using Learning.Repository;
using Microsoft.AspNetCore.Authorization;
using Learning.DTO.PagesDTO;

namespace Learning.Controllers
{
    [Route("Demonstration/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityRepository securityRepository;
        public SecurityController(ISecurityRepository repository)
        {
            securityRepository = repository;
        }

        [HttpGet("{UserId}/Info")]
        [Authorize]
        public async Task<IActionResult> GetSecurityInfo(string UserId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await securityRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await securityRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var profilepage = await securityRepository.GetSecurityInfo(UserId);
            return Ok(profilepage);
        }

        [HttpPut("{UserId}/Info")]
        [Authorize]
        public async Task<IActionResult> UpdateSecurityInfo(string UserId, [FromBody] SecurityInfoPageDTO securityinfo)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await securityRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await securityRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var change = await securityRepository.UpdateSecurityInfo(UserId, securityinfo);
            if (!change) { return StatusCode(500); }
            return Ok();
        }

        [HttpPut("{UserId}/Password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string UserId, [FromBody] string newpassword)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await securityRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await securityRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var ispasswordnew = await securityRepository.IsPasswordNew(UserId, newpassword);
            if (!ispasswordnew) { return BadRequest("This Password Was Already Used!"); }
            var change = await securityRepository.ChangePassword(UserId, newpassword);
            if (!change) { return StatusCode(500); }
            return Ok();
        }

        [HttpDelete("{UserId}/Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await securityRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await securityRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var delete = await securityRepository.DeleteUser(UserId);
            if (!delete) { return StatusCode(500); }
            return Ok();
        }


    }
}
