using Microsoft.AspNetCore.Mvc;
using YZProje.Models;
using System.Linq;

namespace YZProje.Controllers
{
    public class AdminController : Controller
    {
        private readonly ETicaretDbContext _context;

        public AdminController(ETicaretDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string KullaniciAdi, string Sifre)
        {
            EncryptionService encryptionService = new EncryptionService();
            string encryptedPassword = encryptionService.Encrypt(Sifre); 

            var admin = _context.Admin.FirstOrDefault(a => a.KullaniciAdi == KullaniciAdi && a.Sifre == encryptedPassword);

            if (admin != null)
            {
                return RedirectToAction("Messages"); 
            }

            ViewBag.Error = "Kullanıcı adı veya şifre yanlış.";
            return View();
        }

        public IActionResult Messages()
        {
            var messages = _context.Iletisim.OrderByDescending(m => m.GonderimTarihi).ToList();
            return View(messages);
        }
    }
}
