using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.ViewModels.Types;
using System;
using System.Linq;
using System.Threading.Tasks;
using Type = OilShop.Models.Data.Type;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin , manager, registeredUser")]
    public class TypesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public TypesController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Types
        public async Task<IActionResult> Index()
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // через контекст данных получаем доступ к таблице базы данных
            var appCtx = _context.Types
                .OrderBy(f => f.TypeOil);

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }


        // GET: Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Types/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTypeViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Types
                    .Where(f => f.TypeOil == model.TypeOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный тип уже существует");
            }

            if (ModelState.IsValid)
            {
                Type type = new()
                {
                    TypeOil = model.TypeOil
                };

                _context.Add(type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Types/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var type = await _context.Types.FindAsync(id);
            if (type == null)
            {
                return NotFound();
            }

            EditTypeViewModel model = new()
            {
                Id = type.Id,
                TypeOil = type.TypeOil
            };

            return View(model);
        }

        // POST: Types/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditTypeViewModel model)
        {
            if (_context.Types
                    .Where(f => f.TypeOil == model.TypeOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный тип уже существует");
            }

            Type type = await _context.Types.FindAsync(id);

            if (id != type.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    type.TypeOil = model.TypeOil;
                    _context.Update(type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(type.Id))
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

        // GET: Types/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var type = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (type == null)
            {
                return NotFound();
            }

            return View(type);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var type = await _context.Types.FindAsync(id);
            _context.Types.Remove(type);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Types/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var type = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (type == null)
            {
                return NotFound();
            }

            return View(type);
        }

        private bool TypeExists(short id)
        {
            return _context.Types.Any(e => e.Id == id);
        }
    }
}
