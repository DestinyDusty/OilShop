using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class Country
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите страну производителя")]
        [Display(Name = "Страна производителя")]
        public string CountryOrigin { get; set; }


        //навигационное свойство
        [Required]
        public ICollection<Oil> Oils { get; set; }
    }
}
