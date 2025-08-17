using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace KrediHesaplama.Models
{
    public class KrediUrun
    {
        [Key]
        public int CreditTypeId { get; set; }

        [Required]
        public string? urunAdi { get; set; }
        public double faizOrani { get; set; } 
        public double minTutar { get; set; } 
        public double maxTutar { get; set; } 
        public int minVade { get; set; }
        public int maxVade { get; set; }

    }
}
