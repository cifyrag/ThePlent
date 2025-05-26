using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThePlant.UI.Models;
using ThePlant.EF;
using System;
using ThePlant.EF.Models;

namespace ThePlant.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ThePlant.EF.ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ThePlant.EF.ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index(string searchTerm, string category, int? careFrequency)
    {
        var categories = _context.Plants.Select(p => p.Category).Distinct().ToList();

        var model = new PlantFilterViewModel
        {
            SearchTerm = searchTerm,
            CurrentCategory = category,
            CurrentCareFrequency = careFrequency,
            Categories = categories
        };


        var plantsQuery = _context.Plants.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            plantsQuery = plantsQuery.Where(p => p.PlantName.Contains(searchTerm));
        }

        if (!string.IsNullOrEmpty(category))
        {
            plantsQuery = plantsQuery.Where(p => p.Category == category);
        }

        if (careFrequency.HasValue)
        {
            plantsQuery = plantsQuery.Where(p => p.PlantCareInstructions.Any(c => c.FrequencyRecommended == careFrequency.Value));
        }

        var plants = plantsQuery.ToList();

        ViewData["FilterModel"] = model;

        return View(plants);
    }

    public IActionResult Details(Guid id)
    {
        var plant = _context.Plants
            .Include(p => p.PlantCareInstructions)
            .Include(p => p.PlantOverviews)
            .Include(p => p.PlantImages)
            .FirstOrDefault(p => p.PlantId == id);

        if (plant == null)
            return NotFound();

        return View(plant);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
