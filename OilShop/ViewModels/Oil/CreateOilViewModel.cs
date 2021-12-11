using System;
using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Oil
{
    public class CreateOilViewModel
    {
        [Required]
        [Display(Name = "Бренд")]
        public short IdBrand { get; set; }

        [Required]
        [Display(Name = "Тип")]
        public short IdType { get; set; }

        [Required]
        [Display(Name = "Вязкость")]
        public short IdViscosity { get; set; }

        [Required]
        [Display(Name = "Объем")]
        public short IdCapasity { get; set; }

        [Required]
        [Display(Name = "Страна производителя")]
        public short IdCountry { get; set; }

        [Required]
        [Display(Name = "Поставщик")]
        public int IdSupplier { get; set; }

        [Required(ErrorMessage = "Введите дату изготовления")]
        [Display(Name = "Дата изготовления")]
        public DateTime DateOfManufacture { get; set; }

        [Required(ErrorMessage = "Введите дату окончания срока годности")]
        [Display(Name = "Дата окончания срока годности")]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage = "Введите цену закупки")]
        [Display(Name = "Цена закупки")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Введите дату закупки")]
        [Display(Name = "Дата закупки")]
        public DateTime PurchaseDate { get; set; }
    }
}
