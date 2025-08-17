using System.ComponentModel.DataAnnotations;

namespace KrediHesaplama.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(11)]
        public string? TCKN { get; set; }
        public string? Sifre { get; set; }
        public string? adSoyad { get; set; }

    }
}
