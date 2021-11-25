using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class Capasity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите объем масла")]
        [Display(Name = "Объем масла")]
        public short CapasityOil { get; set; }

        [Display(Name = "Пользователь")]
        public string IdUser { get; set; }

        //навигационное свойство
        [Required]
        public ICollection<Oil> Oils { get; set; }

        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
