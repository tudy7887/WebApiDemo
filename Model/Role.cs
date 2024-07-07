using Learning.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning.Model
{
    public class Role : IdentityRole, IBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        override public string Id { get; set; }
        public virtual ICollection<RoleUser> RoleUser { get; set; }
    }
}
