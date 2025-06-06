using Microsoft.AspNetCore.Mvc;
using ThePlant.EF.Models;
using ThePlant.EF;
using Microsoft.EntityFrameworkCore;
using System.Net; // For HttpStatusCode
using System.Diagnostics; // Add this for Debug.WriteLine

namespace ThePlant.UI.Controllers
{
    public class PlantController : Controller
    {
        private readonly ThePlant.EF.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment; // Keep if you still want file uploads for other scenarios or edit

        public PlantController(ThePlant.EF.ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var plants = await _context.Plants
                .Include(p => p.PlantImages)
                .Include(p => p.PlantCareInstructions)
                .Include(p => p.PlantOverviews)
                .ToListAsync();

            foreach (var plant in plants)
            {
                Debug.WriteLine($"Plant '{plant.PlantName}' loaded with ID: {plant.PlantId}");
            }

            return View(plants);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var newPlant = new Plant
            {
                PlantCareInstructions = new List<PlantCareInstruction>(),
                PlantOverviews = new List<PlantOverview>()
            };
            return View(newPlant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("PlantName,Category,ScientificTitle")] Plant plant,
            string? imageUrl, // Changed from IFormFile to string
            List<PlantCareInstruction>? plantCareInstructions,
            List<PlantOverview>? plantOverviews)
        {
            Debug.WriteLine("--- Entering Create (POST) method ---");
            Debug.WriteLine($"Submitted Plant Name: {plant.PlantName}");
            Debug.WriteLine($"Submitted imageUrl: {imageUrl}"); // Debugging the URL
            Debug.WriteLine($"Submitted PlantCareInstructions count: {plantCareInstructions?.Count ?? 0}");
            Debug.WriteLine($"Submitted PlantOverviews count: {plantOverviews?.Count ?? 0}");

            if (ModelState.IsValid)
            {
                // Handle image URL
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    // You might want to add validation here to ensure it's a valid URL format
                    plant.PlantImages = new List<PlantImage>
                    {
                        new PlantImage { URL = imageUrl } // Store the URL directly
                    };
                    Debug.WriteLine($"Primary image URL set: {imageUrl}");
                }
                // If you want to allow no image, don't add an else block for null/empty URL
                // If an image is required, add ModelState.AddModelError here:
                // else
                // {
                //     ModelState.AddModelError("imageUrl", "Please provide an image URL for the plant.");
                //     // Repopulate dynamic lists for view
                //     plant.PlantCareInstructions = plantCareInstructions ?? new List<PlantCareInstruction>();
                //     plant.PlantOverviews = plantOverviews ?? new List<PlantOverview>();
                //     return View(plant);
                // }


                // Add care instructions
                if (plantCareInstructions != null)
                {
                    plant.PlantCareInstructions = new List<PlantCareInstruction>();
                    foreach (var instruction in plantCareInstructions)
                    {
                        instruction.PlantCareInstructionId = Guid.NewGuid();
                        plant.PlantCareInstructions.Add(instruction);
                        Debug.WriteLine($"Adding care instruction: {instruction.Description}");
                    }
                }

                // Add overviews
                if (plantOverviews != null)
                {
                    plant.PlantOverviews = new List<PlantOverview>();
                    foreach (var overview in plantOverviews)
                    {
                        overview.PlantOverviewId = Guid.NewGuid();
                        plant.PlantOverviews.Add(overview);
                        Debug.WriteLine($"Adding overview: {overview.OverviewType} - {overview.Description}");
                    }
                }

                _context.Add(plant);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plant created successfully!";
                Debug.WriteLine("Plant successfully created and saved to DB.");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Debug.WriteLine("ModelState is NOT valid. Returning to View with errors.");
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                    }
                }
                // Repopulate dynamic lists to retain user input on validation failure
                plant.PlantCareInstructions = plantCareInstructions ?? new List<PlantCareInstruction>();
                plant.PlantOverviews = plantOverviews ?? new List<PlantOverview>();
                return View(plant);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants
                .Include(p => p.PlantImages)
                .Include(p => p.PlantCareInstructions)
                .Include(p => p.PlantOverviews)
                .FirstOrDefaultAsync(p => p.PlantId == id);

