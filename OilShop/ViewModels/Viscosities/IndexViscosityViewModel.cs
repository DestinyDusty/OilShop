using OilShop.Models.Data;
using System.Collections.Generic;

namespace OilShop.ViewModels.Viscosities
{
    public class IndexViscosityViewModel
    {
        public IEnumerable<Viscosity> Viscosities { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViscosityViewModel FilterViscosityViewModel { get; set; }
    }
}

