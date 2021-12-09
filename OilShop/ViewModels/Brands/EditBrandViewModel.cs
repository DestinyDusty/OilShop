using System.ComponentModel.DataAnnotations;

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
