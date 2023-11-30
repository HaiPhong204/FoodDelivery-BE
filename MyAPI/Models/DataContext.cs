using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAPI.Models
{
	public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<CartModel> Carts => Set<CartModel>();
        public DbSet<FoodModel> Foods => Set<FoodModel>();
        public DbSet<OrderModel> Orders => Set<OrderModel>();
        public DbSet<OrderDetailsModel> OrderDetails => Set<OrderDetailsModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartModel>()
                .HasOne(b => b.UserModel)
                .WithMany(p => p.Carts)
                .HasForeignKey(p => p.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CartModel>()
                .HasOne(b => b.FoodModel)
                .WithOne(p => p.CartModel)
                .HasForeignKey<CartModel>(p => p.FoodId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderDetailsModel>()
                .HasOne(b => b.OrderModel)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderDetailsModel>()
                .HasOne(b => b.FoodModel)
                .WithOne(p => p.OrderDetailsModel)
                .HasForeignKey<OrderDetailsModel>(p => p.FoodId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderModel>()
                .HasOne(u => u.UserModel)
                .WithMany(o => o.Orders)
                .HasForeignKey(p => p.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}

