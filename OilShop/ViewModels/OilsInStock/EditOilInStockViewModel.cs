using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.OilsInStock
{
    public class EditOilInStockViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите цену закупки")]
        [Display(Name = "Цена закупки, руб.")]
        //[DataType(DataType.Currency, ErrorMessage ="Цена введена неправильно")]
        public int PurchasePriceRub { get; set; }

        [Required(ErrorMessage = "Введите цену закупки")]
        [Display(Name = "Цена закупки, коп.")]
        //[DataType(DataType.Currency, ErrorMessage ="Цена введена неправильно")]
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
        public int IdOil { get; set; }
    }
}
