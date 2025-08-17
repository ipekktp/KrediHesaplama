using System.ComponentModel.DataAnnotations;

namespace KrediHesaplama.Models
{
    public class KrediHesaplamaKaydi
    {
        [Key]
        public int KayitId { get; set; }

        public int CreditTypeId { get; set; }
        public double Tutar { get; set; }
        public int Vade { get; set; }
        public double FaizOrani { get; set; }
        public double AylikOdeme { get; set; }
        public double ToplamOdeme { get; set; }
        public DateTime HesaplamaTarihi { get; set; } = DateTime.Now;
    }
}
