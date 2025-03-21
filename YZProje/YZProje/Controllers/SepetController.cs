using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YZProje.Models;

namespace YZProje.Controllers
{
    public class SepetController : Controller
    {
        private readonly ETicaretDbContext _context;

        public SepetController(ETicaretDbContext context)
        {
            _context = context;
        }

        // Sepet sayfasını görüntüleme
        public IActionResult Index()
        {
            // Kullanıcının sepetindeki tüm ürünleri getiriyoruz
            var sepet = _context.Sepet.Include(s => s.Urun).Where(s => s.KullaniciID == 1).ToList();
            return View(sepet);
        }

        // Sepete ürün ekleme
        [HttpPost]
        public IActionResult AddToCart(int urunID, int miktar)
        {
            var kullaniciID = 1;

            // Ürünün sepete daha önce eklenip eklenmediğini kontrol et
            var mevcutUrun = _context.Sepet.FirstOrDefault(s => s.UrunID == urunID && s.KullaniciID == kullaniciID);

            if (mevcutUrun != null)
            {
                // Eğer varsa, miktarını artır
                mevcutUrun.Miktar += miktar;
            }
            else
            {
                // Eğer yoksa, yeni bir sepet kaydı oluştur
                var yeniUrun = new Sepet
                {
                    UrunID = urunID,
                    KullaniciID = kullaniciID,
                    Miktar = miktar,
                };
                _context.Sepet.Add(yeniUrun);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // Sepetten ürün çıkarma
        public IActionResult RemoveFromCart(int sepetID, int urunID)
        {
            // Kullanıcı ID'sini al
            var kullaniciID = 1; // Bu kısmı oturum bilgisiyle değiştirin

            // Sepetteki ürünü bul
            var sepetUrun = _context.Sepet
                                    .FirstOrDefault(s => s.SepetID == sepetID && s.UrunID == urunID && s.KullaniciID == kullaniciID);

            if (sepetUrun != null)
            {
                // Miktarı 1 azalt
                if (sepetUrun.Miktar > 1)
                {
                    sepetUrun.Miktar -= 1;
                    _context.SaveChanges();
                }
                else
                {
                    // Miktar 1'e eşitse, ürünü tamamen sepetten çıkar
                    _context.Sepet.Remove(sepetUrun);
                    _context.SaveChanges();
                }
            }

            TempData["SuccessMessage"] = "Ürün sepete başarıyla eklendi.";

            
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult UpdateQuantity(int sepetID, int yeniMiktar)
        {
            var sepetUrun = _context.Sepet.FirstOrDefault(s => s.SepetID == sepetID);
            if (sepetUrun != null)
            {
                sepetUrun.Miktar = yeniMiktar;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }

}
