using Learning.DTO.ElementDTO;
using Learning.Model;

namespace Learning.DTO.PagesDTO
{
    public class UserProfilePageDTO : BaseDTO
    {
        public ICollection<AdressDTO> Adresses { get; set; }
        public ICollection<OrderDTO> Orders { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
