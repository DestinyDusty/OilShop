using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.ViewModels.Viscosities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class ViscositiesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public ViscositiesController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Viscosities
        public async Task<IActionResult> Index(string viscosity, int page = 1)
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            int pageSize = 10;

            //фильтрация
            IQueryable<Viscosity> viscosities = _context.Viscosities;

            if (!String.IsNullOrEmpty(viscosity))
            {
                viscosities = viscosities.Where(p => p.ViscosityOil.Contains(viscosity));
            }

            // пагинация
            var count = await viscosities.CountAsync();
            var items = await viscosities.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViscosityViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                FilterViscosityViewModel = new(viscosity),
                Viscosities = items
            };

            // возвращаем в представление полученный список записей
            return View(viewModel);
        }

        // GET: Viscosities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Viscosities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViscosityViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Viscosities
                    .Where(f => f.ViscosityOil == model.ViscosityOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная вязкость уже существует");
            }

            if (ModelState.IsValid)
            {
                Viscosity viscosity = new()
                {
                    ViscosityOil = model.ViscosityOil
                };

                _context.Add(viscosity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Viscosities/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viscosity = await _context.Viscosities.FindAsync(id);
            if (viscosity == null)
            {
                return NotFound();
            }

            EditViscosityViewModel model = new()
            {
                Id = viscosity.Id,
                ViscosityOil = viscosity.ViscosityOil
            };

            return View(model);
        }

        // POST: Viscosities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditViscosityViewModel model)
        {
            if (_context.Viscosities
                    .Where(f => f.ViscosityOil == model.ViscosityOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная вязкость уже существует");
            }

            Viscosity viscosity = await _context.Viscosities.FindAsync(id);

            if (id != viscosity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    viscosity.ViscosityOil = model.ViscosityOil;
                    _context.Update(viscosity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViscosityExists(viscosity.Id))
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

        // GET: Viscosities/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viscosity = await _context.Viscosities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viscosity == null)
            {
                return NotFound();
            }

            return View(viscosity);
        }

        // POST: Viscosities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var viscosity = await _context.Viscosities.FindAsync(id);
            _context.Viscosities.Remove(viscosity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Viscosities/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viscosity = await _context.Viscosities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viscosity == null)
            {
                return NotFound();
            }

            return View(viscosity);
        }

        private bool ViscosityExists(short id)
        {
            return _context.Viscosities.Any(e => e.Id == id);
        }
    }
}
