using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.Suppliers
{
    public class EditSupplierViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите поставщика масла")]
        [Display(Name = "Поставщик масла")]
        public string SupplierOil { get; set; }
    }
}
