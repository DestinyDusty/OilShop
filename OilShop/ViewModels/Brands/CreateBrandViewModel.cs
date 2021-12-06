using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.Brands
{
    public class CreateBrandViewModel
    {
        [Required(ErrorMessage = "Введите название бренда")]
        [Display(Name = "Название бренда")]
        public string BrandOil { get; set; }
    }
}
