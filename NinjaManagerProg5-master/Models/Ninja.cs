using System.Collections.Generic;

namespace NinjaManagerProg5.Models
{
    public class Ninja
    {
        public Ninja()
        {
            
        }
        public Ninja(int id, String name, int gold)
        {
            this.Id = Id;
            this.Name = Name;
            this.Gold = gold;
            
        }
        public int Id { get; set; }
        public string Name { get; set; }      // Ninja's naam
        public int Gold { get; set; }         // Ninja's beschikbare goud

        // Verzameling van NinjaEquipment voor de veel-op-veel-relatie
        public ICollection<NinjaEquipment> NinjaEquipments { get; set; } = new List<NinjaEquipment>();
    }
}