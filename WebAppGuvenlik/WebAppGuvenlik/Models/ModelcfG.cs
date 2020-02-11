namespace WebAppGuvenlik.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelcfG : DbContext
    {
        public ModelcfG()
            : base("name=ModelcfG")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Guvenlik> Guvenlik { get; set; }
        public virtual DbSet<Kullanıcı> Kullanıcı { get; set; }
        public virtual DbSet<OnayDurum> OnayDurum { get; set; }
        public virtual DbSet<Personel> Personel { get; set; }
        public virtual DbSet<Ziyaretci> Ziyaretci { get; set; }
        public virtual DbSet<deneme> deneme { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Admin>()
            //    .Property(e => e.Ad)
            //    .IsFixedLength();

            //modelBuilder.Entity<Admin>()
            //    .Property(e => e.Soyad)
            //    .IsFixedLength();

            //modelBuilder.Entity<Admin>()
            //    .Property(e => e.Tel)
            //    .IsFixedLength();

            //modelBuilder.Entity<Admin>()
            //    .Property(e => e.Gorev)
            //    .IsFixedLength();

            //modelBuilder.Entity<Admin>()
            //    .HasOptional(e => e.Kullanıcı)
            //    .WithRequired(e => e.Admin);

            //modelBuilder.Entity<Guvenlik>()
            //    .Property(e => e.Ad)
            //    .IsFixedLength();

            //modelBuilder.Entity<Guvenlik>()
            //    .Property(e => e.Soyad)
            //    .IsFixedLength();

            //modelBuilder.Entity<Guvenlik>()
            //    .HasOptional(e => e.Kullanıcı)
            //    .WithRequired(e => e.Guvenlik);

            //modelBuilder.Entity<Kullanıcı>()
            //    .Property(e => e.KullaniciAd)
            //    .IsFixedLength();

            //modelBuilder.Entity<Kullanıcı>()
            //    .Property(e => e.Sifre)
            //    .IsFixedLength();

            //modelBuilder.Entity<Kullanıcı>()
            //    .Property(e => e.Tur)
            //    .IsFixedLength();

            //modelBuilder.Entity<OnayDurum>()
            //    .Property(e => e.Adı)
            //    .IsFixedLength();

            //modelBuilder.Entity<OnayDurum>()
            //    .Property(e => e.Aciklama)
            //    .IsFixedLength();

            //modelBuilder.Entity<Personel>()
            //    .Property(e => e.Ad)
            //    .IsFixedLength();

            //modelBuilder.Entity<Personel>()
            //    .Property(e => e.Soyad)
            //    .IsFixedLength();

            //modelBuilder.Entity<Personel>()
            //    .Property(e => e.Mail)
            //    .IsFixedLength();

            //modelBuilder.Entity<Personel>()
            //    .Property(e => e.Tur)
            //    .IsFixedLength();

            //modelBuilder.Entity<Personel>()
            //    .HasOptional(e => e.Kullanıcı)
            //    .WithRequired(e => e.Personel);

            //modelBuilder.Entity<Ziyaretci>()
            //    .Property(e => e.Ad)
            //    .IsFixedLength();

            //modelBuilder.Entity<Ziyaretci>()
            //    .Property(e => e.Soyad)
            //    .IsFixedLength();

            //modelBuilder.Entity<Ziyaretci>()
            //    .Property(e => e.ZiyaretSebebi)
            //    .IsFixedLength();


        }
    }
}
