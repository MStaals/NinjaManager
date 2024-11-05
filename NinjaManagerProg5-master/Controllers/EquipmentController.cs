using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NinjaManagerProg5.Models; // Pas aan naar je eigen namespace
using System.Threading.Tasks;
using NinjaManagerProg5.DbAccess;
using NinjaManagerProg5.ViewModels;

public class EquipmentController(ILogger<EquipmentController> logger, MyNinjaDbContext dbContext) : Controller
{

    // GET: Equipment
    [HttpGet]
    public ActionResult Index()
    {
        List<Equipment> equipmentList = dbContext.Equipments
            .OrderBy(e => e.Name)
            .ToList();

        List<EquipmentVM> equipmentVMList = new List<EquipmentVM>();

        foreach (Equipment equipment in equipmentList)
        {
            EquipmentVM equipmentVM = new EquipmentVM(equipment);
            equipmentVMList.Add(equipmentVM);
        }

        return View(equipmentVMList);
    }
// GET: Equipment/Upsert
// GET: Equipment/Upsert/{id}
[HttpGet]
public async Task<IActionResult> Upsert(int? id)
{
    Equipment equipment = new Equipment();

    if (id.HasValue)
    {
        // Try to find the equipment; if not found, return 404
        equipment = await dbContext.Equipments.FirstOrDefaultAsync(e => e.Id == id.Value);
        if (equipment == null)
        {
            return NotFound(); // If equipment not found, return 404
        }
    }
    else // For new equipment creation, initialize a new Equipment instance
    {
        equipment.Id = dbContext.Equipments.Any() ? dbContext.Equipments.Max(e => e.Id) + 1 : 1;
    }

    var equipmentVM = new EquipmentVM(equipment);
    return View(equipmentVM);
}

// POST: Equipment/Upsert
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<ActionResult> Upsert(EquipmentVM equipmentVM, int[] equipments)
{
    if (ModelState.IsValid)
    {
        var existingEquipment = await dbContext.Equipments
            .FirstOrDefaultAsync(e => e.Id == equipmentVM.Equipment.Id);
           logger.LogInformation("ModelState is valid. Equipment ID: {Id}", equipmentVM.Equipment.Id);

            if (existingEquipment == null)
            { 
                    var newEquipment = new Equipment
                    {
                        Name = equipmentVM.Equipment.Name,
                        Category = equipmentVM.Equipment.Category,
                        Strength = equipmentVM.Equipment.Strength,
                        Intelligence = equipmentVM.Equipment.Intelligence,
                        Agility = equipmentVM.Equipment.Agility,
                        GoldValue = equipmentVM.Equipment.GoldValue
                    };
                   dbContext.Equipments.Add(newEquipment);
                   TempData["Message"] = "Equipment toegevoegd";
            }
            else
            {
                // Update de velden van het bestaande equipment
                existingEquipment.Name = equipmentVM.Equipment.Name;
                existingEquipment.Category = equipmentVM.Equipment.Category;
                existingEquipment.Strength = equipmentVM.Equipment.Strength;
                existingEquipment.Intelligence = equipmentVM.Equipment.Intelligence;
                existingEquipment.Agility = equipmentVM.Equipment.Agility;
                existingEquipment.GoldValue = equipmentVM.Equipment.GoldValue;
                TempData["Message"] = "Equipment bijgewerkt";
                
            }
            

            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        
    }
    TempData["Message"] = "Error bij het toevoegen van equipment";
    return RedirectToAction(nameof(Index));
}

    // POST: Equipment/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var equipment = await dbContext.Equipments.FindAsync(id);
        if (equipment == null)
        {
            return NotFound();
        }

        dbContext.Equipments.Remove(equipment);
        await dbContext.SaveChangesAsync();
        TempData["Message"] = "Equipment verwijderd";
        return RedirectToAction(nameof(Index));
    }
    
}
