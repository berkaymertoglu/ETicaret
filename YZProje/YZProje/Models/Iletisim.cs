using System.ComponentModel.DataAnnotations;

namespace YZProje.Models
{
    public class Iletisim
    {
        public int Id { get; set; }

        public string Ad { get; set; }

        public string Eposta { get; set; }

        public string Mesaj { get; set; }

        public DateTime GonderimTarihi { get; set; } = DateTime.Now;
    }
}
