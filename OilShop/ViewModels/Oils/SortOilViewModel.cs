using OilShop.Models.Enums;

namespace OilShop.ViewModels.Oils
{
    public class SortOilViewModel
    {
        public OilSortState BrandSort { get; private set; }
        public OilSortState TypeSort { get; private set; }
        public OilSortState ViscositySort { get; private set; }
        public OilSortState CapasitySort { get; private set; }
        public OilSortState CountrySort { get; private set; }
        public OilSortState SupplierSort { get; private set; }
        public OilSortState PurchasePriceSort { get; private set; }
        public OilSortState Current { get; private set; }     // текущее значение сортировки

        public SortOilViewModel(OilSortState sortOrder)
        {
            BrandSort = sortOrder == OilSortState.BrandAsc ?
                OilSortState.BrandDesc : OilSortState.BrandAsc;
            TypeSort = sortOrder == OilSortState.TypeAsc ?
                OilSortState.TypeDesc : OilSortState.TypeAsc;
            ViscositySort = sortOrder == OilSortState.ViscosityAsc ?
                OilSortState.ViscosityDesc : OilSortState.ViscosityAsc;
            CapasitySort = sortOrder == OilSortState.CapasityAsc ?
                OilSortState.CapasityDesc : OilSortState.CapasityAsc;
            CountrySort = sortOrder == OilSortState.CountryAsc ?
                OilSortState.CountryDesc : OilSortState.CountryAsc;
            SupplierSort = sortOrder == OilSortState.SupplierAsc ?
                OilSortState.SupplierDesc : OilSortState.SupplierAsc;
            PurchasePriceSort = sortOrder == OilSortState.PurchasePriceAsc ?
                OilSortState.PurchasePriceDesc : OilSortState.PurchasePriceAsc;
            Current = sortOrder;
        }
    }
}
