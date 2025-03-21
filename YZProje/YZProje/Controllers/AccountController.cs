using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using YZProje.Models;

public class AccountController : Controller
{
    private readonly ETicaretDbContext _context;
    private readonly EncryptionService _encryptionService;

    public AccountController(ETicaretDbContext context, EncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    // Giriş formu
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Kullanici.SingleOrDefault(u => u.Email == email);

        if (user == null)
        {
            ViewBag.ErrorMessage = "Geçersiz e-posta veya şifre.";
            return View();
        }

        // Şifreyi deşifre et
        string decryptedPassword = _encryptionService.Decrypt(user.Sifre);

        if (password == decryptedPassword)
        {
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Ad); 

            TempData["Message"] = "Başarıyla giriş yaptınız!";
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.ErrorMessage = "Geçersiz e-posta veya şifre.";
            return View();
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); 
        return RedirectToAction("Login"); 
    }

    public IActionResult Register(string ad, string soyad, string email, string sifre)
    {
        if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad) ||
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(sifre))
        {
            ViewBag.ErrorMessage = "Tüm alanları doldurmanız gerekmektedir!";
            return View();
        }

        if (_context.Kullanici.Any(u => u.Email == email))
        {
            ViewBag.ErrorMessage = "Bu e-posta adresi zaten kayıtlı!";
            return View();
        }

        // Şifreyi şifrele
        string encryptedPassword = _encryptionService.Encrypt(sifre);

        var yeniKullanici = new Kullanici
        {
            Ad = ad,
            Soyad = soyad,
            Email = email,
            Sifre = encryptedPassword
        };

        _context.Kullanici.Add(yeniKullanici);
        _context.SaveChanges();

        TempData["Success"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
        return View();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ForgotPassword(string Email)
    {
        if (string.IsNullOrEmpty(Email))
        {
            ViewBag.Message = "Lütfen geçerli bir e-posta adresi girin.";
            return View();
        }

        var user = _context.Kullanici.FirstOrDefault(u => u.Email == Email);

        if (user == null)
        {
            ViewBag.Message = "Bu e-posta adresi ile kayıtlı bir hesap bulunamadı.";
            return View();
        }

        string decryptedPassword = _encryptionService.Decrypt(user.Sifre);

        var subject = "Şifre Sıfırlama Talebi";
        var body = $"Merhaba {user.Ad},\n\nŞifreniz: {decryptedPassword}\n\nGüvenliğiniz için giriş yaptıktan sonra şifrenizi değiştirmenizi öneririz.";

        bool mailSent = SendEmail(Email, subject, body);

        if (mailSent)
        {
            ViewBag.SuccessMessage = "Şifreniz e-posta adresinize gönderildi.";
        }
        else
        {
            ViewBag.Message = "E-posta gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
        }

        return View();
    }

    private bool SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("ypyproje@gmail.com", "vvvi eold wqos qwdh"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("ypyproje@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);
            smtpClient.Send(mailMessage);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Mail Gönderme Hatası: " + ex.Message);
            return false;
        }
    }
}
