using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class Oil
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public int Id { get; set; }

        [Display(Name = "Название фото")]
        public string Photo { get; set; }

        public string Path { get; set; }

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
        [DataType(DataType.Date)]
        public DateTime DateOfManufacture { get; set; }

        [Required(ErrorMessage = "Введите дату окончания срока годности")]
        [Display(Name = "Дата окончания срока годности")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage = "Введите цену закупки")]
        [Display(Name = "Цена закупки")]
        [Column("decimal(10,2)")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Введите дату закупки")]
        [Display(Name = "Дата закупки")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        //навигационное свойство
        [Required]
        public ICollection<Order> Orders { get; set; }

        [ForeignKey("IdBrand")]
        [Display(Name = "Бренд")]
        public Brand Brand { get; set; }

        [Display(Name = "Тип")]
        [ForeignKey("IdType")]
        public Type Type { get; set; }

        [Display(Name = "Вязкость")]
        [ForeignKey("IdViscosity")]
        public Viscosity Viscosity { get; set; }

        [Display(Name = "Вязкость")]
        [ForeignKey("IdCapasity")]
        public Capasity Capasity { get; set; }

        [Display(Name = "Страна производителя")]
        [ForeignKey("IdCountry")]
        public Country Country { get; set; }

        [Display(Name = "Поставщик")]
        [ForeignKey("IdSupplier")]
        public Supplier Supplier { get; set; }

        [Required]
        public ICollection<PriceOil> PricesOil { get; set; }

    }
}
