using IkinciElPlatform.Data;
using IkinciElPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")] // ✅ Sadece ADMIN girebilir
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ Admin Ana Sayfa
    public IActionResult Index()
    {
        return View();
    }

    // ✅ KATEGORİ LİSTESİ
    public IActionResult Categories()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    // ✅ KATEGORİ EKLE (GET)
    public IActionResult CreateCategory()
    {
        return View();
    }

    // ✅ KATEGORİ EKLE (POST)
    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        }

        return View(category);
    }

    // ✅ KATEGORİ SİL
    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.FirstOrDefault(x => x.Id == id);

        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        return RedirectToAction("Categories");
    }
    // ✅ ADMİN → TÜM İLANLARI LİSTELE
    public IActionResult Products()
    {
        var products = _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.Id)
            .ToList();

        return View(products);
    }

    // ✅ ADMİN → İLAN SİL
    public IActionResult DeleteProduct(int id)
    {
        var product = _context.Products.FirstOrDefault(x => x.Id == id);

        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        return RedirectToAction("Products");
    }

    // ✅ ADMİN → İLAN AKTİF/PASİF
    public IActionResult ToggleProductStatus(int id)
    {
        var product = _context.Products.FirstOrDefault(x => x.Id == id);

        if (product != null)
        {
            product.IsActive = !product.IsActive; // true ↔ false
            _context.SaveChanges();
        }

        return RedirectToAction("Products");
    }

}

