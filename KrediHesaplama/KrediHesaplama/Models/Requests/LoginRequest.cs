using System.ComponentModel.DataAnnotations;

namespace KrediHesaplama.Models.Requests
{
    public class LoginRequest
    {
        [Key] [Required]
        public string TCKN { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
    }
}
