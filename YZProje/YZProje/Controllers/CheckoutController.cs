using Microsoft.AspNetCore.Mvc;
using YZProje.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class CheckoutController : Controller
{
    private readonly ETicaretDbContext _context;

    public CheckoutController(ETicaretDbContext context)
    {
        _context = context;
    }

    // Sepeti tamamla sayfasını gösteren action
    public IActionResult Index()
    {
        var userId = 1; // Kullanıcı ID'sini dinamik almanız gerekebilir

        // Siparişler tablosundan, ilgili kullanıcıya ait siparişleri çekiyoruz
        var siparisler = _context.Siparis
                                .Where(s => s.KullaniciID == userId) // Kullanıcıyı filtreliyoruz
                                .ToList();

        if (siparisler.Any())
        {
            // Siparişlerdeki toplam fiyatı hesaplamak için toplamları alıyoruz
            decimal toplamFiyat = siparisler.Sum(s => s.Miktar * s.Fiyat);
            ViewBag.ToplamFiyat = toplamFiyat;

            return View(siparisler);  // Siparişleri View'a gönderiyoruz
        }
        else
        {
            ViewBag.Message = "Henüz siparişiniz bulunmamaktadır.";
            return View();  // Sipariş bulunamadığında mesaj gösteriyoruz
        }
    }

    // Sepeti tamamladıktan sonra, sepetteki ürünleri silip, alışverişi tamamla
    [HttpPost]
    public IActionResult CompletePurchase()
    {
        var userId = 1; // Kullanıcı ID'sini dinamik almanız gerekebilir
        var sepet = _context.Sepet
                            .Where(s => s.KullaniciID == userId)
                            .Include(s => s.Urun)  // Urun bilgilerini de dahil ediyoruz
                            .ToList();

        // Sepetteki her bir ürünü Siparisler tablosuna ekliyoruz
        foreach (var sepetUrun in sepet)
        {
            // Ürün stok miktarını kontrol et
            var urun = _context.Urun.FirstOrDefault(u => u.UrunID == sepetUrun.UrunID);

            if (urun != null && urun.StokMiktari >= sepetUrun.Miktar)
            {
                // Sipariş oluştur
                var siparis = new Siparis
                {
                    UrunID = sepetUrun.UrunID,
                    UrunAdi = sepetUrun.Urun.Ad,
                    Aciklama = sepetUrun.Urun.Aciklama,
                    Miktar = sepetUrun.Miktar,
                    Fiyat = sepetUrun.Urun.Fiyat,
                    FotoUrl = sepetUrun.Urun.FotoUrl,
                    KargoDurumu = "Hazırlanıyor",  // Varsayılan kargo durumu
                    KullaniciID = userId,
                    SiparisTarihi = DateTime.Now
                };

                _context.Siparis.Add(siparis);  // Siparişi veritabanına ekliyoruz

                // Ürünün stok miktarını güncelle
                urun.StokMiktari -= sepetUrun.Miktar;
                _context.Urun.Update(urun);  // Ürün tablosunu güncelliyoruz
            }
            else
            {
                // Eğer stok yeterli değilse hata mesajı dönebiliriz
                TempData["Error"] = $"Ürün {sepetUrun.Urun.Ad} için yeterli stok yok!";
                return RedirectToAction("Index", "Sepet");
            }
        }

        // Sepetten ürünleri siliyoruz
        _context.Sepet.RemoveRange(sepet);
        _context.SaveChanges();  // Değişiklikleri kaydediyoruz

        TempData["Success"] = "Alışveriş başarıyla tamamlandı!";
        return RedirectToAction("Index", "Checkout");  // Kullanıcıyı başka bir sayfaya yönlendiriyoruz
    }


}
