namespace OilShop.ViewModels.Viscosities
{
    public class FilterViscosityViewModel
    {
        public string SelectedViscosity { get; private set; }    // введенная страна производителя

        public FilterViscosityViewModel(string viscosity)
        {
            SelectedViscosity = viscosity;
        }
    }
}