using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Suppliers
{
    public class CreateSupplierViewModel
    {
        [Required(ErrorMessage = "Введите поставщика масла")]
        [Display(Name = "Поставщик масла")]
        public string SupplierOil { get; set; }
    }
}
