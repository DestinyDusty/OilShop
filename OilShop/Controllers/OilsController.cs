using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.Models.Enums;
using OilShop.ViewModels.Oils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, manager, registeredUser")]
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

        // GET:
        [AllowAnonymous]
        public IActionResult Index(int page = 1)
        {
            int pageSize = 15;

            List<PriceOil> LastPriceOil = new List<PriceOil>();

            foreach (int id in _context.PricesOil.OrderBy(f => f.IdOil).Select(f => f.IdOil).Distinct())
            {
                PriceOil priceOil = _context.PricesOil
                 .Include(o => o.Oil.Brand)
                 .Include(o => o.Oil.Type)
                 .Include(o => o.Oil.Viscosity)
                 .Include(o => o.Oil.Capasity)
                 .Include(o => o.Oil.Country)
                 .Include(o => o.Oil.Supplier)
                .Where(f => f.IdOil == id)
                .OrderByDescending(f => f.PriceSettingDate)
                .FirstOrDefault();

                LastPriceOil.Add(priceOil);
            }
            return View(LastPriceOil);
        }

        public async Task<IActionResult> Table(short? brand, short? type, short? viscosity,
            short? capasity, short? country, short? supplier, DateTime dateOfManufacture,
            DateTime expirationDate, string purchasePrice, DateTime purchaseDate, int page = 1,
            OilSortState sortOrder = OilSortState.BrandAsc)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            int pageSize = 10;

            IQueryable<Oil> oils = _context.Oils.Include(o => o.PricesOil)
                 .Include(s => s.Brand)
                 .Include(s => s.Type)
                 .Include(s => s.Viscosity)
                 .Include(s => s.Capasity)
                 .Include(s => s.Country)
                 .Include(s => s.Supplier);

            if (brand != null && brand != 0)
            {
                oils = oils.Where(p => p.IdBrand == brand);
            }
            if (type != null && type != 0)
            {
                oils = oils.Where(p => p.IdType == type);
            }
            if (viscosity != null && viscosity != 0)
            {
                oils = oils.Where(p => p.IdViscosity == viscosity);
            }
            if (capasity != null && capasity != 0)
            {
                oils = oils.Where(p => p.IdCapasity == capasity);
            }
            if (country != null && country != 0)
            {
                oils = oils.Where(p => p.IdCountry == country);
            }
            if (supplier != null && supplier != 0)
            {
                oils = oils.Where(p => p.IdSupplier == supplier);
            }

            // сортировка
            switch (sortOrder)
            {
                case OilSortState.BrandDesc:
                    oils = oils.OrderByDescending(s => s.Brand.BrandOil);
                    break;
                case OilSortState.TypeDesc:
                    oils = oils.OrderBy(s => s.Type.TypeOil);
                    break;
                case OilSortState.TypeAsc:
                    oils = oils.OrderByDescending(s => s.Type.TypeOil);
                    break;
                case OilSortState.ViscosityDesc:
                    oils = oils.OrderBy(s => s.Viscosity.ViscosityOil);
                    break;
                case OilSortState.ViscosityAsc:
                    oils = oils.OrderByDescending(s => s.Viscosity.ViscosityOil);
                    break;
                case OilSortState.CapasityDesc:
                    oils = oils.OrderBy(s => s.Capasity.CapasityOil);
                    break;
                case OilSortState.CapasityAsc:
                    oils = oils.OrderByDescending(s => s.Capasity.CapasityOil);
                    break;
                case OilSortState.CountryDesc:
                    oils = oils.OrderBy(s => s.Country.CountryOrigin);
                    break;
                case OilSortState.CountryAsc:
                    oils = oils.OrderByDescending(s => s.Country.CountryOrigin);
                    break;
                case OilSortState.SupplierDesc:
                    oils = oils.OrderBy(s => s.Supplier.SupplierOil);
                    break;
                case OilSortState.SupplierAsc:
                    oils = oils.OrderByDescending(s => s.Supplier.SupplierOil);
                    break;
                case OilSortState.DateOfManufactureDesc:
                    oils = oils.OrderBy(s => s.DateOfManufacture);
                    break;
                case OilSortState.DateOfManufactureAsc:
                    oils = oils.OrderByDescending(s => s.DateOfManufacture);
                    break;
                case OilSortState.ExpirationDateDesc:
                    oils = oils.OrderBy(s => s.ExpirationDate);
                    break;
                case OilSortState.ExpirationDateAsc:
                    oils = oils.OrderByDescending(s => s.ExpirationDate);
                    break;
                case OilSortState.PurchasePriceDesc:
                    oils = oils.OrderBy(s => s.PurchasePrice);
                    break;
                case OilSortState.PurchasePriceAsc:
                    oils = oils.OrderByDescending(s => s.PurchasePrice);
                    break;
                case OilSortState.PurchaseDateDesc:
                    oils = oils.OrderBy(s => s.PurchaseDate);
                    break;
                case OilSortState.PurchaseDateAsc:
                    oils = oils.OrderByDescending(s => s.PurchaseDate);
                    break;
                default:
                    oils = oils.OrderBy(s => s.Brand);
                    break;
            }

            // пагинация
            var count = await oils.CountAsync();
            var items = await oils.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexOilViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                //SortOilViewModel = new(sortOrder),
                //FilterOilViewModel = new(code, name, _context.FormsOfStudy.ToList(), formOfEdu),
                //Oil = items
            };

            // возвращаем в представление полученный список записей
            return View(viewModel);
        }

        // GET: Oils/Create
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> CreateAsync()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdBrand"] = new SelectList(_context.Brands, "Id", "BrandOil");
            ViewData["IdCapasity"] = new SelectList(_context.Capasities, "Id", "CapasityOil");
            ViewData["IdCountry"] = new SelectList(_context.Countries, "Id", "CountryOrigin");
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "Id", "SupplierOil");
            ViewData["IdType"] = new SelectList(_context.Types, "Id", "TypeOil");
            ViewData["IdViscosity"] = new SelectList(_context.Viscosities, "Id", "ViscosityOil");
            ViewData["IdPrice"] = new SelectList(_context.PricesOil, "Id", "PriceOil");
            return View();
        }

        // POST: Oils/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Create(CreateOilViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (model.UploadedFile == null)
            {
                ModelState.AddModelError("","Файл не был загружен");
            }

            //if (_context.Oils
            //    .Where(f => f.IdBrand == model.IdBrand &&
            //        f.IdCapasity == model.IdCapasity &&
            //        f.IdCountry == model.IdCountry &&
            //        f.IdSupplier == model.IdSupplier &&
            //        f.IdType == model.IdType &&
            //        f.IdViscosity == model.IdViscosity
            //        )
            //    .FirstOrDefault() != null)
            //{
            //    ModelState.AddModelError("", "Введенное масло уже существует");
            //}

            if (model.DateOfManufacture > model.ExpirationDate)
            {
                ModelState.AddModelError("", "Дата изготовления должна быть меньше на 5 лет, чем дата окончания срока годности");
            }

            if (ModelState.IsValid)
            {
                // путь к папке images
                string path = @"\images\" + model.UploadedFile.FileName;
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
                    PurchasePrice = Convert.ToDecimal(model.PurchasePriceRub)+ Convert.ToDecimal(model.PurchasePrice)/100,

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
        [Authorize(Roles = "admin,manager")]
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
                PurchasePriceRub = Convert.ToInt32(Math.Truncate(oil.PurchasePrice)),
                PurchasePrice = Convert.ToInt32(oil.PurchasePrice - Math.Truncate(oil.PurchasePrice)),

                Photo = oil.Photo,
                Path = oil.Path,

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
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id, EditOilViewModel model)
        {
            //if (_context.Oils
            //    .Where(f => f.IdBrand == model.IdBrand &&
            //        f.IdCapasity == model.IdCapasity &&
            //        f.IdCountry == model.IdCountry &&
            //        f.IdSupplier == model.IdSupplier &&
            //        f.IdType == model.IdType &&
            //        f.IdViscosity == model.IdViscosity)
            //    .FirstOrDefault() != null)
            //{
            //    ModelState.AddModelError("", "Введенное масло уже существует");
            //}

            Oil oil = await _context.Oils.FindAsync(id);

            if (id != oil.Id)
            {
                return NotFound();
            }

            //if (model.UploadedFile == null)
            //{
            //    ModelState.AddModelError("", "Файл не был загружен");
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    if (oil.Path != model.Path && oil.Photo != model.Photo)
                    {
                        string fullPath = _appEnvironment.WebRootPath + oil.Path;
                        if (System.IO.File.Exists(fullPath))
                            System.IO.File.Delete(fullPath);
                        
                        //// путь к папке images
                        //string path = "/images/" + model.UploadedFile.FileName;
                        //// сохраняем файл в папку images в каталоге wwwroot
                        //using (var filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        //{
                        //    await model.UploadedFile.CopyToAsync(filestream);
                        //}

                        //oil.Photo = model.UploadedFile.FileName;
                        //oil.Path = path;
                    }

                    oil.DateOfManufacture = model.DateOfManufacture;
                    oil.ExpirationDate = model.ExpirationDate;
                    oil.PurchaseDate = model.PurchaseDate;
                    oil.PurchasePrice = Convert.ToDecimal(model.PurchasePriceRub) + Convert.ToDecimal(model.PurchasePrice) / 100;

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
        [Authorize(Roles = "admin,manager")]
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
        [Authorize(Roles = "admin,manager")]
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
