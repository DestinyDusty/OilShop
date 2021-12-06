using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.Brands
{
    public class EditBrandViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название бренда")]
        [Display(Name = "Бренд")]
        public string BrandOil { get; set; }
    }
}
