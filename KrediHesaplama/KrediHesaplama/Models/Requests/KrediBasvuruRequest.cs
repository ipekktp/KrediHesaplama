using System.ComponentModel.DataAnnotations;

namespace KrediHesaplama.Models.Requests
{
    //data transfer object- presentation layer
    public class KrediBasvuruRequest
    {
        [Key]
        public int? UserId { get; set; }

        public string? urunAdi { get; set; }
        public string? TCKN { get; set; }
        public int CreditTypeId { get; set; }
        public int Tutar { get; set; }
        public int Vade { get; set; }
    }
}
