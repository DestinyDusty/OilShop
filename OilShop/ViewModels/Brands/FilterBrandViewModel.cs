using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.Brands
{
    public class FilterBrandViewModel
    {
        public string SelectedBrand { get; private set; }    // введенный бренд

        public FilterBrandViewModel(string brand)
        {
            SelectedBrand = brand;
        }
    }
}
