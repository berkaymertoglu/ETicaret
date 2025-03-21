using System.ComponentModel.DataAnnotations;

namespace YZProje.Models
{
    public class Admin
    {
        public int Id { get; set; }

        public string KullaniciAdi { get; set; }

        public string Sifre { get; set; }
    }
}
