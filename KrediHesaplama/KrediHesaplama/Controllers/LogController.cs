using Microsoft.AspNetCore.Mvc;
using System.Linq;
using KrediHesaplama.Models;
using KrediHesaplama.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KrediHesaplama.Controllers
{
    [ApiController]
    [Route("api/logs")] // Tüm log endpoint'leri için temel rota
    public class LogController : ControllerBase
    {
        private readonly KrediDbContext _context;

        public LogController(KrediDbContext context)
        {
            // Dependency Injection ile KrediDbContext'i alıyoruz
            _context = context;
        }

        [HttpGet("hesaplama-logs")] // Kredi hesaplama loglarını listelemek için endpoint
        public IActionResult GetHesaplamaLogs(
            [FromQuery] int? creditTypeId,
            [FromQuery] double? tutar,
            [FromQuery] int? vade)
        {
            // Veritabanındaki KrediHesaplamaLog tablosuna sorgu hazırlıyoruz
            var query = _context.HesaplamaLoglari.AsQueryable();

            // Eğer creditTypeId parametresi gönderilmişse sorguya filtre ekliyoruz
            if (creditTypeId.HasValue)
            {
                query = query.Where(l => l.CreditTypeId == creditTypeId.Value);
            }

            // Eğer tutar parametresi gönderilmişse sorguya filtre ekliyoruz
            if (tutar.HasValue)
            {
                query = query.Where(l => l.Tutar == tutar.Value);
            }

            // Eğer vade parametresi gönderilmişse sorguya filtre ekliyoruz
            if (vade.HasValue)
            {
                query = query.Where(l => l.Vade == vade.Value);
            }

            // Log kayıtlarını en son tarihe göre sıralayıp listeyi çekiyoruz
            var logs = query.OrderByDescending(l => l.HesaplamaTarihi).ToList();

            return Ok(logs);
        }

        [HttpGet("basvuru-logs")] // Kredi başvuru loglarını listelemek için endpoint
        public async Task<IActionResult> GetBasvuruLogs()
        {
            // Veritabanındaki KrediBasvuruLog tablosundan tüm kayıtları en son başvuru tarihine göre sıralayıp listeyi çekiyoruz
            var logs = await _context.BasvuruLoglari.OrderByDescending(l => l.basvuruTarihi).ToListAsync();
            return Ok(logs);
        }
    }
}