using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.Models.Enums;
using OilShop.ViewModels.Brands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, manager, registeredUser")]
    public class BrandsController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;


        public BrandsController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Brands
        public async Task<IActionResult> Index(string brand, int page = 1,
            BrandSortState sortOrder = BrandSortState.BrandOilAsc)
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // через контекст данных получаем доступ к таблице базы данных
            var brandOil = _context.Brands
                .OrderBy(f => f.BrandOil);
            int pageSize = 10;

            //фильтрация
            IQueryable<Brand> brands = _context.Brands;

            if (!String.IsNullOrEmpty(brand))
            {
                brands = brands.Where(p => p.BrandOil.Contains(brand));
            }

            // сортировка
            switch (sortOrder)
            {
                case BrandSortState.BrandOilDesc:
                    brands = brands.OrderByDescending(s => s.BrandOil);
                    break;
                default:
                    brands = brands.OrderBy(s => s.BrandOil);
                    break;
            }

            // пагинация
            var count = await brands.CountAsync();
            var items = await brands.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexBrandViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                SortBrandViewModel = new(sortOrder),
                FilterBrandViewModel = new (brand),
                Brands = items
            };

            // возвращаем в представление полученный список записей
            return View(viewModel);
        }


        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBrandViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Brands
                    .Where(f=> f.BrandOil == model.BrandOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный бренд уже существует");
            }

            if (ModelState.IsValid)
            {
                Brand brand = new()
                {
                    BrandOil = model.BrandOil
                };

                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            EditBrandViewModel model = new()
            {
                Id = brand.Id,
                BrandOil = brand.BrandOil
            };

            return View(model);
        }

        // POST: Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditBrandViewModel model)
        {
            if (_context.Brands
                    .Where(f => f.BrandOil == model.BrandOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный бренд уже существует");
            }

            Brand brand = await _context.Brands.FindAsync(id);

            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    brand.BrandOil = model.BrandOil;
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            return View(model);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var brand = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        private bool BrandExists(short id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}
