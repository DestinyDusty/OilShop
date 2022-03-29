using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.ViewModels.PricesOil
{
    public class EditPriceOilViewModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите дату установки цены")]
        [Display(Name = "Дата установки цены")]
        public DateTime PriceSettingDate { get; set; }

        [Required(ErrorMessage = "Введите цену масла")]
        [Display(Name = "Цена масла")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Масло")]
        public int IdOil { get; set; }
    }
}
