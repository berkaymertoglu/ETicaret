
using System.ComponentModel.DataAnnotations;



namespace YZProje.Models
{
    public class Kategori
    {
        [Key]
        public int KategoriID { get; set; }

        public string Ad { get; set; }

        public string Aciklama { get; set; }

        public ICollection<Urun> Urunler { get; set; }
    }
}
