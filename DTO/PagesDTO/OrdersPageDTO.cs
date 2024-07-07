using Learning.DTO.ElementDTO;

namespace Learning.DTO.PagesDTO
{
    public class OrdersPageDTO : BaseDTO
    {
        public ICollection<OrderDTO> Orders { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
