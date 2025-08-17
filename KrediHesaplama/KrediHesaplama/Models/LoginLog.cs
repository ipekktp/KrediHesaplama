using System.ComponentModel.DataAnnotations;

namespace KrediHesaplama.Models
{
    public class LoginLog
    {
        [Key]
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public string? islemTuru { get; set; } 
        public string? Seviye { get; set; } 
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
