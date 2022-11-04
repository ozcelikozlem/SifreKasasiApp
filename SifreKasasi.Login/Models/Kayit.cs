using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SifreKasasi.Login.Models
{
    public class Kayit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(15)]
        [Required]
        public string KullaniciAdi { get; set; }

        [StringLength(50)]
        
        public string Email { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }

        [Display(Name = "Ad Soyad")]
        public string FullName => string.Concat(Ad, " ", Soyad);

        [Required]
        public string Password { get; set; }
    }
}
