
using System.ComponentModel.DataAnnotations;


namespace YZProje.Models
{
    public class Kullanici
    {
        [Key]
        public int KullaniciID { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
        
    }
}
