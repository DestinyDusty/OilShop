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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        //// GET:
        //[AllowAnonymous]
        //public IActionResult Index(short? brandOil, short? typeOil, short? viscosityOil,
        //    short? capasityOil, short? countryOrigin, short? supplierOil, int page = 1,
        //    OilSortState sortOrder = OilSortState.BrandAsc)
        //{
        //    //int pageSize = 15;

        //    List<PriceOil> LastPriceOil = new List<PriceOil>();

        //    foreach (int id in _context.PricesOil.OrderBy(f => f.IdOil).Select(f => f.IdOil).Distinct())
        //    {
        //        PriceOil priceOil = _context.PricesOil
        //         .Include(o => o.Oil.Brand)
        //         .Include(o => o.Oil.Type)
        //         .Include(o => o.Oil.Viscosity)
        //         .Include(o => o.Oil.Capasity)
        //         .Include(o => o.Oil.Country)
        //         .Include(o => o.Oil.Supplier)
        //        .Where(f => f.IdOil == id)
        //        .OrderByDescending(f => f.PriceSettingDate)
        //        .FirstOrDefault();

        //        LastPriceOil.Add(priceOil);
        //    }

        //    return View(LastPriceOil);
        //}

        // GET:
        [AllowAnonymous]
        public async Task<IActionResult> Index(short? brandOil, short? typeOil, short? viscosityOil,
            short? capasityOil, short? countryOrigin, short? supplierOil, int page = 1,
            OilSortState sortOrder = OilSortState.BrandAsc)
        {
            //int pageSize = 15;

            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var oils = _context.Oils
                 .Include(s => s.Brand) // связь таблиц
                 .Include(s => s.Type)
                 .Include(s => s.Viscosity)
                 .Include(s => s.Capasity)
                 .Include(s => s.Country)
                 .Include(s => s.Supplier)
                 .OrderBy(f => f.Brand); // сортировка
            return View(await oils.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Table(short? brandOil, short? typeOil, short? viscosityOil,
            short? capasityOil, short? countryOrigin, short? supplierOil, DateTime dateOfManufacture,
            DateTime expirationDate, decimal purchasePrice, DateTime purchaseDate, int page = 1,
            OilSortState sortOrder = OilSortState.BrandAsc)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            int pageSize = 5;

            IQueryable<Oil> oils = _context.Oils.Include(o => o.PricesOil)
                 .Include(s => s.Brand)
                 .Include(s => s.Type)
                 .Include(s => s.Viscosity)
                 .Include(s => s.Capasity)
                 .Include(s => s.Country)
                 .Include(s => s.Supplier);

            if (brandOil != null && brandOil != 0)
            {
                oils = oils.Where(p => p.IdBrand == brandOil);
            }
            if (typeOil != null && typeOil != 0)
            {
                oils = oils.Where(p => p.IdType == typeOil);
            }
            if (viscosityOil != null && viscosityOil != 0)
            {
                oils = oils.Where(p => p.IdViscosity == viscosityOil);
            }
            if (capasityOil != null && capasityOil != 0)
            {
                oils = oils.Where(p => p.IdCapasity == capasityOil);
            }
            if (countryOrigin != null && countryOrigin != 0)
            {
                oils = oils.Where(p => p.IdCountry == countryOrigin);
            }
            if (supplierOil != null && supplierOil != 0)
            {
                oils = oils.Where(p => p.IdSupplier == supplierOil);
            }

            /*oils = oils.Where(p => p.DateOfManufacture== dateOfManufacture);
            oils = oils.Where(p => p.ExpirationDate == expirationDate);
            oils = oils.Where(p => p.PurchaseDate == purchaseDate);*/

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
                SortOilViewModel = new(sortOrder),
                FilterOilViewModel = new(
                    _context.Brands.ToList(), brandOil,
                    _context.Types.ToList(), typeOil,
                    _context.Viscosities.ToList(), viscosityOil,
                    _context.Capasities.ToList(), capasityOil,
                    _context.Countries.ToList(), countryOrigin,
                    _context.Suppliers.ToList(), supplierOil),
                Oils = items
            };

            // возвращаем в представление полученный список записей
            return View(viewModel);
        }

        // GET: Oils/Create
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> CreateAsync()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdBrand"] = new SelectList(_context.Brands.OrderBy(f => f.BrandOil), "Id", "BrandOil");
            ViewData["IdCapasity"] = new SelectList(_context.Capasities.OrderBy(f => f.CapasityOil), "Id", "CapasityOil");
            ViewData["IdCountry"] = new SelectList(_context.Countries.OrderBy(f => f.CountryOrigin), "Id", "CountryOrigin");
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers.OrderBy(f => f.SupplierOil), "Id", "SupplierOil");
            ViewData["IdType"] = new SelectList(_context.Types.OrderBy(f => f.TypeOil), "Id", "TypeOil");
            ViewData["IdViscosity"] = new SelectList(_context.Viscosities.OrderBy(f => f.ViscosityOil), "Id", "ViscosityOil");
            return View();
        }

        // POST: Oils/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Create(CreateOilViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

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

            if (model.Photo == null)
                ModelState.AddModelError("", "Фото не загружено");

            if (model.Photo != null)
            {
                /*// путь к папке images
                string path = @"\images\" + model.UploadedFile.FileName;
                // сохраняем файл в папку images в каталоге wwwroot*/
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(model.Photo.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.Photo.Length);
                }

                Oil oil = new()
                {
                    Photo = imageData,
                    DateOfManufacture = model.DateOfManufacture,
                    ExpirationDate = model.ExpirationDate,

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
            ViewData["IdBrand"] = new SelectList(_context.Brands.OrderBy(f => f.BrandOil), "Id", "BrandOil");
            ViewData["IdCapasity"] = new SelectList(_context.Capasities.OrderBy(f => f.CapasityOil), "Id", "CapasityOil");
            ViewData["IdCountry"] = new SelectList(_context.Countries.OrderBy(f => f.CountryOrigin), "Id", "CountryOrigin");
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers.OrderBy(f => f.SupplierOil), "Id", "SupplierOil");
            ViewData["IdType"] = new SelectList(_context.Types.OrderBy(f => f.TypeOil), "Id", "TypeOil");
            ViewData["IdViscosity"] = new SelectList(_context.Viscosities.OrderBy(f => f.ViscosityOil), "Id", "ViscosityOil");
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

                //Photo = oil.Photo,

                IdBrand = oil.IdBrand,
                IdType = oil.IdType,
                IdViscosity = oil.IdViscosity,
                IdCapasity = oil.IdCapasity,
                IdCountry = oil.IdCountry,
                IdSupplier = oil.IdSupplier
            };

            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdBrand"] = new SelectList(_context.Brands.OrderBy(f => f.BrandOil), "Id", "BrandOil", model.IdBrand);
            ViewData["IdCapasity"] = new SelectList(_context.Capasities.OrderBy(f => f.CapasityOil), "Id", "CapasityOil", model.IdCapasity);
            ViewData["IdCountry"] = new SelectList(_context.Countries.OrderBy(f => f.CountryOrigin), "Id", "CountryOrigin", model.IdCountry);
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers.OrderBy(f => f.SupplierOil), "Id", "SupplierOil", model.IdSupplier);
            ViewData["IdType"] = new SelectList(_context.Types.OrderBy(f => f.TypeOil), "Id", "TypeOil", model.IdType);
            ViewData["IdViscosity"] = new SelectList(_context.Viscosities.OrderBy(f => f.ViscosityOil), "Id", "ViscosityOil", model.IdViscosity);
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
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(model.Photo.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Photo.Length);
                    }

                    string photoModel = Encoding.Default.GetString(imageData.Where(x => x != 0).ToArray());
                    string photoOil = Encoding.Default.GetString(oil.Photo.Where(x => x != 0).ToArray());

                    if (photoModel != photoOil)
                    {

                        oil.Photo = null;
                        oil.Photo = imageData;
                    }

                    //if (oil.Path != model.Path && oil.Photo != model.Photo)
                    //{
                    //    string fullPath = _appEnvironment.WebRootPath + oil.Path;
                    //    if (System.IO.File.Exists(fullPath))
                    //        System.IO.File.Delete(fullPath);

                    //    //// путь к папке images
                    //    //string path = "/images/" + model.UploadedFile.FileName;
                    //    //// сохраняем файл в папку images в каталоге wwwroot
                    //    //using (var filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    //    //{
                    //    //    await model.UploadedFile.CopyToAsync(filestream);
                    //    //}

                    //    //oil.Photo = model.UploadedFile.FileName;
                    //    //oil.Path = path;
                    //}

                    oil.DateOfManufacture = model.DateOfManufacture;
                    oil.ExpirationDate = model.ExpirationDate;

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
            ViewData["IdBrand"] = new SelectList(_context.Brands.OrderBy(f => f.BrandOil), "Id", "BrandOil", model.IdBrand);
            ViewData["IdCapasity"] = new SelectList(_context.Capasities.OrderBy(f => f.CapasityOil), "Id", "CapasityOil", model.IdCapasity);
            ViewData["IdCountry"] = new SelectList(_context.Countries.OrderBy(f => f.CountryOrigin), "Id", "CountryOrigin", model.IdCountry);
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers.OrderBy(f => f.SupplierOil), "Id", "SupplierOil", model.IdSupplier);
            ViewData["IdType"] = new SelectList(_context.Types.OrderBy(f => f.TypeOil), "Id", "TypeOil", model.IdType);
            ViewData["IdViscosity"] = new SelectList(_context.Viscosities.OrderBy(f => f.ViscosityOil), "Id", "ViscosityOil", model.IdViscosity);
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
