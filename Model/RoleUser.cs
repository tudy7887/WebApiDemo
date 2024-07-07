using Learning.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning.Model
{
    public class RoleUser : IdentityUserRole<string>
    {
        [Key]
        [ForeignKey("User")]
        override public string UserId { get; set; }
        [Key]
        [ForeignKey("Role")]
        override public string RoleId { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        
    }
}
