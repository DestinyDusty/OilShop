using OilShop.Models.Data;
using System.Collections.Generic;

namespace OilShop.ViewModels.StatusesOrder
{
    public class IndexStatusOrderViewModel
    {
        public IEnumerable<StatusOrder> Statuses { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterStatusOrderViewModel FilterStatusOrderViewModel { get; set; }
        public SortStatusOrderViewModel SortStatusOrderViewModel { get; set; }
    }
}
