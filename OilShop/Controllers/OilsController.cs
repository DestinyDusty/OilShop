using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.ViewModels.Oil;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    public class OilsController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public OilsController(AppCtx context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Oils
        [Authorize(Roles = "admin, registeredUser")]
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var appCtx = _context.Oils
                 .Include(s => s.Brand) // связь таблиц
                 .Include(s => s.Type)
                 .Include(s => s.Viscosity)
                 .Include(s => s.Capasity)
                 .Include(s => s.Country)
                 .Include(s => s.Supplier)
                 .OrderBy(f => f.Brand); // сортировка
            return View(await appCtx.ToListAsync());
        }

        // GET: Oils/Create
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAsync()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdBrand"] = new SelectList(_context.Brands, "Id", "BrandOil");
            ViewData["IdCapasity"] = new SelectList(_context.Capasities, "Id", "CapasityOil");
            ViewData["IdCountry"] = new SelectList(_context.Countries, "Id", "CountryOrigin");
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "Id", "SupplierOil");
            ViewData["IdType"] = new SelectList(_context.Types, "Id", "TypeOil");
            ViewData["IdViscosity"] = new SelectList(_context.Viscosities, "Id", "ViscosityOil");
            return View();
        }

        // POST: Oils/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateOilViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            /*if (_context.Oils
                .Where(f => f.Brand == user.Id &&
                    f.Code == model.Code &&
                    f.Name == model.Name &&
                    f.IdFormOfStudy == model.IdFormOfStudy)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная специальность уже существует");
            }*/

            if (ModelState.IsValid)
            {
                Oil oil = new()
                {
                    DateOfManufacture = model.DateOfManufacture,
                    ExpirationDate = model.ExpirationDate,
                    PurchaseDate = model.PurchaseDate,
                    PurchasePrice = model.PurchasePrice,

                    IdBrand = model.IdBrand,
                    IdType = model.IdType,
                    IdViscosity = model.IdViscosity,
                    IdCapasity = model.IdCapasity,
                    IdCountry = model.IdCountry,
                    IdSupplier = model.IdSupplier
                };


                _context.Add(oil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBrand"] = new SelectList(_context.Brands, "Id", "BrandOil", model.IdBrand);
            ViewData["IdCapasity"] = new SelectList(_context.Capasities, "Id", "CapasityOil", model.IdCapasity);
            ViewData["IdCountry"] = new SelectList(_context.Countries, "Id", "CountryOrigin", model.IdCountry);
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "Id", "SupplierOil", model.IdSupplier);
            ViewData["IdType"] = new SelectList(_context.Types, "Id", "TypeOil", model.IdType);
            ViewData["IdViscosity"] = new SelectList(_context.Viscosities, "Id", "ViscosityOil", model.IdViscosity);
            return View(model);
        }

        // GET: Oils/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oil = await _context.Oils.FindAsync(id);
            if (oil == null)
            {
                return NotFound();
            }

            EditOilViewModel model = new()
            {
                Id = oil.Id,
                DateOfManufacture = oil.DateOfManufacture,
                ExpirationDate = oil.ExpirationDate,
                PurchaseDate = oil.PurchaseDate,
                PurchasePrice = oil.PurchasePrice,

                IdBrand = oil.IdBrand,
                IdType = oil.IdType,
                IdViscosity = oil.IdViscosity,
                IdCapasity = oil.IdCapasity,
                IdCountry = oil.IdCountry,
                IdSupplier = oil.IdSupplier
            };

            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdBrand"] = new SelectList(_context.Brands, "Id", "BrandOil", oil.IdBrand);
            ViewData["IdCapasity"] = new SelectList(_context.Capasities, "Id", "CapasityOil", oil.IdCapasity);
            ViewData["IdCountry"] = new SelectList(_context.Countries, "Id", "CountryOrigin", oil.IdCountry);
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "Id", "SupplierOil", oil.IdSupplier);
            ViewData["IdType"] = new SelectList(_context.Types, "Id", "TypeOil", oil.IdType);
            ViewData["IdViscosity"] = new SelectList(_context.Viscosities, "Id", "ViscosityOil", oil.IdViscosity);
            return View(model);
        }

        // POST: Oils/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, EditOilViewModel model)
        {
            Oil oil = await _context.Oils.FindAsync(id);

            if (id != oil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oil.DateOfManufacture = model.DateOfManufacture;
                    oil.ExpirationDate = model.ExpirationDate;
                    oil.PurchaseDate = model.PurchaseDate;
                    oil.PurchasePrice = model.PurchasePrice;

                    oil.IdBrand = model.IdBrand;
                    oil.IdType = model.IdType;
                    oil.IdViscosity = model.IdViscosity;
                    oil.IdCapasity = model.IdCapasity;
                    oil.IdCountry = model.IdCountry;
                    oil.IdSupplier = model.IdSupplier;

                    _context.Update(oil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OilExists(oil.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBrand"] = new SelectList(_context.Brands, "Id", "BrandOil", oil.IdBrand);
            ViewData["IdCapasity"] = new SelectList(_context.Capasities, "Id", "CapasityOil", oil.IdCapasity);
            ViewData["IdCountry"] = new SelectList(_context.Countries, "Id", "CountryOrigin", oil.IdCountry);
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "Id", "SupplierOil", oil.IdSupplier);
            ViewData["IdType"] = new SelectList(_context.Types, "Id", "TypeOil", oil.IdType);
            ViewData["IdViscosity"] = new SelectList(_context.Viscosities, "Id", "ViscosityOil", oil.IdViscosity);
            return View(model);
        }

        // GET: Oils/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oil = await _context.Oils
                .Include(o => o.Brand)
                .Include(o => o.Capasity)
                .Include(o => o.Country)
                .Include(o => o.Supplier)
                .Include(o => o.Type)
                .Include(o => o.Viscosity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oil == null)
            {
                return NotFound();
            }

            return View(oil);
        }

        // POST: Oils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oil = await _context.Oils.FindAsync(id);
            _context.Oils.Remove(oil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Oils/Details/5
        [Authorize(Roles = "admin, registeredUser")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oil = await _context.Oils
                .Include(o => o.Brand)
                .Include(o => o.Capasity)
                .Include(o => o.Country)
                .Include(o => o.Supplier)
                .Include(o => o.Type)
                .Include(o => o.Viscosity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oil == null)
            {
                return NotFound();
            }

            return View(oil);
        }

        private bool OilExists(int id)
        {
            return _context.Oils.Any(e => e.Id == id);
        }
    }
}
