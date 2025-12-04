using IkinciElPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products
            .OrderByDescending(p => p.CreatedDate)
            .Take(6)
            .ToList();

        var categories = _context.Categories.ToList();

        ViewBag.Categories = categories;

        return View(products);
    }


}
