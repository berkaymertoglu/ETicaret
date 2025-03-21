using Microsoft.AspNetCore.Mvc;
using YZProje.Models;


namespace YZProje.Controllers
{
    public class UrunController : Controller
    {
        private readonly ETicaretDbContext _context;

        public UrunController(ETicaretDbContext context)
        {
            _context = context;
        }

        public IActionResult Details(int id)
        {
            var urun = _context.Urun.FirstOrDefault(u => u.UrunID == id);
            if (urun == null)
            {
                return NotFound();
            }
            return View(urun);
        }
    }
}
