using System.ComponentModel.DataAnnotations;

namespace KrediHesaplama.Models.Requests
{
    public class SignupRequest
    {
        [Key]
        public string? TCKN { get; set; }
        public string? adSoyad { get; set; }
        public string? sifre { get; set; }
    }
}
