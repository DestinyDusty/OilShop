using OilShop.Models.Data;
using System.Collections.Generic;

namespace OilShop.ViewModels.Suppliers
{
    public class IndexSupplierViewModel
    {
        public IEnumerable<Supplier> Suppliers { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterSupplierViewModel FilterSupplierViewModel { get; set; }
        public SortSupplierViewModel SortSupplierViewModel { get; set; }
    }
}
