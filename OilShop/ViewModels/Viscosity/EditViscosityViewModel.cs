using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Viscosity
{
    public class EditViscosityViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите вязкость масла")]
        [Display(Name = "Вязкость масла")]
        public string ViscosityOil { get; set; }
    }
}
