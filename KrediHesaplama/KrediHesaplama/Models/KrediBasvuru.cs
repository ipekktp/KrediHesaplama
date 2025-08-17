using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KrediHesaplama.Models
{
    //data access layer- veri saklama  
    public class KrediBasvuru
    {
        [Key]
        public int basvuruId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int CreditTypeId { get; set; }

        public double Tutar { get; set; }
        public int Vade { get; set; }

        public bool IsApproved { get; set; }
        public bool IsCancelled { get; set; }


        public DateTime basvuruTarihi { get; set; } = DateTime.Now;

        //Userın navigasyon (gezinti) özelliğini anlatır ve KrediBasvuru ile 
        //User tablosu arasındaki doğrudan ilişkiyi belirtir
        //virtual ef core özelliği veritabanından user nesnesini yükler
        [ForeignKey("UserId")]
        public virtual required User User { get; set; }

        [ForeignKey("CreditTypeId")]
        public virtual required KrediUrun KrediUrun { get; set; }

    }
}
