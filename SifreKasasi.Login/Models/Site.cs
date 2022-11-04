using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SifreKasasi.Login.Models
{
    public class Site
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SId { get; set; }
        [Required]
        public string SiteAdi { get; set; }
        [Required]
        public string SPassword { get; set; }
        
    }
}
