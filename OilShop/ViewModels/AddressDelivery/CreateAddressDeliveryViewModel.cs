using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.AddressDelivery
{
    public class CreateAddressDeliveryViewModel
    {

        [Required(ErrorMessage = "Введите улицу")]
        [Display(Name = "Поставщик масла")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Введите номер дома")]
        [Display(Name = "Номер дома")]
        public string House { get; set; }
    }
}
