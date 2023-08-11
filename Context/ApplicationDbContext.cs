using bodybykhoshalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.Xml;

namespace bodybykhoshalApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<Chats> Chats { get; set; }
        public DbSet<Booking> Booking { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Users>().ToTable("Users", "dbo");
            modelBuilder.Entity<Packages>().ToTable("Packages", "dbo");
            modelBuilder.Entity<ShoppingCart>().ToTable("ShoppingCart", "dbo");
            modelBuilder.Entity<Chats>().ToTable("Chats", "dbo");
            modelBuilder.Entity<Booking>().ToTable("Booking", "dbo");

            base.OnModelCreating(modelBuilder);
        }
    }
}
