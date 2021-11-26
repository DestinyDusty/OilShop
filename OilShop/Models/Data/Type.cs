using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class Type
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип масла")]
        [Display(Name = "Тип масла")]
        public string TypeOil { get; set; }
        

        //навигационное свойство
        [Required]
        public ICollection<Oil> Oils { get; set; }
    }
}
