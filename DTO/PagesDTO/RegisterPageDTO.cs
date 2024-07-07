using System.ComponentModel.DataAnnotations;

namespace Learning.DTO.PagesDTO
{
    public class RegisterPageDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
