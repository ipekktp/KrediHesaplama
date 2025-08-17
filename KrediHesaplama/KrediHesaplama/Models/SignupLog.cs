using System.ComponentModel.DataAnnotations;

namespace KrediHesaplama.Models
{
    public class SignupLog
    {
        [Key]
        public int LogId { get; set; }

        public int? UserId { get; set; }

        public string? TCKN { get; set; }

        public DateTime Timestamp { get; set; }

        public string? Message { get; set; }
    }
}
