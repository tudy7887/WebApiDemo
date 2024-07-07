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
    public class ProfileInfoController : ControllerBase
    {
        private readonly IUserInfoRepository userInfoRepository;
        public ProfileInfoController(IUserInfoRepository repository)
        {
            userInfoRepository = repository;
        }

        [HttpGet("{UserId}")]
        [Authorize]
        public async Task<IActionResult> GetProfileInfo(string UserId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await userInfoRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var profilepage = await userInfoRepository.GetPersonalInfo(UserId);
            return Ok(profilepage);
        }

        [HttpGet("{UserId}/Adresses")]
        [Authorize]
        public async Task<IActionResult> GetAdresses(string UserId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await userInfoRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await userInfoRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var adresses = await userInfoRepository.GetAdresses(UserId);
            return Ok(adresses);
        }

        [HttpGet("{UserId}/Adresses/{AdressId}")]
        [Authorize]
        public async Task<IActionResult> GetAdress(string UserId, string AdressId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await userInfoRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await userInfoRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var ismyadress = await userInfoRepository.IfMyAdress(UserId, AdressId);
            if (!ismyadress) { return Forbid(); }
            var adresses = await userInfoRepository.GetAdress(UserId, AdressId);
            return Ok(adresses);
        }

        [HttpPut("{UserId}")]
        [Authorize]
        public async Task<IActionResult> UpdateProfileInfo(string UserId, [FromBody] UserProfilePageDTO profileinfo)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await userInfoRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await userInfoRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var change = await userInfoRepository.UpdatePersonalInfo(UserId, profileinfo);
            if (!change) { return StatusCode(500); }
            return Ok();
        }

        [HttpPut("{UserId}/Adresses/{AdressId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAdress(string UserId, string AdressId, [FromBody] SingleAdressPageDTO adress)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await userInfoRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await userInfoRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var change = await userInfoRepository.ChangeAdress(UserId, AdressId, adress);
            if (!change) { return StatusCode(500); }
            return Ok();
        }

        [HttpPost("{UserId}/Adresses")]
        [Authorize]
        public async Task<IActionResult> CreateAdress(string UserId, [FromBody] SingleAdressPageDTO adress)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await userInfoRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await userInfoRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var create = await userInfoRepository.CreateAdress(UserId, adress);
            if (!create) { return StatusCode(500); }
            return Ok();
        }

        [HttpDelete("{UserId}/Adresses/{AdressId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdress(string UserId, string AdressId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await userInfoRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await userInfoRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var delete = await userInfoRepository.RemoveAdress(UserId, AdressId);
            if (!delete) { return StatusCode(500); }
            return Ok();
        }

        [HttpPost("{UserId}/ProfilePicture")]
        [Authorize]
        public async Task<IActionResult> AddProfilePicture(string UserId, [FromBody] string image)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await userInfoRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await userInfoRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var create = await userInfoRepository.AddProfilePicture(UserId, image);
            if (!create) { return StatusCode(500); }
            return Ok();
        }

        [HttpDelete("{UserId}/ProfilePicture")]
        [Authorize]
        public async Task<IActionResult> DeleteProfilePicture(string UserId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var authorId = User.GetUserId();
            if (authorId == null) { return Unauthorized(); }
            var author = await userInfoRepository.GetToken(authorId);
            if (author == null) { return StatusCode(500); }
            var isme = await userInfoRepository.IfUserIsMe(author, UserId);
            if (!isme) { return Forbid(); }
            var delete = await userInfoRepository.RemoveProfilePicture(UserId);
            if (!delete) { return StatusCode(500); }
            return Ok();
        }

    }
}
