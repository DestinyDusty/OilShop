using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.Models.Enums;
using OilShop.ViewModels.StatusesOrder;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class StatusesOrderController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public StatusesOrderController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: StatusesOrder
        public async Task<IActionResult> Index(string statusOrder, int page = 1,
            StatusOrderSortState sortOrder = StatusOrderSortState.StatusAsc)
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            int pageSize = 10;

            //фильтрация
            IQueryable<StatusOrder> statuses = _context.StatusesOrder;

            if (!String.IsNullOrEmpty(statusOrder))
            {
                statuses = statuses.Where(p => p.Status.Contains(statusOrder));
            }

            // сортировка
            switch (sortOrder)
            {
                case StatusOrderSortState.StatusDesc:
                    statuses = statuses.OrderByDescending(s => s.Status);
                    break;
                default:
                    statuses = statuses.OrderBy(s => s.Status);
                    break;
            }

            // пагинация
            var count = await statuses.CountAsync();
            var items = await statuses.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexStatusOrderViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                SortStatusOrderViewModel = new(sortOrder),
                FilterStatusOrderViewModel = new(statusOrder),
                Statuses = items
            };

            // возвращаем в представление полученный список записей
            return View(viewModel);
        }




        // GET: StatusesOrder/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusesOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStatusOrderViewModel model)
        {
            if (_context.StatusesOrder
                    .Where(f => f.Status == model.Status).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный статус уже существует");
            }

            if (ModelState.IsValid)
            {
                StatusOrder statusOrder = new()
                {
                    Status = model.Status
                };

                _context.Add(statusOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: StatusesOrder/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusOrder = await _context.StatusesOrder.FindAsync(id);
            if (statusOrder == null)
            {
                return NotFound();
            }

            EditStatusOrderViewModel model = new()
            {
                Id = statusOrder.Id,
                Status = statusOrder.Status
            };

            return View(model);
        }

        // POST: StatusesOrder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditStatusOrderViewModel model)
        {
            if (_context.StatusesOrder
                    .Where(f => f.Status == model.Status).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный статус уже существует");
            }

            StatusOrder statusOrder = await _context.StatusesOrder.FindAsync(id);

            if (id != statusOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    statusOrder.Status = model.Status;
                    _context.Update(statusOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusOrderExists(statusOrder.Id))
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

        // GET: StatusesOrder/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusOrder = await _context.StatusesOrder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusOrder == null)
            {
                return NotFound();
            }

            return View(statusOrder);
        }

        // POST: StatusesOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var statusOrder = await _context.StatusesOrder.FindAsync(id);
            _context.StatusesOrder.Remove(statusOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: StatusesOrder/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusOrder = await _context.StatusesOrder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusOrder == null)
            {
                return NotFound();
            }

            return View(statusOrder);
        }

        private bool StatusOrderExists(short id)
        {
            return _context.StatusesOrder.Any(e => e.Id == id);
        }
    }
}