            if (plant == null)
            {
                return NotFound();
            }
            return View(plant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Plant plant, string? newImageUrl, // Keep newImageUrl for editing
                                              List<PlantCareInstruction>? plantCareInstructions,
                                              List<PlantOverview>? plantOverviews)
        {
            Debug.WriteLine("--- Entering Edit (POST) method ---");
            Debug.WriteLine($"Plant ID: {id}");
            Debug.WriteLine($"Submitted Plant Name: {plant.PlantName}");
            Debug.WriteLine($"Submitted newImageUrl: {newImageUrl}");
            Debug.WriteLine($"Submitted PlantCareInstructions count: {plantCareInstructions?.Count ?? 0}");
            Debug.WriteLine($"Submitted PlantOverviews count: {plantOverviews?.Count ?? 0}");


            if (id != plant.PlantId)
            {
                Debug.WriteLine($"Error: ID mismatch! Route ID: {id}, Plant Object ID: {plant.PlantId}");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("ModelState is NOT valid. Returning to View with errors.");
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                    }
                }

                var existingPlantForView = await _context.Plants
                    .Include(p => p.PlantImages)
                    .Include(p => p.PlantCareInstructions)
                    .Include(p => p.PlantOverviews)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PlantId == id);

                if (existingPlantForView != null)
                {
                    existingPlantForView.PlantCareInstructions = plantCareInstructions ?? new List<PlantCareInstruction>();
                    existingPlantForView.PlantOverviews = plantOverviews ?? new List<PlantOverview>();
                }
                else
                {
                    return NotFound();
                }

                return View(existingPlantForView);
            }

            Debug.WriteLine("ModelState is valid. Proceeding with update logic.");

            var existingPlant = await _context.Plants
                .Include(p => p.PlantImages)
                .Include(p => p.PlantCareInstructions)
                .Include(p => p.PlantOverviews)
                .FirstOrDefaultAsync(p => p.PlantId == id);

            if (existingPlant == null)
            {
                Debug.WriteLine($"Error: Existing plant with ID {id} not found in DB.");
                return NotFound();
            }

            existingPlant.PlantName = plant.PlantName;
            existingPlant.Category = plant.Category;
            existingPlant.ScientificTitle = plant.ScientificTitle;

            // Handle image URL update
            if (!string.IsNullOrEmpty(newImageUrl))
            {
                Debug.WriteLine($"Handling new image URL: {newImageUrl}");
                var existingImage = existingPlant.PlantImages.FirstOrDefault();
                if (existingImage != null)
                {
                    Debug.WriteLine($"Updating existing image URL from '{existingImage.URL}' to '{newImageUrl}'");
                    existingImage.URL = newImageUrl;
                }
                else
                {
                    Debug.WriteLine($"Adding new image with URL: {newImageUrl}");
                    existingPlant.PlantImages.Add(new PlantImage { URL = newImageUrl, PlantId = id });
                }
            }
            else
            {
                // If newImageUrl is empty, you might want to remove the existing image
                // or keep it as is based on your requirements.
                // For now, it will keep the existing image if newImageUrl is null/empty.
                Debug.WriteLine("newImageUrl is empty or null. No image URL update.");
            }

            // Update PlantCareInstructions
            Debug.WriteLine("--- Updating PlantCareInstructions ---");
            Debug.WriteLine($"Existing PlantCareInstructions count: {existingPlant.PlantCareInstructions?.Count ?? 0}");

            var currentCareInstructions = existingPlant.PlantCareInstructions.ToList();
            if (plantCareInstructions == null) plantCareInstructions = new List<PlantCareInstruction>();

            // Remove instructions that are no longer in the submitted list
            foreach (var existingPci in currentCareInstructions)
            {
                if (!plantCareInstructions.Any(pci => pci.PlantCareInstructionId == existingPci.PlantCareInstructionId && pci.PlantCareInstructionId != Guid.Empty))
                {
                    Debug.WriteLine($"  - Removing PlantCareInstruction ID: {existingPci.PlantCareInstructionId}, Desc: '{existingPci.Description}'");
                    _context.PlantCareInstructions.Remove(existingPci);
                }
            }

            // Add or update instructions
            foreach (var newPci in plantCareInstructions)
            {
                var existingPci = existingPlant.PlantCareInstructions.FirstOrDefault(pci => pci.PlantCareInstructionId == newPci.PlantCareInstructionId && pci.PlantCareInstructionId != Guid.Empty);
                if (existingPci != null)
                {
                    // Update existing
                    Debug.WriteLine($"  - Updating existing PlantCareInstruction ID: {existingPci.PlantCareInstructionId}");
                    existingPci.Description = newPci.Description;
                    existingPci.FrequencyRecommended = newPci.FrequencyRecommended;
                    existingPci.Note = newPci.Note;
                }
                else
                {
                    // Add new (ensure PlantId is set)
                    Debug.WriteLine($"  - Adding new PlantCareInstruction. Description: '{newPci.Description}'");
                    newPci.PlantId = id;
                    newPci.PlantCareInstructionId = Guid.NewGuid(); // Ensure a new ID for new entries
                    existingPlant.PlantCareInstructions.Add(newPci);
                }
            }

            // Update PlantOverviews
            Debug.WriteLine("--- Updating PlantOverviews ---");
            Debug.WriteLine($"Existing PlantOverviews count: {existingPlant.PlantOverviews?.Count ?? 0}");

            var currentOverviews = existingPlant.PlantOverviews.ToList();
            if (plantOverviews == null) plantOverviews = new List<PlantOverview>();

            // Remove overviews that are no longer in the submitted list
            foreach (var existingPo in currentOverviews)
            {
                if (!plantOverviews.Any(po => po.PlantOverviewId == existingPo.PlantOverviewId && po.PlantOverviewId != Guid.Empty))
                {
                    Debug.WriteLine($"  - Removing PlantOverview ID: {existingPo.PlantOverviewId}, Type: '{existingPo.OverviewType}'");
                    _context.PlantOverviews.Remove(existingPo);
                }
            }

            // Add or update overviews
            foreach (var newPo in plantOverviews)
            {
                var existingPo = existingPlant.PlantOverviews.FirstOrDefault(po => po.PlantOverviewId == newPo.PlantOverviewId && po.PlantOverviewId != Guid.Empty);
                if (existingPo != null)
                {
                    // Update existing
                    Debug.WriteLine($"  - Updating existing PlantOverview ID: {existingPo.PlantOverviewId}");
                    existingPo.OverviewType = newPo.OverviewType;
                    existingPo.Description = newPo.Description;
                }
                else
                {
                    // Add new (ensure PlantId is set)
                    Debug.WriteLine($"  - Adding new PlantOverview. Type: '{newPo.OverviewType}'");
                    newPo.PlantId = id;
                    newPo.PlantOverviewId = Guid.NewGuid(); // Ensure a new ID for new entries
                    existingPlant.PlantOverviews.Add(newPo);
                }
            }

            Debug.WriteLine("--- Entity Framework Change Tracker State ---");
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                Debug.WriteLine($"  Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
                if (entry.State == EntityState.Modified)
                {
                    foreach (var property in entry.Properties)
                    {
                        if (property.IsModified)
                        {
                            Debug.WriteLine($"    Property '{property.Metadata.Name}' modified. Old: '{property.OriginalValue}', New: '{property.CurrentValue}'");
                        }
                    }
                }
            }
            Debug.WriteLine("--- End Change Tracker State ---");


            try
            {
                Debug.WriteLine("Attempting to save changes...");
                var savedChanges = await _context.SaveChangesAsync();
                Debug.WriteLine($"Successfully saved {savedChanges} changes to the database.");
                TempData["SuccessMessage"] = "Plant updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine($"DbUpdateConcurrencyException caught: {ex.Message}");
                if (!PlantExists(id))
                {
                    Debug.WriteLine($"Plant with ID {id} does not exist after concurrency exception.");
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine("Concurrency exception occurred, but plant still exists. Rethrowing.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An unexpected exception occurred during SaveChanges: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving the plant: " + ex.Message);

                var existingPlantForView = await _context.Plants
                    .Include(p => p.PlantImages)
                    .Include(p => p.PlantCareInstructions)
                    .Include(p => p.PlantOverviews)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PlantId == id);

                if (existingPlantForView != null)
                {
                    existingPlantForView.PlantCareInstructions = plantCareInstructions ?? new List<PlantCareInstruction>();
                    existingPlantForView.PlantOverviews = plantOverviews ?? new List<PlantOverview>();
                }
                else
                {
                    return NotFound();
                }
                return View(existingPlantForView);
            }
        }

        private bool PlantExists(Guid id)
        {
            return _context.Plants.Any(e => e.PlantId == id);
        }

        // Helper class for comparing PlantCareInstruction objects
        private class PlantCareInstructionComparer : IEqualityComparer<PlantCareInstruction>
        {
            public bool Equals(PlantCareInstruction? x, PlantCareInstruction? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
                return x.PlantCareInstructionId == y.PlantCareInstructionId && x.PlantCareInstructionId != Guid.Empty;
            }

            public int GetHashCode(PlantCareInstruction obj)
            {
                return obj.PlantCareInstructionId.GetHashCode();
            }
        }

        // Helper class for comparing PlantOverview objects
        private class PlantOverviewComparer : IEqualityComparer<PlantOverview>
        {
            public bool Equals(PlantOverview? x, PlantOverview? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
                return x.PlantOverviewId == y.PlantOverviewId && x.PlantOverviewId != Guid.Empty;
            }

            public int GetHashCode(PlantOverview obj)
            {
                return obj.PlantOverviewId.GetHashCode();
            }
        }
    }
}