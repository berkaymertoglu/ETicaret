using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using YZProje.Models;

namespace YZProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly ETicaretDbContext _context;

        public HomeController(ETicaretDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var urunler = _context.Urun.ToList();
            return View(urunler);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(Iletisim model)
        {
            if (ModelState.IsValid)
            {
                _context.Iletisim.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Mesajýnýz baþarýyla gönderildi.";
                return RedirectToAction("Contact");
            }
            return View(model);
        }

        public IActionResult Knowledge()
        {
            return View();
        }
    }
}