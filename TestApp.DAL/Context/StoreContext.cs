using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestApp.DAL.Entities;


namespace TestApp.DAL.Context
{
    public class StoreContext : IdentityDbContext<Users>
    {

        public StoreContext( DbContextOptions<StoreContext> options ):base(options) 
        {
            
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Payment> payments { get; set; }

        public DbSet<PaymentDetail> paymentDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>()
                .HasMany(p => p.Ratings)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId);

            modelBuilder.Entity<Categories>()
             .HasMany(p => p.Products)
             .WithOne(r => r.Category)
             .HasForeignKey(r => r.CategoryId)
             .OnDelete(DeleteBehavior.SetNull);


            base.OnModelCreating(modelBuilder);


            // Other configurations
        }


    }
}
