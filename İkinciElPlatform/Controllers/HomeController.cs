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
        // ✅ KATEGORİLER (BU YOKTUĞU İÇİN HATA VERİYORDU)
        var categories = _context.Categories.ToList();
        ViewBag.Categories = categories;

        // ✅ SON EKLENEN ÜRÜNLER
        var products = _context.Products
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedDate)
            .Take(6)
            .ToList();

        // 🔥 EN ÇOK FAVORİLENEN ÜRÜNLER
        var mostFavorited = _context.Favorites
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(6)
            .Join(_context.Products,
                  fav => fav.ProductId,
                  product => product.Id,
                  (fav, product) => product)
            .ToList();

        ViewBag.MostFavorited = mostFavorited;

        return View(products);
    }




}
