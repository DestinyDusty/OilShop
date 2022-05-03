using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.ViewModels.OilsInStock
{
    public class CreateOilInStockViewModel
    {
        [Required(ErrorMessage = "Введите цену закупки")]
        [Display(Name = "Цена закупки, руб.")]
        public int PurchasePriceRub { get; set; }

        [Required(ErrorMessage = "Введите цену закупки")]
        [Display(Name = "Цена закупки, коп.")]
        public int PurchasePrice { get; set; }

        [Required(ErrorMessage = "Введите дату закупки")]
        [Display(Name = "Дата закупки")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Введите дату закупки")]
        [Display(Name = "Дата закупки")]
        public int Margin { get; set; }

        [Required]
        [Display(Name = "Масло")]
        public short IdOil { get; set; }
    }
}
