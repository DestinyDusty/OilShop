using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.ViewModels.Viscosity;
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
        public async Task<IActionResult> Index()
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // через контекст данных получаем доступ к таблице базы данных
            var appCtx = _context.Viscosities
                .OrderBy(f => f.ViscosityOil);

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
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
