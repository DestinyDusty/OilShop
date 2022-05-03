using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Oils
{
    public class EditOilViewModel
    {
        public int Id { get; set; }

        public IFormFile Photo { get; set; }

        //public string Path { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile UploadedFile { get; set; }

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
    }
}
