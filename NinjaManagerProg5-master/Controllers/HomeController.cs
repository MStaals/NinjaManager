using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NinjaManagerProg5.DbAccess;
using NinjaManagerProg5.Models;
using NinjaManagerProg5.ViewModels;

namespace NinjaManagerProg5.Controllers;

public class HomeController(ILogger<HomeController> logger, MyNinjaDbContext dbContext) : Controller
{
    
    public async Task<IActionResult> Index()
    {
        // Retrieve all ninja's
        List<Ninja> ninjalist = await dbContext.Ninja
            .Include(n => n.NinjaEquipments) // Zorg ervoor dat de NinjaEquipments ook worden opgehaald
            .ThenInclude(ne => ne.Equipment) // En de bijbehorende Equipment
            .OrderBy(n => n.Name)
            .ToListAsync();

        // Store the list of ninjas in ViewBag for use in the view
        ViewBag.NinjaList = ninjalist;

        // Retrieve all available equipment, ordered by name
        List<Equipment> equipmentList = await dbContext.Equipments
            .OrderBy(e => e.Name)
            .ToListAsync();

        // Create a NinjaVM for each ninja, associating it with the equipment list
        var ninjaVMList = ninjalist.Select(ninja => new NinjaVM(ninja, ninja.NinjaEquipments.Select(ne => ne.Equipment).ToList())).ToList();
        ViewBag.NinjaVMList = ninjaVMList;

        return View(ninjaVMList);
    }
    

    // Error handling for unexpected issues in the application
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // Display the error view, including the request ID for debugging purposes
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
