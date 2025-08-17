using System.ComponentModel.DataAnnotations;

namespace KrediHesaplama.Models.Requests
{
    public class HesaplamaRequest
    {
        [Key]
        public int LogId { get; set; }
        public int CreditTypeId { get; set; }

        [Required]
        public double Tutar { get; set; }
        public int Vade { get; set; }

        public double FaizOrani { get; set; }

        public double AylikOdeme { get; set; }
        public double ToplamOdeme { get; set; }
    }
}
