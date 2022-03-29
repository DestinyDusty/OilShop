namespace OilShop.ViewModels.StatusesOrder
{
    public class FilterStatusOrderViewModel
    {
        public string SelectedStatusOrder { get; private set; }    // введенный поставщик

        public FilterStatusOrderViewModel(string statusOrder)
        {
            SelectedStatusOrder = statusOrder;
        }
    }
}
