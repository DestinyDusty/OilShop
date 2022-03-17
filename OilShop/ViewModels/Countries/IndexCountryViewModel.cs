using OilShop.Models.Data;
using System.Collections.Generic;

namespace OilShop.ViewModels.Countries
{
    public class IndexCountryViewModel
    {
        public IEnumerable<Country> Countries { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterCountryViewModel FilterCountryViewModel { get; set; }
        public SortCountryViewModel SortCountryViewModel { get; set; }
    }
}
