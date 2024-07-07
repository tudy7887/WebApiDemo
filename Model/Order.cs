using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Learning.Interfaces;

namespace Learning.Model
{
    public class Order : IBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public required DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public int Total { get; set; }
        public int Devivered { get; set; }
        [Required]
        public required string OrderNumber { get; set; }
        public string Transport { get; set; }
        [Required]
        public required int Cost { get; set; }
        public DateTime DateModified { get; set; }
        [Required]
        [ForeignKey("Adress")]
        public string AdressId { get; set; }
        public virtual required Adress Adress { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
