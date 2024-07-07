using Learning.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Learning.Data
{
    public class DataContext : IdentityDbContext<User, Role, string>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<Image> Images { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleUser> RoleUser { get; set; }
        public DbSet<User> User { get; set; }       
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<Image>().HasKey(i => i.Id);
            modelBuilder.Entity<Image>().HasOne(i => i.User).WithOne(u => u.ProfilePicture).HasForeignKey<Image>(i => i.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Image>().HasOne(i => i.Product).WithMany(p => p.Pictures).HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserLog>().ToTable("UserLog");
            modelBuilder.Entity<UserLog>().HasKey(l => l.Id);
            modelBuilder.Entity<UserLog>().HasOne(l => l.User).WithMany(u => u.Logs);

            modelBuilder.Entity<Password>().ToTable("Password");
            modelBuilder.Entity<Password>().HasKey(p => p.Id);
            modelBuilder.Entity<Password>().HasOne(p => p.User).WithMany(u => u.Passwords);

            modelBuilder.Entity<Adress>().ToTable("Adress");
            modelBuilder.Entity<Adress>().HasKey(a => a.Id);
            modelBuilder.Entity<Adress>().HasOne(a => a.User).WithMany(u => u.Adresses);
            modelBuilder.Entity<Adress>().HasMany(a => a.Orders).WithOne(o => o.Adress);

            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().HasMany(p => p.ProductOrders).WithOne(po => po.Product);
            modelBuilder.Entity<Product>().HasMany(p => p.Cart).WithOne(c => c.Product);
            modelBuilder.Entity<Product>().HasOne(p => p.User).WithMany(u => u.Products);
            modelBuilder.Entity<Product>().HasMany(p => p.Pictures).WithOne(p => p.Product);

            modelBuilder.Entity<ProductOrder>().ToTable("ProductOrder");
            modelBuilder.Entity<ProductOrder>().HasKey(po => new { po.ProductId, po.OrderId });
            modelBuilder.Entity<ProductOrder>().HasOne(p => p.Product).WithMany(po => po.ProductOrders).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductOrder>().HasOne(o => o.Order).WithMany(po => po.ProductOrders).IsRequired().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().HasMany(o => o.ProductOrders).WithOne(po => po.Order);
            modelBuilder.Entity<Order>().HasOne(o => o.Adress).WithMany(a => a.Orders);

            modelBuilder.Entity<Cart>().ToTable("Cart");
            modelBuilder.Entity<Cart>().HasKey(c => new { c.ProductId, c.UserId });
            modelBuilder.Entity<Cart>().HasOne(p => p.Product).WithMany(c => c.Cart).HasForeignKey(c => c.ProductId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Cart>().HasOne(u => u.User).WithMany(c => c.Cart).HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().HasMany(r => r.RoleUser).WithOne(ur => ur.Role);

            modelBuilder.Entity<RoleUser>().ToTable("RoleUser");
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(ur => new { ur.RoleId, ur.UserId });
            modelBuilder.Entity<RoleUser>().HasOne(r => r.Role).WithMany(ur => ur.RoleUser).HasForeignKey(ur => ur.RoleId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<RoleUser>().HasOne(u => u.User).WithMany(ur => ur.RoleUser).HasForeignKey(ur => ur.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasMany(u => u.Adresses).WithOne(a => a.User);
            modelBuilder.Entity<User>().HasMany(u => u.Products).WithOne(p => p.User);
            modelBuilder.Entity<User>().HasMany(u => u.Passwords).WithOne(p => p.User);
            modelBuilder.Entity<User>().HasMany(u => u.Logs).WithOne(l => l.User);
            modelBuilder.Entity<User>().HasMany(u => u.Cart).WithOne(c => c.User);
            modelBuilder.Entity<User>().HasMany(u => u.RoleUser).WithOne(ur => ur.User);
            modelBuilder.Entity<User>().HasOne(u => u.ProfilePicture).WithOne(p => p.User);

            List<Role> roles = new List<Role>
            {
                new Role{Name = "Admin", NormalizedName = "ADMIN" },
                new Role{Name = "Producer", NormalizedName = "PRODUCER" },
                new Role{Name = "User", NormalizedName = "USER" }
            };
            modelBuilder.Entity<Role>().HasData(roles);
        }
    }
}
