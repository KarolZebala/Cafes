using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi.Entities
{
    public class CafeDbContext : DbContext
    {
        private string _conectionString = "Server=.;Database=CafeDb;Trusted_Connection=True;";
        public DbSet<Cafe> Cafes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .Property(r => r.Email)
               .IsRequired();

            modelBuilder.Entity<Role>()
               .Property(r => r.Name)
               .IsRequired();


            modelBuilder.Entity<Cafe>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Drink>()
                .Property(d => d.Name)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(d => d.City)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(d => d.Street)
                .IsRequired()
                .HasMaxLength(50);






        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conectionString);
        }
    }
}
