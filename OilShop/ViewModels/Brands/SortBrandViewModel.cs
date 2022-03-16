using OilShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.Brands
{
    public class SortBrandViewModel
    {
        public BrandSortState BrandSort { get; private set; }
        public BrandSortState Current { get; private set; }     // текущее значение сортировки

        public SortBrandViewModel(BrandSortState sortOrder)
        {
            BrandSort = sortOrder == BrandSortState.BrandOilAsc ?
                BrandSortState.BrandOilDesc : BrandSortState.BrandOilAsc;
            Current = sortOrder;
        }
    }
}
