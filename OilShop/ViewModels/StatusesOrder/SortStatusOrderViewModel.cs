using OilShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.ViewModels.StatusesOrder
{
    public class SortStatusOrderViewModel
    {
        public StatusOrderSortState StatusOrderSort { get; private set; }
        public StatusOrderSortState Current { get; private set; }     // текущее значение сортировки

        public SortStatusOrderViewModel(StatusOrderSortState sortOrder)
        {
            StatusOrderSort = sortOrder == StatusOrderSortState.StatusAsc ?
                StatusOrderSortState.StatusDesc : StatusOrderSortState.StatusAsc;
            Current = sortOrder;
        }
    }
}