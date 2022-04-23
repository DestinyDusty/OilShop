using OilShop.Models.Data;
using System.Collections.Generic;

namespace OilShop.ViewModels.Oils
{
    public class IndexOilViewModel
    {
        public IEnumerable<Oil> Oils { get; set; }
        public FilterOilViewModel FilterOilViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
