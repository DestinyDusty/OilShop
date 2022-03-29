using System;
using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.PricesOil
{
    public class CreatePriceOilViewModel
    {
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
