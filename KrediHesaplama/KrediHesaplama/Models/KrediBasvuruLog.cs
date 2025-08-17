using System.ComponentModel.DataAnnotations;

namespace KrediHesaplama.Models
{
    public class KrediBasvuruLog
    {
        [Key]
        public int basvurulogId { get; set; }
        public int UserId { get; set; }
        public int CreditTypeId { get; set; }
        public double Tutar { get; set; }
        public int Vade { get; set; }
        public bool isApproved { get; set; }
        public bool isCancelled { get; set; }
        public DateTime basvuruTarihi { get; set; } = DateTime.Now;
    }
}
