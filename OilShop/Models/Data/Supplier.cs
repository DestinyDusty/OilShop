using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class Supplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите поставщика масла")]
        [Display(Name = "Поставщик масла")]
        public string SupplierOil { get; set; }

        [Display(Name = "Пользователь")]
        public string IdUser { get; set; }

        //навигационное свойство
        [Required]
        public ICollection<Oil> Oils { get; set; }

        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
