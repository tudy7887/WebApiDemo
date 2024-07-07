using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning.Model
{
    public class Cart
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Key]
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
