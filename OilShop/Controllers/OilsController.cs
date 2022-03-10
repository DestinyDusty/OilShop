using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.Models.Enums;
using OilShop.ViewModels.Oil;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class OilsController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager; 
        private readonly IWebHostEnvironment _appEnvironment;

        public OilsController(AppCtx context, UserManager<User> user, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = user;
            _appEnvironment = appEnvironment;
        }

        // GET: Oils
        /*public async Task<IActionResult> Index(string Photo, string Path, 
            DateTime DateOfManufacture, DateTime ExpirationDate, decimal PurchasePrice, 
            DateTime PurchaseDate, int page = 1, OilSortState sortOil = OilSortState.CodeAsc)*/
        /*public async Task<IActionResult> Index(OilSortState sortOrder = OilSortState.OilAsc)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var oils = _context.Oils
                 .Include(s => s.Brand) // связь таблиц
                 .Include(s => s.Type)
                 .Include(s => s.Viscosity)
                 .Include(s => s.Capasity)
                 .Include(s => s.Country)
                 .Include(s => s.Supplier);

            ViewData["OilSort"] = sortOrder == OilSortState.OilAsc ? OilSortState.OilDesc : OilSortState.OilAsc;

            oils = sortOrder switch
            {
                OilSortState.OilDesc => oils.OrderByDescending(s => s.Brand),
                _=> oils.OrderBy(s => s.Brand),
            };

            return View(await oils.AsNoTracking().ToListAsync());
        }*/

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

            if (model.UploadedFile == null)
            {
                ModelState.AddModelError("","Файл не был загружен");
            }

            if (_context.Oils
                .Where(f => f.IdBrand == model.IdBrand &&
                    f.IdCapasity == model.IdCapasity &&
                    f.IdCountry == model.IdCountry &&
                    f.IdSupplier == model.IdSupplier &&
                    f.IdType == model.IdType &&
                    f.IdViscosity == model.IdViscosity)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенное масло уже существует");
            }

            if (ModelState.IsValid)
            {
                // путь к папке images
                string path = "/images/" + model.UploadedFile.FileName;
                // сохраняем файл в папку images в каталоге wwwroot
                using (var filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await model.UploadedFile.CopyToAsync(filestream);
                }

                Oil oil = new()
                {
                    Photo = model.UploadedFile.FileName,
                    Path = path,
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

            if (model.UploadedFile == null)
            {
                ModelState.AddModelError("", "Файл не был загружен");
            }

            if (ModelState.IsValid)
            {
                // путь к папке images
                string path = "/images/" + model.UploadedFile.FileName;
                // сохраняем файл в папку images в каталоге wwwroot
                using (var filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await model.UploadedFile.CopyToAsync(filestream);
                }

                try
                {
                    oil.Photo = model.Photo;
                    oil.Path = path;
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
