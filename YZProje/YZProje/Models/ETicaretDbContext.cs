using Microsoft.EntityFrameworkCore;


namespace YZProje.Models
{
    public class ETicaretDbContext : DbContext
    {
        public ETicaretDbContext(DbContextOptions<ETicaretDbContext> options) : base(options) { }

        public DbSet<Kullanici> Kullanici { get; set; }
        public DbSet<Urun> Urun { get; set; }
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<Siparis> Siparis { get; set; }
        public DbSet<Sepet> Sepet { get; set; } 
        public DbSet<Iletisim> Iletisim { get; set; }
        public DbSet<Admin> Admin { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API ile ilişkileri belirtebilirsin
        }
    }
}
