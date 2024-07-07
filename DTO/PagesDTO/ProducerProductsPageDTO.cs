using Learning.DTO.ElementDTO;

namespace Learning.DTO.PagesDTO
{
    public class ProducerProductsPageDTO : BaseDTO
    {
        public ICollection<ProducerProductDTO> Products { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
