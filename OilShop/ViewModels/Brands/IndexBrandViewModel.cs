using OilShop.Models.Data;
using System.Collections.Generic;

namespace OilShop.ViewModels.Brands
{
    public class IndexBrandViewModel
    {
        public IEnumerable<Brand> Brands { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterBrandViewModel FilterBrandViewModel { get; set; }
        public SortBrandViewModel SortBrandViewModel { get; set; }
    }
}

