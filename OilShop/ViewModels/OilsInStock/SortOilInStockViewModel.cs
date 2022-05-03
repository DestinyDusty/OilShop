using OilShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.OilsInStock
{
    public class SortOilInStockViewModel
    {
        public OilInStockSortState MarginSort { get; private set; }
        public OilInStockSortState PurchaseDateSort { get; private set; }
        public OilInStockSortState PurchasePriceSort { get; private set; }
        public OilInStockSortState Current { get; private set; }     // текущее значение сортировки

        public SortOilInStockViewModel(OilInStockSortState sortOrder)
        {
            MarginSort = sortOrder == OilInStockSortState.MarginAsc ?
                OilInStockSortState.MarginDesc : OilInStockSortState.MarginAsc;
            PurchaseDateSort = sortOrder == OilInStockSortState.PurchaseDateAsc ?
                OilInStockSortState.PurchaseDateDesc : OilInStockSortState.PurchaseDateAsc;
            PurchasePriceSort = sortOrder == OilInStockSortState.PurchasePriceAsc ?
                OilInStockSortState.PurchasePriceDesc : OilInStockSortState.PurchasePriceAsc;

            Current = sortOrder;
        }
    }
}
