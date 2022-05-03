namespace OilShop.ViewModels.OilsInStock
{
    public class FilterOilInStockViewModel
    {
        public int SelectedMargin { get; private set; }    // введенное масло на складе

        public FilterOilInStockViewModel (int margin)
        {
            SelectedMargin = margin;
        }
    }
}
