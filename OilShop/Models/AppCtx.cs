using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OilShop.Models.Data;

namespace OilShop.Models
{
    public class AppCtx : IdentityDbContext<User>
    {
        public AppCtx(DbContextOptions<AppCtx> options)
            : base(options)

        {
            Database.EnsureCreated();
        }

        public DbSet<AddressDelivery> AddressDeliveries  { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Capasity> Capasities { get; set; }
        public DbSet<Country> Countries  { get; set; }
        public DbSet<Oil> Oils  { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PriceOil> PricesOil  { get; set; }
        public DbSet<StatusOrder> StatusesOrder { get; set; }
        public DbSet<Supplier> Suppliers  { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Viscosity> Viscosities  { get; set; }
        public DbSet<OrderList> OrdersList { get; set; }
        public DbSet<OilInStock> OilsInStock { get; set; }


    }
}