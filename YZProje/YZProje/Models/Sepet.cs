namespace YZProje.Models
{
    public class Sepet
    {
        public int SepetID { get; set; } // SepetID, otomatik artan birincil anahtar
        public int UrunID { get; set; }  // UrunID, sepetteki ürünün ID'si
        public int KullaniciID { get; set; }  // Kullanıcıya özel sepet
        public int Miktar { get; set; }  // Ürünün sepetteki miktarı

        public virtual Urun Urun { get; set; }  // Urun ile ilişki
        public virtual Kullanici Kullanici { get; set; }  // Kullanici ile ilişki
    }
}
