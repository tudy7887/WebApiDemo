using Learning.Model;

namespace Learning.DTO.PagesDTO
{
    public class SecurityInfoPageDTO : BaseDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public ICollection<DateTime> Logs { get; set; }
    }
}
