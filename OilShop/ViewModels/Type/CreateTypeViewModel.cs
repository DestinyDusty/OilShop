using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Type
{
    public class CreateTypeViewModel
    {
        [Required(ErrorMessage = "Введите статус заказа")]
        [Display(Name = "Статус заказа")]
        public string TypeOil { get; set; }
    }
}
