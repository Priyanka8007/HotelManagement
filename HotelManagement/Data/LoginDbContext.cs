using HotelManagement.Models;
using HotelManagement.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HotelManagement.Data
{
    public class LoginDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options)
        {
        }
        public DbSet<Hotel> Hotels { get; set; } // DbSet for Hotel model
        public DbSet<Item> Items { get; set; } // DbSet for Hotel model

        // DbSet to represent the result of the stored procedure
        public DbSet<HotelItemDetails> HotelItemDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<HotelItemDetails>().HasNoKey();

            base.OnModelCreating(builder);

            var superAdminRoleId = "5d6ffabf-54fb-402e-9c56-5633c88bc123";
            var adminRoleId = "5d6ffabf-54fb-402e-9c56-5633c88bc345";
            var userRoleId = "b4bbec88-2c3a-417b-89da-bfe5ec46bde1";
            var kitchenRoleId = "d60abcf5-d529-4f47-99f2-3a7205e7bb1b";

            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id = superAdminRoleId, Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = kitchenRoleId, Name = "Kitchen", NormalizedName = "KITCHEN" }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
