using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;
using Learning.Model;
using Learning.Interfaces;
using Learning.Services;
using Learning.DTO.PagesDTO;
using Microsoft.AspNetCore.Authorization;
using Learning.Extensions;
using Learning.Repository;

namespace Learning.Controllers
{
    [Route("Demonstration/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITokenService tokenService;
        public AccountController(IAccountRepository repository, ITokenService tokenservice)
        {
            accountRepository = repository;
            tokenService = tokenservice;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginPageDTO loginDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var user = await accountRepository.Login(loginDto);
            if (user == null) { return Unauthorized("Invalid Creditentials!"); }
            var author = await accountRepository.GetToken(user, tokenService.CreateToken(user));
            if (author == null) { return StatusCode(500); }
            return Ok(author);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterPageDTO registerDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var isnew = await accountRepository.IsUserNew(registerDto);
            if(!isnew) {  return Conflict("User ALready Exists!"); }
            var user = await accountRepository.CreateNewUser(registerDto);
            if (user == null) { return NoContent(); }
            var author = await accountRepository.GetToken(user, tokenService.CreateToken(user));
            if (author == null) { return StatusCode(500); }
            return Ok(author);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var user = await accountRepository.ForgotPassword(email);
            if (user == null) { return NotFound("Invalid Email!"); }
            var author = await accountRepository.GetToken(user, tokenService.CreateToken(user));
            if (author == null) { return StatusCode(500); }
            return Ok(author);
        }

        [HttpPut("{UserId}/ResetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(string UserId, [FromBody] string newpassword)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await accountRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await accountRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var ispasswordnew = await accountRepository.IsPasswordNew(authorId, newpassword);
            if (!ispasswordnew) { return BadRequest("This Password Was Already Used!"); }
            var newuser = await accountRepository.ResetPassword(authorId, newpassword);
            if (newuser == null) { return Unauthorized("Something Went Wrong!"); }
            author = await accountRepository.GetToken(newuser, tokenService.CreateToken(newuser));
            if (author == null) { return StatusCode(500); }
            return Ok(author);
        }
    }
}
