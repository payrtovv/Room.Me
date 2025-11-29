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
        public DbSet<Preferences> Preferences { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; } 
    }
}
