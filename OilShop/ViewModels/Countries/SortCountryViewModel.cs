using OilShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.Countries
{
    public class SortCountryViewModel
    {
        public CountrySortState CountrySort { get; private set; }
        public CountrySortState Current { get; private set; }     // текущее значение сортировки

        public SortCountryViewModel(CountrySortState sortOrder)
        {
            CountrySort = sortOrder == CountrySortState.CountryAsc ?
                CountrySortState.CountryDesc : CountrySortState.CountryAsc;
            Current = sortOrder;
        }
    }
}
