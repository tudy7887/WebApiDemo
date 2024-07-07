using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning.Model
{
    public class ProductOrder
    {
        [Key]
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        [Key]
        [ForeignKey("Order")]
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public required string Status { get; set; }
    }
}
