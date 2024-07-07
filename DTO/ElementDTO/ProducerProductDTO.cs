using System.ComponentModel.DataAnnotations;

namespace Learning.DTO.ElementDTO
{
    public class ProducerProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public ICollection<ProductImageDTO> Pictures { get; set; }
        public ICollection<OrderDTO> Orders { get; set; }
    }
}
