using Learning.Model;
using System.ComponentModel.DataAnnotations;

namespace Learning.DTO.ElementDTO
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public int Delivered { get; set; }
        public int Total { get; set; }
        public string OrderNumber { get; set; }
        public string Transport { get; set; }
        public int Cost { get; set; }
        public DateTime DateModified { get; set; }
        public AdressDTO Adress { get; set; }
        public ICollection<OrderProductDTO> Products { get; set; }
    }
}
