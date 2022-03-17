using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.Suppliers
{
    public class FilterSupplierViewModel
    {
        public string SelectedSupplier { get; private set; }    // введенный поставщик

        public FilterSupplierViewModel(string supplier)
        {
            SelectedSupplier = supplier;
        }
    }
}
