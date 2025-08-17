using KrediHesaplama.Data;
using KrediHesaplama.Models;
using KrediHesaplama.Models.Requests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KrediHesaplama.Service
{
    public class KrediService
    {
        private readonly KrediDbContext _context;

        public KrediService(KrediDbContext context)
        {
            // Dependency Injection ile KrediDbContexti alıyoruz
            _context = context;
        }

        public KrediHesaplamaKaydi Hesapla(HesaplamaRequest request)
        {
            // Gelen istekten kredi türü ve tutar bilgilerini alıyoruz
            var urun = _context.KrediUrunleri.FirstOrDefault(k => k.CreditTypeId == request.CreditTypeId);

            if (urun == null)
            {
                return null;
            }

            // Kredi ürününün aylık faiz oranını kullanarak taksit hesaplamasını yapıyoruz
            double aylikFaiz = urun.faizOrani / 12;
            double taksit = request.Tutar * (aylikFaiz * Math.Pow(1 + aylikFaiz, request.Vade)) /
                                             (Math.Pow(1 + aylikFaiz, request.Vade) - 1);

            // Hesaplama kaydını oluşturuyoruz
            return new KrediHesaplamaKaydi
            {
                CreditTypeId = request.CreditTypeId,
                Tutar = request.Tutar,
                Vade = request.Vade,
                FaizOrani = urun.faizOrani,
                AylikOdeme = Math.Round(taksit, 2),
                ToplamOdeme = Math.Round(taksit * request.Vade, 2),
                HesaplamaTarihi = DateTime.Now
            };
        }

        // Loglama metodunu direkt KrediHesaplamaLog modeliyle çalışacak şekilde oluşturuyoruz
        // böylece Controllerda bir dönüşüm yapmaya gerek kalmıyor
        public void LogHesaplama(KrediHesaplamaLog hesaplamaLogu)
        {
            _context.HesaplamaLoglari.Add(hesaplamaLogu);
            _context.SaveChanges();
        }

        public async Task LogBasvuru(KrediBasvuru basvuru)
        {
            // Kredi başvuru logunu oluşturuyoruz
            var log = new KrediBasvuruLog
            {
                UserId = basvuru.UserId,
                CreditTypeId = basvuru.CreditTypeId,
                Tutar = basvuru.Tutar,
                Vade = basvuru.Vade,
                basvuruTarihi = basvuru.basvuruTarihi
            };

            // Başvuru onaylandıysa log kaydını güncelliyoruz
            _context.BasvuruLoglari.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}