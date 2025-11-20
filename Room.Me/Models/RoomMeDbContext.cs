using Microsoft.EntityFrameworkCore;

namespace Room.Me.Models
{
    public class RoomMeDbContext : DbContext
    {
        public RoomMeDbContext (DbContextOptions<RoomMeDbContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
