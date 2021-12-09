using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Brands
{
    public class CreateBrandViewModel
    {
        [Required(ErrorMessage = "Введите название бренда")]
        [Display(Name = "Название бренда")]
        public string BrandOil { get; set; }
    }
}
