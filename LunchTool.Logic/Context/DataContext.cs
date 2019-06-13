using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using LunchTool.Logic.Enities;

namespace LunchTool.Logic.Context
{
    public class DataContext: DbContext
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDish>()
                .HasOne(d => d.Order)
                .WithMany(o => o.OrderDishes)
                .HasForeignKey(d => d.OrderId);

            modelBuilder.Entity<OrderDish>()
                .HasOne(o => o.Dish)
                .WithMany(d => d.OrderDishes)
                .HasForeignKey(o => o.DishId);


            modelBuilder.Entity<MenuDish>()
                .HasOne(m => m.Dish)
                .WithMany(d => d.MenuDishes)
                .HasForeignKey(m => m.DishId);

            modelBuilder.Entity<MenuDish>()
                .HasOne(d => d.Menu)
                .WithMany(m => m.MenuDishes)
                .HasForeignKey(d => d.MenuId);
        }
    }
}
