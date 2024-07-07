namespace Learning.DTO.ElementDTO
{
    public class UserRoleDTO
    {
        public string UserId { get; set; }
        public string UserUsername { get; set; }
        public string UserEmail { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public ICollection<string> UserRoles { get; set; }
    }
}
