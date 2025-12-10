using IkinciElPlatform.Data;
using IkinciElPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IkinciElPlatform.Controllers
{
    [Authorize] // ✅ Sadece giriş yapmış kullanıcılar favori kullanabilir
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FavoriteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ FAVORİ EKLE / ÇIKAR (TOGGLE)
        public async Task<IActionResult> Toggle(int productId, string returnUrl = null)
        {
            var userId = _userManager.GetUserId(User);

            var fav = await _context.Favorites
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

            if (fav == null)
            {
                _context.Favorites.Add(new Favorite
                {
                    UserId = userId,
                    ProductId = productId
                });

                TempData["FavoriteMessage"] = "✅ Ürün favorilere eklendi";
            }
            else
            {
                _context.Favorites.Remove(fav);
                TempData["FavoriteMessage"] = "❌ Ürün favorilerden çıkarıldı";
            }

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Detail", "Product", new { id = productId });
        }


        // ✅ FAVORİLERİM SAYFASI
        public async Task<IActionResult> MyFavorites()
        {
            var userId = _userManager.GetUserId(User);

            var products = await _context.Favorites
                .Where(x => x.UserId == userId)
                .Include(x => x.Product)
                    .ThenInclude(p => p.Category)
                .Select(x => x.Product)
                .ToListAsync();

            return View(products);
        }

    }
}
