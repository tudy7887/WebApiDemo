using Learning.DTO.ElementDTO;

namespace Learning.DTO.PagesDTO
{
    public class AdminPageDTO : BaseDTO
    {
        public ICollection<UserRoleDTO> UserRoles { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
