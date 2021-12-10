using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.StatusOrders
{
    public class EditStatusOrderViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите статус заказа")]
        [Display(Name = "Статус заказа")]
        public string Status { get; set; }
    }
}
