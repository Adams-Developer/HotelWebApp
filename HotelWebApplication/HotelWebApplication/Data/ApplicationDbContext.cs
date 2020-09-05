using System;
using System.Collections.Generic;
using System.Text;
using HotelWebApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelWebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ItemImage> ItemImageRelationships { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomFeature> RoomFeatureRelationships { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RoomFeature>()
                .HasKey(x => new { x.RoomId, x.FeatureId });

            builder.Entity<RoomFeature>()
                .HasOne(rf => rf.Room)
                .WithMany(r => r.Features);

            builder.Entity<RoomFeature>()
                .HasOne(f => f.Feature)
                .WithMany(r => r.Rooms);

            builder.Entity<ItemImage>()
                .HasKey(x => new { x.ItemId, x.ImageId });

            builder.Entity<RoomType>()
                .HasMany(b => b.Rooms)
                .WithOne(p => p.RoomType)
                .HasForeignKey(p => p.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
