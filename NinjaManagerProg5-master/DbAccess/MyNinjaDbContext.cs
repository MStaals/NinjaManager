using Microsoft.EntityFrameworkCore;
using NinjaManagerProg5.Models;

namespace NinjaManagerProg5.DbAccess
{
    public class MyNinjaDbContext : DbContext
    {
        public MyNinjaDbContext(DbContextOptions<MyNinjaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ninja> Ninja { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<NinjaEquipment> NinjaEquipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuratie voor de veel-op-veel-relatie tussen Ninja en Equipment
            modelBuilder.Entity<NinjaEquipment>()
                .HasKey(ne => new { ne.NinjaId, ne.EquipmentId });

            modelBuilder.Entity<NinjaEquipment>()
                .HasOne(ne => ne.Ninja)
                .WithMany(n => n.NinjaEquipments)
                .HasForeignKey(ne => ne.NinjaId);

            modelBuilder.Entity<NinjaEquipment>()
                .HasOne(ne => ne.Equipment)
                .WithMany(e => e.NinjaEquipments)
                .HasForeignKey(ne => ne.EquipmentId);

            // Seed data voor Ninja
            modelBuilder.Entity<Ninja>().HasData(
                new Ninja { Id = 1, Name = "Ninja A", Gold = 100 },
                new Ninja { Id = 2, Name = "Ninja B", Gold = 150 },
                new Ninja { Id = 3, Name = "Ninja C", Gold = 120 },
                new Ninja { Id = 4, Name = "Ninja D", Gold = 180 },
                new Ninja { Id = 5, Name = "Ninja E", Gold = 90 },
                new Ninja { Id = 6, Name = "Ninja F", Gold = 200 },
                new Ninja { Id = 7, Name = "Ninja G", Gold = 110 },
                new Ninja { Id = 8, Name = "Ninja H", Gold = 140 },
                new Ninja { Id = 9, Name = "Ninja I", Gold = 160 },
                new Ninja { Id = 10, Name = "Ninja J", Gold = 130 }
            );

            // Seed data voor Equipment
            modelBuilder.Entity<Equipment>().HasData(
                new Equipment { Id = 1, Name = "Sword", Category = "Hand", Strength = 5, Intelligence = 0, Agility = 3, GoldValue = 50 },
                new Equipment { Id = 2, Name = "Helmet", Category = "Head", Strength = 2, Intelligence = 1, Agility = 0, GoldValue = 30 },
                new Equipment { Id = 3, Name = "Armor", Category = "Chest", Strength = 10, Intelligence = 0, Agility = 2, GoldValue = 80 },
                new Equipment { Id = 4, Name = "Shield", Category = "Hand", Strength = 3, Intelligence = 0, Agility = 1, GoldValue = 40 },
                new Equipment { Id = 5, Name = "Boots", Category = "Feet", Strength = 1, Intelligence = 0, Agility = 5, GoldValue = 25 },
                new Equipment { Id = 6, Name = "Amulet", Category = "Neck", Strength = 0, Intelligence = 5, Agility = 2, GoldValue = 60 },
                new Equipment { Id = 7, Name = "Gloves", Category = "Hands", Strength = 2, Intelligence = 0, Agility = 4, GoldValue = 35 },
                new Equipment { Id = 8, Name = "Ring", Category = "Finger", Strength = 1, Intelligence = 2, Agility = 3, GoldValue = 45 },
                new Equipment { Id = 9, Name = "Belt", Category = "Waist", Strength = 2, Intelligence = 0, Agility = 1, GoldValue = 20 },
                new Equipment { Id = 10, Name = "Cloak", Category = "Back", Strength = 1, Intelligence = 3, Agility = 4, GoldValue = 70 }
            );

            // Seed data voor NinjaEquipment (Koppelingen tussen Ninja en Equipment)
            modelBuilder.Entity<NinjaEquipment>().HasData(
                new NinjaEquipment { NinjaId = 1, EquipmentId = 1 },
                new NinjaEquipment { NinjaId = 1, EquipmentId = 2 },
                new NinjaEquipment { NinjaId = 2, EquipmentId = 3 },
                new NinjaEquipment { NinjaId = 2, EquipmentId = 4 },
                new NinjaEquipment { NinjaId = 3, EquipmentId = 5 },
                new NinjaEquipment { NinjaId = 3, EquipmentId = 6 },
                new NinjaEquipment { NinjaId = 4, EquipmentId = 7 },
                new NinjaEquipment { NinjaId = 4, EquipmentId = 8 },
                new NinjaEquipment { NinjaId = 5, EquipmentId = 9 },
                new NinjaEquipment { NinjaId = 5, EquipmentId = 10 },
                new NinjaEquipment { NinjaId = 6, EquipmentId = 1 },
                new NinjaEquipment { NinjaId = 6, EquipmentId = 3 },
                new NinjaEquipment { NinjaId = 7, EquipmentId = 2 },
                new NinjaEquipment { NinjaId = 7, EquipmentId = 4 },
                new NinjaEquipment { NinjaId = 8, EquipmentId = 5 },
                new NinjaEquipment { NinjaId = 8, EquipmentId = 6 },
                new NinjaEquipment { NinjaId = 9, EquipmentId = 7 },
                new NinjaEquipment { NinjaId = 9, EquipmentId = 8 },
                new NinjaEquipment { NinjaId = 10, EquipmentId = 9 },
                new NinjaEquipment { NinjaId = 10, EquipmentId = 10 }
            );
        }
    }
}
