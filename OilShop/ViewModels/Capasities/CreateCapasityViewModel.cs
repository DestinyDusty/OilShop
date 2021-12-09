using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Capasity
{
    public class CreateCapasityViewModel
    {
        [Required(ErrorMessage = "Введите объем")]
        [Display(Name = "Объем")]
        public short CapasityOil { get; set; }
    }
}
