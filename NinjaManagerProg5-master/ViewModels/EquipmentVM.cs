using NinjaManagerProg5.Models;

namespace NinjaManagerProg5.ViewModels;

public class EquipmentVM
{
    public Equipment Equipment {get; set;}
    public EquipmentVM()
    {
        Equipment = new Equipment()
        {
            NinjaEquipments = new List<NinjaEquipment>() // Initialize the collection
        };
    }

    public EquipmentVM(Equipment newEquipment)
    {
        Equipment = newEquipment;
    }

    
}
