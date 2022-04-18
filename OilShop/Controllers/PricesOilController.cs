using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.ViewModels.PricesOil;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class PricesOilController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public PricesOilController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: PricesOil
        public async Task<IActionResult> Index()
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var appCtx = _context.PricesOil.Include(p => p.Oil);
            
            return View(await appCtx.ToListAsync());
        }

        // GET: PricesOil/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceOil = await _context.PricesOil
                .Include(p => p.Oil)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (priceOil == null)
            {
                return NotFound();
            }

            return View(priceOil);
        }

        // GET: PricesOil/Create
        public async Task<IActionResult> Create()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var oil = _context.Oils
                .OrderBy(w => w.Brand)
                .ToList();

            ViewData["IdOil"] = new SelectList(_context.Oils, "Id", "Id");
            return View();
        }

        // POST: PricesOil/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Create(int id, CreatePriceOilViewModel model)
        {
            var oils = await _context.Oils.FindAsync(id);
            
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.PricesOil
                .Where(f => f.Price == model.Price)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенная цена уже существует");
            }

            if (_context.PricesOil
                .Where(f => f.PriceSettingDate == model.PriceSettingDate)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенная дата уставновки цены уже существует");
            }

            if (ModelState.IsValid)
            {
                PriceOil priceOil = new()
                {
                    PriceSettingDate = model.PriceSettingDate,
                    Price = model.Price,

                    IdOil = id
                };

                _context.Add(priceOil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdOil"] = new SelectList(_context.Oils, "Id", "Id", model.IdOil);
            return View(model);
        }

        // GET: PricesOil/Edit/5
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceOil = await _context.PricesOil.FindAsync(id);
            if (priceOil == null)
            {
                return NotFound();
            }

            EditPriceOilViewModel model = new()
            {
                Id = priceOil.Id,
                PriceSettingDate = priceOil.PriceSettingDate,
                Price = priceOil.Price,

                IdOil = priceOil.IdOil
            };

            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdOil"] = new SelectList(_context.Oils, "Id", "Id", priceOil.IdOil);
            return View(model);
        }

        // POST: PricesOil/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id, EditPriceOilViewModel model)
        {
            if (_context.PricesOil
                .Where(f => f.Price == model.Price)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенная цена уже существует");
            }

            //if (_context.PricesOil
            //    .Where(f => f.PriceSettingDate == model.PriceSettingDate)
            //    .FirstOrDefault() != null)
            //{
            //    ModelState.AddModelError("", "Введенная дата уставновки цены уже существует");
            //}

            PriceOil priceOil = await _context.PricesOil.FindAsync(id);

            if (id != priceOil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    priceOil.Price = model.Price;
                    priceOil.PriceSettingDate = model.PriceSettingDate;

                    priceOil.IdOil = model.IdOil;

                    _context.Update(priceOil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceOilExists(priceOil.Id))
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
            ViewData["IdOil"] = new SelectList(_context.Oils, "Id", "Id", priceOil.IdOil);
            return View(model);
        }

        // GET: PricesOil/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceOil = await _context.PricesOil
                .Include(p => p.Oil)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (priceOil == null)
            {
                return NotFound();
            }

            return View(priceOil);
        }

        // POST: PricesOil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priceOil = await _context.PricesOil.FindAsync(id);
            _context.PricesOil.Remove(priceOil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceOilExists(int id)
        {
            return _context.PricesOil.Any(e => e.Id == id);
        }
    }
}
