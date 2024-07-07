using Learning.DTO.ElementDTO;

namespace Learning.DTO.PagesDTO
{
    public class CartPageDTO : BaseDTO
    {
        public ICollection<ConsumerProductDTO> Products { get; set; }
    }
}
