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

        public DbSet<Rule> Rules { get; set; }

        public DbSet<Preference> Preferences { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rooms>()
                .HasMany(r => r.RoomRule)
                .WithOne(rr => rr.Room)
                .HasForeignKey(rr => rr.RoomId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Rule>()
                .HasMany(r => r.RoomRules)
                .WithOne(rr => rr.Rule)
                .HasForeignKey(rr => rr.RuleId)
                .OnDelete(DeleteBehavior.Cascade);

            //Semilla de Reglas definidas
            modelBuilder.Entity<Rule>().HasData(

                new Rule { Id = 1, Name = "Fumar", IsMandatory = true },
                new Rule { Id = 2, Name = "Mascotas", IsMandatory = true },
                new Rule { Id = 3, Name = "Visitas", IsMandatory = true },
                new Rule { Id = 4, Name = "Reuniones", IsMandatory = true },
                new Rule { Id = 5, Name = "Alcohol", IsMandatory = true }
               );

            modelBuilder.Entity<UserPreference>()
                .HasKey(up => new { up.UserId, up.PreferenceId });

            modelBuilder.Entity<UserPreference>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPreferences)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPreference>()
                .HasOne(up => up.Preference)
                .WithMany(p => p.UserPreferences)
                .HasForeignKey(up => up.PreferenceId);

            // seed data for Preferences
            modelBuilder.Entity<Preference>().HasData(
            
                new Preference { Id = 1, Category = "personality", Label = "Extrovertido", Value = "extrovert" },
                new Preference { Id = 2, Category = "personality", Label = "Ambivertido", Value = "ambivert" },
                new Preference { Id = 3, Category = "personality", Label = "Introvertido", Value = "introvert" },

                new Preference { Id = 4, Category = "schedule", Label = "Madrugador", Value = "early_bird" },
                new Preference { Id = 5, Category = "schedule", Label = "Horario Flexible", Value = "flexible" },
                new Preference { Id = 6, Category = "schedule", Label = "Nocturno", Value = "night_owl" },

                new Preference { Id = 7, Category = "cleanliness", Label = "Super Ordenado", Value = "neat" },
                new Preference { Id = 8, Category = "cleanliness", Label = "Orden Normal", Value = "average" },
                new Preference { Id = 9, Category = "cleanliness", Label = "Desordenado", Value = "messy" },

                new Preference { Id = 10, Category = "pets", Label = "Tengo Mascotas", Value = "has_pets" },
                new Preference { Id = 11, Category = "pets", Label = "Acepto Mascotas", Value = "ok_with" },
                new Preference { Id = 12, Category = "pets", Label = "Cero Mascotas", Value = "none" },

                new Preference { Id = 13, Category = "visits", Label = "Casa de Fiesta", Value = "party_house" },
                new Preference { Id = 14, Category = "visits", Label = "Visitas Moderadas", Value = "occasional" },
                new Preference { Id = 15, Category = "visits", Label = "Sin Visitas", Value = "private" },

                new Preference { Id = 16, Category = "habits", Label = "Fumador", Value = "smoker" },
                new Preference { Id = 17, Category = "habits", Label = "Fumo afuera", Value = "outside_only" },
                new Preference { Id = 18, Category = "habits", Label = "No fumador", Value = "non_smoker" }
            );

            
        }

    }
}
