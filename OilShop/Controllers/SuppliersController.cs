using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.Models.Enums;
using OilShop.ViewModels.Suppliers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class SuppliersController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public SuppliersController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index(string supplier, int page = 1,
            SupplierSortState sortOrder = SupplierSortState.SupplierAsc)
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            int pageSize = 15;

            //фильтрация
            IQueryable<Supplier> suppliers = _context.Suppliers;

            if (!String.IsNullOrEmpty(supplier))
            {
                suppliers = suppliers.Where(p => p.SupplierOil.Contains(supplier));
            }

            // сортировка
            switch (sortOrder)
            {
                case SupplierSortState.SupplierDesc:
                    suppliers = suppliers.OrderByDescending(s => s.SupplierOil);
                    break;
                default:
                    suppliers = suppliers.OrderBy(s => s.SupplierOil);
                    break;
            }

            // пагинация
            var count = await suppliers.CountAsync();
            var items = await suppliers.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexSupplierViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                SortSupplierViewModel = new(sortOrder),
                FilterSupplierViewModel = new(supplier),
                Suppliers = items
            };
            return View(viewModel);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSupplierViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Suppliers
                    .Where(f => f.SupplierOil == model.SupplierOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный поставщик масла уже существует");
            }

            if (ModelState.IsValid)
            {
                Supplier supplier = new()
                {
                    SupplierOil = model.SupplierOil
                };
                
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            EditSupplierViewModel model = new()
            {
                Id = supplier.Id,
                SupplierOil = supplier.SupplierOil
            };

            return View(model);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditSupplierViewModel model)
        {
            if (_context.Suppliers
                    .Where(f => f.SupplierOil == model.SupplierOil).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный поставщик масла уже существует");
            }

            Supplier supplier = await _context.Suppliers.FindAsync(id);

            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    supplier.SupplierOil = model.SupplierOil;
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Id))
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

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
