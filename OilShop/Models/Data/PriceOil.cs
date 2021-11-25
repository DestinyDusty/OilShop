using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class PriceOil
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите поставщика масла")]
        [Display(Name = "Поставщик масла")]
        public DateTime Supplier { get; set; }


        [Required(ErrorMessage = "Введите цена масла")]
        [Display(Name = "Цена масла")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Масло")]
        public int IdOil { get; set; }

        [Display(Name = "Пользователь")]
        public string IdUser { get; set; }

        //навигационное свойство
        [ForeignKey("IdUser")]
        public User User { get; set; }

        [ForeignKey("IdOil")]
        public Oil Oil { get; set; }
    }
}
