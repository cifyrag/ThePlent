using Microsoft.AspNetCore.Mvc;
using ThePlent.API.Models;
using ThePlent.API.DTOs;

namespace ThePlent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdsController : ControllerBase
{
    private static List<Ad> _ads = new();
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Ad>> GetAll() => Ok(_ads);

    [HttpGet("{id}")]
    public ActionResult<Ad> Get(int id)
    {
        var ad = _ads.FirstOrDefault(a => a.Id == id);
        return ad is null ? NotFound() : Ok(ad);
    }

    [HttpPost]
    public ActionResult<Ad> Create([FromBody] AdDto dto)
    {
        var ad = new Ad { Id = _nextId++, Title = dto.Title, Description = dto.Description };
        _ads.Add(ad);
        return CreatedAtAction(nameof(Get), new { id = ad.Id }, ad);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] AdDto dto)
    {
        var ad = _ads.FirstOrDefault(a => a.Id == id);
        if (ad is null) return NotFound();
        ad.Title = dto.Title;
        ad.Description = dto.Description;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var ad = _ads.FirstOrDefault(a => a.Id == id);
        if (ad is null) return NotFound();
        _ads.Remove(ad);
        return NoContent();
    }
}
