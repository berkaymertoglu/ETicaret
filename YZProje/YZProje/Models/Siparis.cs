using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace YZProje.Models
{
    public class Siparis
    {
        public int SiparisID { get; set; }       // Siparişin benzersiz kimliği (Primary Key)
        public int UrunID { get; set; }          // Sipariş edilen ürünün ID'si (Foreign Key)
        public string UrunAdi { get; set; }      // Ürünün adı
        public string Aciklama { get; set; }     // Ürünün açıklaması
        public int Miktar { get; set; }          // Ürün miktarı
        public decimal Fiyat { get; set; }       // Ürünün fiyatı
        public string FotoUrl { get; set; }      // Ürünün fotoğrafının URL'si
        public string KargoDurumu { get; set; }  // Ürünün kargo durumu (Varsayılan: Hazırlanıyor)
        public int KullaniciID { get; set; }     // Siparişi veren kullanıcının ID'si
        public DateTime SiparisTarihi { get; set; } // Siparişin verildiği tarih
    }
}
