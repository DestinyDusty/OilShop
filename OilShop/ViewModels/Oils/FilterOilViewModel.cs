using Microsoft.AspNetCore.Mvc.Rendering;
using OilShop.Models.Data;
using System.Collections.Generic;

namespace OilShop.ViewModels.Oils
{
    public class FilterOilViewModel
    {
        public SelectList Brands { get; private set; }
        public short? BrandOil { get; private set; }

        public SelectList Types { get; private set; }
        public short? TypeOil { get; private set; }

        public SelectList Viscosities { get; private set; }
        public short? ViscosityOil { get; private set; }

        public SelectList Capasities { get; private set; }
        public short? CapasityOil { get; private set; }

        public SelectList Countries { get; private set; }
        public short? CountryOrigin { get; private set; }

        public SelectList Suppliers { get; private set; }
        public short? SupplierOil { get; private set; }

        //public dateOfManufacture
        //public expirationDate
        //public purchaseDate
        public string SelectPurchasePrice { get; private set; }

        public FilterOilViewModel(string purchasePrice,
            List<Brand> brands, short? brandOil,
            List<Type> types, short? typeOil,
            List<Viscosity> viscosities, short? viscosityOil,
            List<Capasity> capasities, short? capasityOil,
            List<Country> countries, short? countryOrigin,
            List<Supplier> suppliers, short? supplierOil)
        {
            SelectPurchasePrice = purchasePrice;

            brands.Insert(0, new Brand { BrandOil = "", Id = 0 });

            Brands = new SelectList(brands, "Id", "BrandOil", brandOil);
            BrandOil = brandOil;

            types.Insert(0, new Type { TypeOil = "", Id = 0 });

            Types = new SelectList(types, "Id", "BrandOil", typeOil);
            TypeOil = typeOil;

            viscosities.Insert(0, new Viscosity { ViscosityOil = "", Id = 0 });

            Viscosities = new SelectList(viscosities, "Id", "ViscosityOil", viscosityOil);
            ViscosityOil = viscosityOil;

            /*capasities.Insert(0, new Capasity { CapasityOil = "", Id = 0 });

            Capasities = new SelectList(capasities, "Id", "CapasityOil", capasityOil);
            CapasityOil = capasityOil;*/

            countries.Insert(0, new Country { CountryOrigin = "", Id = 0 });

            Countries = new SelectList(countries, "Id", "CountryOrigin", countryOrigin);
            CountryOrigin = countryOrigin;

            suppliers.Insert(0, new Supplier { SupplierOil = "", Id = 0 });

            Suppliers = new SelectList(suppliers, "Id", "SupplierOil", supplierOil);
            SupplierOil = supplierOil;
        }
    }
}

