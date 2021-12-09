using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Capasity
{
    public class EditCapasityViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите объем")]
        [Display(Name = "Объем")]
        public short CapasityOil { get; set; }
    }
}
