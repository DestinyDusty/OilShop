using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class Viscosity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите вязкость масла")]
        [Display(Name = "Вязкость масла")]
        public short ViscosityOil { get; set; }


        //навигационное свойство
        [Required]
        public ICollection<Oil> Oils { get; set; }
    }
}
