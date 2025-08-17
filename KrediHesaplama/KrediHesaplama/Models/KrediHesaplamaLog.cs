using System.ComponentModel.DataAnnotations;
using System;

namespace KrediHesaplama.Models
{
    public class KrediHesaplamaLog
    {
        [Key]
        public int LogId { get; set; }
        public int CreditTypeId { get; set; }
        public double Tutar { get; set; }
        public int Vade { get; set; }
        public double FaizOrani { get; set; }
        public double AylikOdeme { get; set; }
        public double ToplamOdeme { get; set; }
        public DateTime HesaplamaTarihi { get; set; } = DateTime.Now;
    }
}
