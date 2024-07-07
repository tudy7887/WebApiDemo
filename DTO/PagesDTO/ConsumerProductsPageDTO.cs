using Learning.DTO.ElementDTO;

namespace Learning.DTO.PagesDTO
{
    public class ConsumerProductsPageDTO : BaseDTO
    {
        public ICollection<ConsumerProductDTO> Products { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
