using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NinjaManagerProg5.DbAccess;
using NinjaManagerProg5.Models;
using NinjaManagerProg5.ViewModels;

namespace NinjaManagerProg5.Controllers;

public class NinjaController(ILogger<NinjaController> logger, MyNinjaDbContext dbContext)
    : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        List<Ninja> ninjalist = dbContext.Ninja
            .Include(n => n.NinjaEquipments)
            .OrderBy(n => n.Name)
            .ToList();

        List<NinjaVM> ninjaVMList = new List<NinjaVM>();

        // Get all equipments
        var allEquipments = dbContext.Equipments.ToList();

        foreach (Ninja ninja in ninjalist)
        {
            NinjaVM ninjaVM = new NinjaVM(ninja, allEquipments);
            ninjaVMList.Add(ninjaVM);
        }

        return View(ninjaVMList);
    }
    
    // POST: Ninja/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ninja = await dbContext.Ninja.FindAsync(id);
        if (ninja == null)
        {
            return NotFound();
        }
        TempData["Message"] = $"Ninja {ninja.Name} is succesvol verwijderd.";
        dbContext.Ninja.Remove(ninja);
        await dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    // GET: Ninja/Upsert
    // GET: Ninja/Upsert/{id}
    [HttpGet]
    public async Task<IActionResult> Upsert(int? id)
    {
        Ninja ninja = new Ninja();
        List<Equipment> ninjaEquipments = new List<Equipment>();

        if (id.HasValue)
        {
            // Get ninja with equipment
            ninja = await dbContext.Ninja
                .Include(n => n.NinjaEquipments)
                .ThenInclude(ne => ne.Equipment)
                .FirstOrDefaultAsync(n => n.Id == id.Value);

            // Only select equipment from selected ninja
            if (ninja != null)
            {
                ninjaEquipments = ninja.NinjaEquipments
                    .Select(ne => ne.Equipment)
                    .ToList();
            }
        }
        else
        {
            // Check if ninja exists
            ninja.Id = dbContext.Ninja.Any() ? dbContext.Ninja.Max(n => n.Id) + 1 : 1;
            
        }

        // create  NinjaVM with relevant equipment for the ninja
        var ninjaVM = new NinjaVM(ninja, ninjaEquipments);
        return View(ninjaVM);
    }


    // POST: Ninja/Upsert
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Upsert(NinjaVM ninjaVM, int[] equipments)
    {
        if (ModelState.IsValid)
        {
            // Search for existing ninja
            var existingNinja = await dbContext.Ninja
                .Include(n => n.NinjaEquipments)
                .FirstOrDefaultAsync(n => n.Id == ninjaVM.Ninja.Id);
            if (existingNinja == null)
            {
                // Add new ninja
                var newNinja = new Ninja { Name = ninjaVM.Ninja.Name, Gold = ninjaVM.Ninja.Gold };
                dbContext.Ninja.Add(newNinja);
                await dbContext.SaveChangesAsync(); // Async save
                TempData["Message"] = "Ninja is succesvol aangemaakt.";
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                existingNinja.Name = ninjaVM.Ninja.Name;
                existingNinja.Gold = ninjaVM.Ninja.Gold;

                var currentEquipmentIds = existingNinja.NinjaEquipments.Select(ne => ne.EquipmentId).ToList();
            }
            TempData["Message"] = "Ninja is succesvol opgeslagen.";
            await dbContext.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index));
        }
        TempData["Message"] = "error bij het opslaan van de ninja.";    
        return RedirectToAction(nameof(Index));
    }
}
