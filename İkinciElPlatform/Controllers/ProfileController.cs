using IkinciElPlatform.Data;
using IkinciElPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IkinciElPlatform.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ PROFİL ANA SAYFASI
        public IActionResult Index()

        {
            return View();
        }

        // ✅ KULLANICININ KENDİ ÜRÜNLERİ
        public IActionResult MyProducts()
        {
            var userId = _userManager.GetUserId(User);
            var myProducts = _context.Products
                .Include(p => p.Category)
                .Where(p => p.UserId != null && p.UserId == userId)
                .ToList();


            return View(myProducts);
        }

        // ✅ ÜRÜN EKLE (GET) → SADECE PROFİLDEN
        public IActionResult CreateProduct()
        {
            ViewBag.CategoryList = _context.Categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(Product product, IFormFile imageFile)
        {
            

            product.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            product.SellerName = User.Identity.Name;
            product.CreatedDate = DateTime.Now;
            product.IsActive = true;

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = _context.Categories
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();

                return View(product);
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("MyProducts");
        }





        [Authorize]
        public IActionResult SoldProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var soldProducts = _context.Purchases
                .Where(x => x.SellerId == userId)
                .Include(x => x.Product)
                .Select(x => x.Product)
                .ToList();

            return View(soldProducts);
        }
        [Authorize]
        public IActionResult MyPurchases()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            var purchases = _context.Purchases
                .Where(x => x.BuyerId == userId)
                .Include(x => x.Product)
                .OrderByDescending(x => x.PurchaseDate)
                .ToList();

            return View(purchases);
        }


    }
}
