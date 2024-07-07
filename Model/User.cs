using Learning.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning.Model
{
    public class User : IdentityUser, IBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        override public string Id { get; set; }
        [Required]
        public required string UserName { get; set; }
        [Required, MaxLength(64)]
        public required byte[] CurrentHash { get; set; }
        [Required, MaxLength(64)]
        public required byte[] CurrentSalt { get; set; }
        public virtual Image ProfilePicture { get; set; }

        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required DateTime DateCreated { get; set; }
        public virtual ICollection<Password> Passwords { get; set; }
        public virtual ICollection<Adress> Adresses { get; set; }
        public virtual ICollection<UserLog> Logs { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<RoleUser> RoleUser { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }

    }
}
