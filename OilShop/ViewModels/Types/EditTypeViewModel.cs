using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Types
{
    public class EditTypeViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип масла")]
        [Display(Name = "Тип масла")]
        public string TypeOil { get; set; }
    }
}
