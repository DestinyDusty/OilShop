using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.Countries
{
    public class FilterCountryViewModel
    {
        public string SelectedCountry { get; private set; }    // введенная страна производителя

        public FilterCountryViewModel(string country)
        {
            SelectedCountry = country;
        }
    }
}
