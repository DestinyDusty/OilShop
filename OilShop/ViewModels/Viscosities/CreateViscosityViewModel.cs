using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Viscosities
{
    public class CreateViscosityViewModel
    {
        [Required(ErrorMessage = "Введите вязкость масла")]
        [Display(Name = "Вязкость масла")]
        public string ViscosityOil { get; set; }
    }
}
