using System;
using KrediHesaplama.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace KrediHesaplama.Data
{
    public class KrediDbContext : DbContext
    {
        public KrediDbContext(DbContextOptions<KrediDbContext> options)
        : base(options)
        {
        }

        // Ana veri tabloları
        public DbSet<User> Users { get; set; }
        public DbSet<KrediUrun> KrediUrunleri { get; set; }
        public DbSet<KrediBasvuru> KrediBasvurulari { get; set; }
         public DbSet<KrediHesaplamaKaydi> KrediHesaplamaKayitlari { get; set; }

        // Log tabloları
        public DbSet<KrediHesaplamaLog> HesaplamaLoglari { get; set; }
        public DbSet<KrediBasvuruLog> BasvuruLoglari { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<SignupLog> SignupLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Model oluşturma işlemleri
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KrediBasvuru>()
                .HasKey(k => k.basvuruId);

            modelBuilder.Entity<User>()
                .HasIndex(m => m.TCKN)
                .IsUnique();

            // KrediUrun tablosu için seed veri ekliyoruz
            modelBuilder.Entity<KrediUrun>().HasData(
                new KrediUrun
                {
                    CreditTypeId = 1,
                    urunAdi = "İhtiyaç",
                    faizOrani = 4.99,
                    minTutar = 10000,
                    maxTutar = 100000,
                    minVade=3,
                    maxVade=36
                },
                new KrediUrun
                {
                    CreditTypeId = 2,
                    urunAdi = "Konut",
                    faizOrani = 2.99,
                    minTutar = 300000,
                    maxTutar = 4000000,
                    minVade=3,
                    maxVade = 120
                },
                new KrediUrun
                {
                    CreditTypeId = 3,
                    urunAdi = "Taşıt",
                    faizOrani = 3.99,
                    minTutar = 100000,
                    maxTutar = 1000000,
                    minVade = 3,
                    maxVade = 60
                }
            );
        }
    }

}
