using OilShop.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.OilsInStock
{
    public class IndexOilInStockViewModel
    {
        public IEnumerable<OilInStock> OilsInStock { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterOilInStockViewModel FilterOilInStockViewModel { get; set; }
        public SortOilInStockViewModel SortOilInStockViewModel { get; set; }
    }
}
