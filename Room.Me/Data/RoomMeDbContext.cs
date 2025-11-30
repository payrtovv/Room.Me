using Microsoft.EntityFrameworkCore;
using Room.Me.Models;

namespace Room.Me.Data
{
    public class RoomMeDbContext : DbContext
    {
        public RoomMeDbContext(DbContextOptions<RoomMeDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Rooms> Rooms { get; set; }

        public DbSet<RoomRule> RoomRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rooms>()
                .HasMany(r => r.RoomRule)
                .WithOne(rr => rr.Room)
                .HasForeignKey(rr => rr.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }


        //public DbSet<Preferences> Preferences { get; set; }
        //public DbSet<UserPreferences> UserPreferences { get; set; } 
        //public DbSet<Tags> Tags { get; set; }
    }
}
