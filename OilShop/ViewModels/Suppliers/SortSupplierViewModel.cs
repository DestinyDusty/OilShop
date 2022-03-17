using OilShop.Models.Enums;

namespace OilShop.ViewModels.Suppliers
{
    public class SortSupplierViewModel
    {
        public SupplierSortState SupplierSort { get; private set; }
        public SupplierSortState Current { get; private set; }     // текущее значение сортировки

        public SortSupplierViewModel(SupplierSortState sortOrder)
        {
            SupplierSort = sortOrder == SupplierSortState.SupplierAsc ?
                SupplierSortState.SupplierDesc : SupplierSortState.SupplierAsc;
            Current = sortOrder;
        }
    }
}
