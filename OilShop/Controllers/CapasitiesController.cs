using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.ViewModels.Capasities;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin , manager, registeredUser")]
    public class CapasitiesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public CapasitiesController(AppCtx context, 
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Capasities
        public async Task<IActionResult> Index()
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // через контекст данных получаем доступ к таблице базы данных
            var appCtx = _context.Capasities
                .OrderBy(f => f.CapasityOil);

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }

        // GET: Capasities/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capasity = await _context.Capasities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (capasity == null)
            {
                return NotFound();
            }

            return View(capasity);
        }

        // GET: Capasities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Capasities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCapasityViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Capasities
                    .Where(f => f.CapasityOil == model.CapasityOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный объем уже существует");
            }

            if (ModelState.IsValid)
            {
                Capasity capasity = new()
                {
                    CapasityOil = model.CapasityOil
                };

                _context.Add(capasity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Capasities/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capasity = await _context.Capasities.FindAsync(id);
            if (capasity == null)
            {
                return NotFound();
            }
            
            EditCapasityViewModel model = new()
            {
                Id = capasity.Id,
                CapasityOil = capasity.CapasityOil
            };

            return View(model);
        }

        // POST: Capasities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditCapasityViewModel model)
        {
            if (_context.Capasities
                    .Where(f => f.CapasityOil == model.CapasityOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный объем уже существует");
            }

            Capasity capasity = await _context.Capasities.FindAsync(id);
            
            if (id != capasity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    capasity.CapasityOil = model.CapasityOil;
                    _context.Update(capasity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapasityExists(capasity.Id))
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

        // GET: Capasities/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capasity = await _context.Capasities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (capasity == null)
            {
                return NotFound();
            }

            return View(capasity);
        }

        // POST: Capasities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var capasity = await _context.Capasities.FindAsync(id);
            _context.Capasities.Remove(capasity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }        

        private bool CapasityExists(short id)
        {
            return _context.Capasities.Any(e => e.Id == id);
        }
    }
}
