using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Models.Enums
{
    public enum OilSortState
    {
        BrandAsc, //по имени по возрастанию
        BrandDesc, //по имени по убыванию

        TypeAsc,
        TypeDesc,

        ViscosityAsc,
        ViscosityDesc,

        CapasityAsc,
        CapasityDesc,

        CountryAsc,
        CountryDesc,

        SupplierAsc,
        SupplierDesc,

        DateOfManufactureAsc,
        DateOfManufactureDesc,

        ExpirationDateAsc,
        ExpirationDateDesc,

        PurchasePriceAsc,
        PurchasePriceDesc,

        PurchaseDateAsc,
        PurchaseDateDesc,
    }
}
