using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Countries
{
    public class CreateCountryViewModel
    {
        [Required(ErrorMessage = "Введите название страны производителя")]
        [Display(Name = "Название страны производителя")]
        public string CountryOrigin { get; set; }
    }
}
