using System.Collections.Generic;
using NinjaManagerProg5.Models;

namespace NinjaManagerProg5.ViewModels
{
    public class NinjaVM
    {
       public Ninja Ninja { get; set; }

        public List<Equipment> Equipments { get; set; }

        public NinjaVM()
        {
            Ninja = new Ninja();
            Equipments = new List<Equipment>();
        }

        public NinjaVM(Ninja newNinja, List<Equipment> allEquipments)
        {
            Ninja = newNinja;

            // Filter the equipment to only include the equipment associated with this ninja
            Equipments = newNinja.NinjaEquipments
                .Select(ne => allEquipments.FirstOrDefault(e => e.Id == ne.EquipmentId))
                .Where(e => e != null) // Ensure we only include existing equipment
                .ToList();
        }
        
        
    }
}

    
