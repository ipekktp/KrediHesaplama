using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KrediHesaplama.Data;
using KrediHesaplama.Models;
using KrediHesaplama.Models.Requests;
using KrediHesaplama.Service;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace KrediHesaplama.Controllers
{
    [ApiController]
    [Route("api/kredi")] // Tüm kredi işlemleri için temel rota
    public class KrediController : ControllerBase
    {
        private readonly KrediDbContext _context;
        private readonly KrediService _krediService;

        public KrediController(KrediDbContext context, KrediService krediService)
        {
            // Dependency Injection ile KrediDbContext ve KrediService'i alıyoruz.
            // KrediDbContext Entity Framework Core ile veritabanı işlemlerini yapmamızı sağlıyor
            // KrediService ise kredi hesaplama ve başvuru işlemlerini yönetiyor
            // KrediService iş mantığını içerir ve KrediDbContext'i kullanarak veritabanı işlemlerini gerçekleştiriyor
            _context = context;
            _krediService = krediService;
        }

        // Kredi Hesaplama İşlemi

        [HttpPost("hesapla")]
        public IActionResult Hesapla([FromBody] HesaplamaRequest request)
        {
            // Gelen istekten kredi türü ve tutar bilgilerini alıyoruz
            // Eğer bu bilgiler eksikse, BadRequest ile hata mesajı döndürüyoruz

            // KrediService'den hesaplama sonucunu alıyoruz
            var hesaplamaKaydi = _krediService.Hesapla(request);

            if (hesaplamaKaydi == null)
            {
                return BadRequest("Geçersiz kredi ürünü.");
            }

            // Hesaplama sonucunu veritabanına log olarak kaydetmek için bir KrediHesaplamaLog nesnesine dönüştürüyoruz
            var logKaydi = new KrediHesaplamaLog
            {
                CreditTypeId = hesaplamaKaydi.CreditTypeId,
                Tutar = hesaplamaKaydi.Tutar,
                Vade = hesaplamaKaydi.Vade,
                FaizOrani = hesaplamaKaydi.FaizOrani,
                AylikOdeme = hesaplamaKaydi.AylikOdeme,
                ToplamOdeme = hesaplamaKaydi.ToplamOdeme,
                HesaplamaTarihi = hesaplamaKaydi.HesaplamaTarihi
            };

            // Oluşturduğumuz log nesnesini KrediService'deki LogHesaplama metodu ile veritabanına kaydediyoruz
            // Bu, hesaplama işlemi yapıldığında veritabanına bir kayıt atılmasını sağlar
            _krediService.LogHesaplama(logKaydi);

            // Hesaplama sonucunu döndürüyoruz
            return Ok(new { aylikTaksit = hesaplamaKaydi.AylikOdeme });
        }

        // Yeni Kredi Başvurusu Yapma İşlemi

        [HttpPost("basvur")]
        public async Task<IActionResult> YeniBasvuru([FromBody] KrediBasvuruRequest request)
        {
            // try-catch bloğunun amacı programın bir hatayla karşılaştığında çökmesini önlemektir. Bu blok olmadan beklenmedik bir istisna
            // tüm uygulamanın durmasına neden olabiliyor. try-catch bloğu bu hatayı yakalıyor
            // ve programın normal bir şekilde çalışmaya devam etmesine izin verirken kullanıcıya daha anlamlı bir yanıt veriyor
            try
            {
                // Gelen istekten TCKN bilgisi alıyoruz
                var musteri = await _context.Users.FirstOrDefaultAsync(m => m.TCKN == request.TCKN);
                if (musteri == null)
                {
                    // Eğer TCKN bilgisi ile eşleşen müşteri bulunamazsa, BadRequest ile hata mesajı döndürüyoruz
                    return BadRequest("Belirtilen TCKN'ye ait müşteri bulunamadı.");
                }

                // Eğer müşteri bulunursa, kredi türü ve tutar bilgilerini alıyoruz
                var krediUrunu = await _context.KrediUrunleri.FirstOrDefaultAsync(k => k.CreditTypeId == request.CreditTypeId);
                if (krediUrunu == null)
                {
                    // Eğer kredi türü bulunamazsa BadRequest ile hata mesajı döndürüyoruz
                    return BadRequest("Belirtilen kredi türü geçersiz.");
                }

                // Eğer kredi türü ve tutar bilgileri geçerliyse yeni bir kredi başvurusu oluşturuyoruz
                var basvuru = new KrediBasvuru
                {
                    // Başvuru için gerekli alanları dolduruyoruz
                    UserId = musteri.UserId,
                    CreditTypeId = krediUrunu.CreditTypeId,
                    Tutar = request.Tutar,
                    Vade = request.Vade,
                    IsCancelled = false,
                    basvuruTarihi = DateTime.Now,
                    User = musteri,
                    KrediUrun = krediUrunu
                };

                // Başvuruyu veritabanına ekliyoruz
                _context.KrediBasvurulari.Add(basvuru);
                // Veritabanındaki değişiklikleri kaydediyoruz
                await _context.SaveChangesAsync();
                // Başvuru logunu kaydediyoruz
                await _krediService.LogBasvuru(basvuru);

                return Ok(new { mesaj = "Başvuru başarıyla yapıldı." });
            }
            // Eğer bir hata oluşursa 500 Internal Server Error ile hata mesajı döndürüyoruz
            catch (Exception)
            {
                return StatusCode(500, "Sunucu tarafında bir hata oluştu.");
            }
        }

        // Başvuru Listesini TCKNye Göre Getirme
        [HttpGet("basvurular/{tckn}")]
        public async Task<IActionResult> GetBasvurularByTckn(string tckn)
        {
            // TCKN bilgisi boş veya null ise BadRequest ile hata mesajı döndürüyoruz
            if (string.IsNullOrEmpty(tckn))
            {
                return BadRequest("TCKN bilgisi gereklidir.");
            }

            // Veritabanından TCKNye göre müşteri bilgilerini alıyoruz
            var musteri = await _context.Users.FirstOrDefaultAsync(m => m.TCKN == tckn);
            if (musteri == null)
            {
                // Eğer müşteri bulunamazsa NotFound ile hata mesajı döndürüyoruz
                return NotFound("Müşteri bulunamadı.");
            }

            // Müşteri bilgisi bulunduğunda bu müşteriye ait kredi başvurularını alıyoruz
            var basvurular = await _context.KrediBasvurulari
                .Where(b => b.UserId == musteri.UserId)
                .OrderByDescending(x => x.basvuruTarihi)
                .ToListAsync();

            return Ok(basvurular);
        }

        // Başvuruyu İptal Etme İşlemi
        [HttpDelete("iptal/{basvuruId}")]
        public async Task<IActionResult> IptalEt(int basvuruId)
        {
            // Veritabanından başvuru IDsine göre başvuruyu buluyoruz
            var basvuru = await _context.KrediBasvurulari.FindAsync(basvuruId);
            // Eğer başvuru bulunamazsa NotFound ile hata mesajı döndürüyoruz
            if (basvuru == null)
            {
                return NotFound("İptal edilecek başvuru bulunamadı.");
            }

            if (basvuru.IsCancelled)
            {
                return BadRequest("Bu başvuru zaten iptal edilmiş.");
            }

            // Başvuru bulunduğunda başvurunun iptal edildiğini belirtiyoruz
            basvuru.IsCancelled = true;
            // Başvuru durumunu güncelliyoruz
            _context.KrediBasvurulari.Update(basvuru);
            // Veritabanındaki değişiklikleri kaydediyoruz
            await _context.SaveChangesAsync();

            return Ok(new { mesaj = "Başvuru başarıyla iptal edildi." });
        }

        // Vade aralığı getirme
        [HttpGet("vade-araligi/{creditTypeId}")]
        public async Task<IActionResult> GetVadeAraligi(int creditTypeId)
        {
            // Veritabanından kredi türüne göre kredi ürününü buluyoruz
            var krediUrunu = await _context.KrediUrunleri
                .FirstOrDefaultAsync(k => k.CreditTypeId == creditTypeId);

            // Eğer kredi ürünü bulunamazsa NotFound ile hata mesajı döndürüyoruz
            if (krediUrunu == null)
            {
                return NotFound("Belirtilen kredi türü bulunamadı.");
            }

            return Ok(new
            {
                minVade = krediUrunu.minVade,
                maxVade = krediUrunu.maxVade
            });
        }
    }
}