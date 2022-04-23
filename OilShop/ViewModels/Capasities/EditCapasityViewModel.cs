using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Capasities
{
    public class EditCapasityViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите объем")]
        [Display(Name = "Объем, л.")]
        public short CapasityOil { get; set; }
    }
}
