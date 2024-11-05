using Microsoft.AspNetCore.Mvc;
using NinjaManagerProg5.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NinjaManagerProg5.DbAccess;
using NinjaManagerProg5.ViewModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;

namespace NinjaManagerProg5.Controllers
{
    public class ShopController(ILogger<ShopController> logger, MyNinjaDbContext dbContext) : Controller
    {
        // GET: Display the shop's main page with a list of available equipment and ninjas
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Retrieve all ninjas, including their associated equipment, ordered by name
            List<Ninja> ninjalist = await dbContext.Ninja
                .Include(n => n.NinjaEquipments) 
                .ThenInclude(ne => ne.Equipment) 
                .OrderBy(n => n.Name)
                .ToListAsync();

            // Store the list of ninjas in ViewBag for use in the view
            ViewBag.NinjaList = ninjalist;

            // Retrieve all available equipment, ordered by name
            List<Equipment> equipmentList = await dbContext.Equipments
                .OrderBy(e => e.Name)
                .ToListAsync();

            // Convert each Equipment object to an EquipmentVM and store in a list
            List<EquipmentVM> equipmentVMList = equipmentList
                .Select(e => new EquipmentVM(e))
                .ToList();

            // Create a NinjaVM for each ninja, associating it with the equipment list
            var ninjaVMList = ninjalist.Select(ninja => new NinjaVM(ninja, equipmentList)).ToList();
            ViewBag.NinjaVMList = ninjaVMList;

            // Pass the list of equipment view models to the view
            return View(equipmentVMList);
        }

        // POST: Buy action to allow a ninja to purchase equipment
        [HttpPost]
        public IActionResult Buy(int eid, int nid)
        {
            // Find the specified ninja and include their equipment for category checks
            var ninja = dbContext.Ninja
                .Include(n => n.NinjaEquipments)
                .ThenInclude(ne => ne.Equipment)
                .FirstOrDefault(n => n.Id == nid);

            // Find the specified equipment by ID
            var equipment = dbContext.Equipments.FirstOrDefault(e => e.Id == eid);

            // Check if ninja or equipment was not found
            if (ninja == null || equipment == null)
            {
                logger.LogError("Ninja or equipment not found.");
                TempData["Message"] = "Ninja or equipment not found.";
                return RedirectToAction("Index", "Shop");
            }

            // Check if ninja already has an item in the same category
            var existingItem = ninja.NinjaEquipments
                .Select(ne => ne.Equipment)
                .FirstOrDefault(e => e != null && e.Category == equipment.Category);

            // If an item in the same category exists, cancel the purchase and inform the user
            if (existingItem != null)
            {
                TempData["Message"] = $"This ninja already has an item in this category. " +
                                      $"Existing item: {existingItem.Name} (Category: {existingItem.Category}), " +
                                      $"New item: {equipment.Name} (Category: {equipment.Category})";
                return RedirectToAction("Index");
            }

            // Check if the ninja has enough gold to purchase the item
            if (ninja.Gold < equipment.GoldValue)
            {
                TempData["Message"] = "Not enough gold.";
                return RedirectToAction("Index");
            }

            // Process the purchase: deduct the item's cost from the ninja's gold and add the equipment
            ninja.Gold -= equipment.GoldValue;
            ninja.NinjaEquipments.Add(new NinjaEquipment
            {
                NinjaId = ninja.Id,
                EquipmentId = equipment.Id
            });

            // Save the changes to the database
            dbContext.SaveChanges();
            TempData["Message"] = $"Item '{equipment.Name}' purchased for {equipment.GoldValue} gold by ninja '{ninja.Name}'.";
            return RedirectToAction("Index");
        }

        // POST: Sell action to allow a ninja to sell equipment
        [HttpPost]
        public async Task<IActionResult> Sell(int eid, int nid)
        {
            // Find the specified ninja by ID and include their equipment
            var ninja = await dbContext.Ninja
                .Include(n => n.NinjaEquipments)
                .ThenInclude(ninjaEquipment => ninjaEquipment.Equipment)
                .FirstOrDefaultAsync(n => n.Id == nid);

            // Check if ninja was not found
            if (ninja == null)
            {
                TempData["Message"] = "Ninja not found.";
                return RedirectToAction("Index");
            }

            // Find the equipment the ninja wants to sell by its ID
            var equipmentToRemove = ninja.NinjaEquipments.FirstOrDefault(ne => ne.EquipmentId == eid);

            // If the ninja has this equipment, process the sale
            if (equipmentToRemove != null)
            {
                // Increase the ninja's gold by the equipment's gold value
                ninja.Gold += equipmentToRemove.Equipment.GoldValue;

                // Remove the equipment from the ninja's list of equipment
                ninja.NinjaEquipments.Remove(equipmentToRemove);

                // Save the changes to the database
                await dbContext.SaveChangesAsync();
                TempData["Message"] = "Equipment successfully sold.";
            }
            else
            {
                // If the ninja does not have this equipment, inform the user
                TempData["Message"] = "Ninja does not have this equipment.";
            }

            return RedirectToAction("Index");
        }
    }
}
