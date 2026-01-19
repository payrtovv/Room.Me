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
        public DbSet<Rule> Rules { get; set; }

        public DbSet<Feature> Feature { get; set; }

        public DbSet<RoomFeature> RoomFeatures { get; set; }


        public DbSet<Preference> Preferences { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<RoomMedia> RoomMedia { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Rooms>()
                .HasMany(r => r.Rules)
                .WithOne(rr => rr.Room)
                .HasForeignKey(rr => rr.RoomId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RoomFeature>()
                .HasKey(rf => new { rf.RoomId, rf.FeatureId });

            modelBuilder.Entity<RoomFeature>()
                .HasOne(rf => rf.Room)
                .WithMany(r => r.RoomFeatures)
                .HasForeignKey(rf => rf.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomFeature>()
                .HasOne(rf => rf.Feature)
                .WithMany(f => f.RoomFeatures)
                .HasForeignKey(rf => rf.FeatureId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Feature>().HasData(
                new Feature { Id = 1, Key = "wifi", Name = "Internet / Wifi", Category = "Servicios y Conectividad" },
                new Feature { Id = 2, Key = "agua_caliente", Name = "Agua caliente", Category = "Servicios y Conectividad" },
                new Feature { Id = 3, Key = "servicios_basicos", Name = "Servicios básicos incluidos (Luz/Agua/Gas)", Category = "Servicios y Conectividad" },
                new Feature { Id = 4, Key = "limpieza_comun", Name = "Limpieza de áreas comunes", Category = "Servicios y Conectividad" },
                new Feature { Id = 5, Key = "seguridad_24h", Name = "Portería / Seguridad 24h", Category = "Servicios y Conectividad" },
                new Feature { Id = 6, Key = "ascensor", Name = "Ascensor", Category = "Servicios y Conectividad" },
                new Feature { Id = 7, Key = "bodega", Name = "Bodega", Category = "Servicios y Conectividad" },

                new Feature { Id = 8, Key = "terraza", Name = "Terraza / Rooftop", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 9, Key = "bbq", Name = "Zona BBQ / Parrilla", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 10, Key = "piscina", Name = "Piscina", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 11, Key = "sauna", Name = "Sauna / Turco", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 12, Key = "jacuzzi", Name = "Hidromasaje / Jacuzzi", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 13, Key = "canchas", Name = "Canchas deportivas", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 14, Key = "sala_eventos", Name = "Sala comunal / Eventos", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 15, Key = "gimnasio", Name = "Gimnasio", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 16, Key = "sala_tv", Name = "Sala de TV / Cine", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 17, Key = "jardin", Name = "Jardín interior", Category = "Áreas Sociales y Bienestar" },
                new Feature { Id = 18, Key = "suite_huespedes", Name = "Suite de Huéspedes", Category = "Áreas Sociales y Bienestar" },

                new Feature { Id = 19, Key = "linea_blanca", Name = "Línea Blanca Completa", Category = "Equipamiento del Hogar" },
                new Feature { Id = 20, Key = "microondas", Name = "Microondas", Category = "Equipamiento del Hogar" },
                new Feature { Id = 21, Key = "lavadora", Name = "Lavadora", Category = "Equipamiento del Hogar" },
                new Feature { Id = 22, Key = "secadora", Name = "Secadora", Category = "Equipamiento del Hogar" },
                new Feature { Id = 23, Key = "pequenos_electro", Name = "Pequeños Electrodomésticos", Category = "Equipamiento del Hogar" },
                new Feature { Id = 24, Key = "menaje_cocina", Name = "Menaje de Cocina", Category = "Equipamiento del Hogar" },
                new Feature { Id = 25, Key = "tv_sala", Name = "TV en sala (Smart TV)", Category = "Equipamiento del Hogar" },
                new Feature { Id = 26, Key = "sala_comedor", Name = "Sala y Comedor amoblados", Category = "Equipamiento del Hogar" }
            );

               
            modelBuilder.Entity<UserPreference>()
                .HasKey(up => new { up.UserId, up.PreferenceId });

            modelBuilder.Entity<UserPreference>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPreferences)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPreference>()
                .HasOne(up => up.Preference)
                .WithMany(p => p.UserPreferences)
                .HasForeignKey(up => up.PreferenceId).
                OnDelete(DeleteBehavior.Restrict); 

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

            modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany() 
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany() 
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

        }



    }
}
