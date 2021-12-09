using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Countries
{
    public class EditCountryViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название страны производителя")]
        [Display(Name = "Страна производителя")]
        public string CountryOrigin { get; set; }
    }
}
