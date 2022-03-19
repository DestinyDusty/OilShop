using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OilShop.ViewModels.AddressDelivery;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class AddressDeliveriesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public AddressDeliveriesController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: AddressDeliveries
        public async Task<IActionResult> Index()
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // через контекст данных получаем доступ к таблице базы данных
            var appCtx = _context.AddressDeliveries
                .OrderBy(f => f.Street);

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }

        // GET: AddressDeliveries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AddressDeliveries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAddressDeliveryViewModel model)
        {
            if (_context.AddressDeliveries
                    .Where(f => f.Street == model.Street &&
                    f.House == model.House).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный адрес уже существует");
            }

            if (ModelState.IsValid)
            {
                AddressDelivery addressDelivery = new()
                {
                    Street = model.Street,
                    House = model.House
                };

                _context.Add(addressDelivery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AddressDeliveries/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressDelivery = await _context.AddressDeliveries.FindAsync(id);
            if (addressDelivery == null)
            {
                return NotFound();
            }

            EditAddressDeliveryViewModel model = new()
            {
                Id = addressDelivery.Id,
                Street = addressDelivery.Street,
                House = addressDelivery.House
            };

            return View(model);
        }

        // POST: AddressDeliveries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditAddressDeliveryViewModel model)
        {
            if (_context.AddressDeliveries
                    .Where(f => f.Street == model.Street &&
                    f.House == model.House).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный адрес уже существует");
            }

            AddressDelivery addressDelivery = await _context.AddressDeliveries.FindAsync(id);

            if (id != addressDelivery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    addressDelivery.Street = model.Street;
                    addressDelivery.House = model.House;
                    _context.Update(addressDelivery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressDeliveryExists(addressDelivery.Id))
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

        // GET: AddressDeliveries/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressDelivery = await _context.AddressDeliveries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addressDelivery == null)
            {
                return NotFound();
            }

            return View(addressDelivery);
        }

        // POST: AddressDeliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var addressDelivery = await _context.AddressDeliveries.FindAsync(id);
            _context.AddressDeliveries.Remove(addressDelivery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AddressDeliveries/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressDelivery = await _context.AddressDeliveries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addressDelivery == null)
            {
                return NotFound();
            }

            return View(addressDelivery);
        }

        private bool AddressDeliveryExists(short id)
        {
            return _context.AddressDeliveries.Any(e => e.Id == id);
        }
    }
}
