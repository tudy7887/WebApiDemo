using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Learning.Interfaces;

namespace Learning.Model
{
    public class Password : IBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required, MaxLength(64)]
        public required byte[] Hash { get; set; }
        [Required, MaxLength(64)]
        public required byte[] Salt { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
