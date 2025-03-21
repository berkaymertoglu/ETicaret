
using System.ComponentModel.DataAnnotations;



namespace YZProje.Models
{
    public class Urun
    {
        [Key]
        public int UrunID { get; set; }

        public string? Ad { get; set; }

        public string? Aciklama { get; set; }

        public decimal Fiyat { get; set; }

        public int StokMiktari { get; set; }

        public int KategoriID { get; set; }

        public string? FotoUrl { get; set; } 
    }
}

