using IkinciElPlatform.Data;
using IkinciElPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IkinciElPlatform.Models;

namespace IkinciElPlatform.Controllers
{
    [Authorize] // ✅ Giriş yapmadan kategoriye girilemez
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ KATEGORİ LİSTESİ
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // ✅ KATEGORİ EKLE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // ✅ KATEGORİ EKLE (POST)
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // ✅ KATEGORİ SİL
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
