using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.Models.Enums;
using OilShop.ViewModels.Countries;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin , manager, registeredUser")]
    public class CountriesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public CountriesController(AppCtx context, 
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Countries
        public async Task<IActionResult> Index(string country, int page = 1,
            CountrySortState sortOrder = CountrySortState.CountryAsc)
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            int pageSize = 15;

            //фильтрация
            IQueryable<Country> countries = _context.Countries;

            if (!String.IsNullOrEmpty(country))
            {
                countries = countries.Where(p => p.CountryOrigin.Contains(country));
            }

            // сортировка
            switch (sortOrder)
            {
                case CountrySortState.CountryDesc:
                    countries = countries.OrderByDescending(s => s.CountryOrigin);
                    break;
                default:
                    countries = countries.OrderBy(s => s.CountryOrigin);
                    break;
            }

            // пагинация
            var count = await countries.CountAsync();
            var items = await countries.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexCountryViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                SortCountryViewModel = new(sortOrder),
                FilterCountryViewModel = new(country),
                Countries = items
            };
            // возвращаем в представление полученный список записей
            return View(viewModel);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCountryViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Countries
                    .Where(f => f.CountryOrigin == model.CountryOrigin).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная страна производителя уже существует");
            }

            if (ModelState.IsValid)
            {
                Country country = new()
                {
                    CountryOrigin = model.CountryOrigin
                };
                
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            EditCountryViewModel model = new()
            {
                Id = country.Id,
                CountryOrigin = country.CountryOrigin
            };

            return View(model);
        }

        // POST: Countries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditCountryViewModel model)
        {
            if (_context.Countries
                    .Where(f => f.CountryOrigin == model.CountryOrigin).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная страна производителя уже существует");
            }

            Country country = await _context.Countries.FindAsync(id);

            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    country.CountryOrigin = model.CountryOrigin;
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var country = await _context.Countries.FindAsync(id);
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Countries/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        private bool CountryExists(short id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
