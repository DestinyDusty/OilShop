namespace OilShop.ViewModels.Brands
{
    public class FilterBrandViewModel
    {
        public string SelectedBrand { get; private set; }    // введенный бренд

        public FilterBrandViewModel(string brand)
        {
            SelectedBrand = brand;
        }
    }
}
