using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilShop.Models;
using OilShop.Models.Data;
using OilShop.Models.Enums;
using OilShop.ViewModels.OilsInStock;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class OilsInStockController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public OilsInStockController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET:OilsInStock
        public async Task<IActionResult> Index(decimal purchasePrice, DateTime purchaseDate,
            int margin, int page = 1, 
            OilInStockSortState sortOrder = OilInStockSortState.MarginAsc)
        {
            int pageSize = 10;

            //фильтрация
            IQueryable<OilInStock> oilsInStock = _context.OilsInStock
                .Include(f => f.Oil);

            //if (margin != null && margin != 0)
            //{
            //    oilsInStock = oilsInStock.Where(p => p.Margin == margin);
            //}

            // сортировка
            switch (sortOrder)
            {
                case OilInStockSortState.MarginDesc:
                    oilsInStock = oilsInStock.OrderByDescending(s => s.Margin);
                    break;
                case OilInStockSortState.PurchaseDateAsc:
                    oilsInStock = oilsInStock.OrderByDescending(s => s.PurchaseDate);
                    break;
                case OilInStockSortState.PurchaseDateDesc:
                    oilsInStock = oilsInStock.OrderByDescending(s => s.PurchaseDate);
                    break;
                case OilInStockSortState.PurchasePriceAsc:
                    oilsInStock = oilsInStock.OrderByDescending(s => s.PurchasePrice);
                    break;
                case OilInStockSortState.PurchasePriceDesc:
                    oilsInStock = oilsInStock.OrderByDescending(s => s.PurchasePrice);
                    break;
                default:
                    oilsInStock = oilsInStock.OrderBy(s => s.Margin);
                    break;
            }

            // пагинация
            var count = await oilsInStock.CountAsync();
            var items = await oilsInStock.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexOilInStockViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                SortOilInStockViewModel = new(sortOrder),
                FilterOilInStockViewModel = new(margin),
                OilsInStock = items
            };

            return View(viewModel);
        }

        // GET:
        public async Task<IActionResult> ViewPurchases(int? idOil,
            decimal purchasePrice, DateTime purchaseDate,
            int margin, int page = 1,
            OilInStockSortState sortOrder = OilInStockSortState.MarginAsc)
        {
            if (idOil == null)
            {
                return NotFound();
            }

            var oil = await _context.Oils.FindAsync(idOil);
            if (oil == null)
            {
                return NotFound();
            }

            int pageSize = 10;

            //фильтрация
            IQueryable<OilInStock> oilsInStock = _context.OilsInStock
                .Include(f => f.Oil)
                .Where(f => f.IdOil == idOil);

            //if (margin != null && margin != 0)
            //{
            //    oilsInStock = oilsInStock.Where(p => p.Margin == margin);
            //}

            // сортировка
            switch (sortOrder)
            {
                case OilInStockSortState.MarginDesc:
                    oilsInStock = oilsInStock.OrderByDescending(s => s.Margin);
                    break;
                case OilInStockSortState.PurchaseDateAsc:
                    oilsInStock = oilsInStock.OrderBy(s => s.PurchaseDate.Date);
                    break;
                case OilInStockSortState.PurchaseDateDesc:
                    oilsInStock = oilsInStock.OrderByDescending(s => s.PurchaseDate.Date);
                    break;
                case OilInStockSortState.PurchasePriceAsc:
                    oilsInStock = oilsInStock.OrderBy(s => s.PurchasePrice);
                    break;
                case OilInStockSortState.PurchasePriceDesc:
                    oilsInStock = oilsInStock.OrderByDescending(s => s.PurchasePrice);
                    break;
                default:
                    oilsInStock = oilsInStock.OrderBy(s => s.Margin);
                    break;
            }

            // пагинация
            var count = await oilsInStock.CountAsync();
            var items = await oilsInStock.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexOilInStockViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                SortOilInStockViewModel = new(sortOrder),
                FilterOilInStockViewModel = new(margin),
                OilsInStock = items
            };

            return View(viewModel);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOilInStockViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (ModelState.IsValid)
            {
                OilInStock oilInStock = new()
                {
                    Margin = model.Margin + model.PurchasePrice,
                    PurchaseDate = model.PurchaseDate,
                    PurchasePrice = Convert.ToDecimal(model.PurchasePriceRub) + Convert.ToDecimal(model.PurchasePrice) / 100,
                };

                _context.Add(oilInStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oilInStock = await _context.OilsInStock.FindAsync(id);
            if (oilInStock == null)
            {
                return NotFound();
            }

            EditOilInStockViewModel model = new()
            {
                Id = oilInStock.Id,
                Margin = oilInStock.Margin + Convert.ToInt32(Math.Truncate(oilInStock.PurchasePrice)),
                PurchaseDate = oilInStock.PurchaseDate,
                PurchasePriceRub = Convert.ToInt32(Math.Truncate(oilInStock.PurchasePrice)),
                PurchasePrice = Convert.ToInt32(oilInStock.PurchasePrice - Math.Truncate(oilInStock.PurchasePrice)),
            };

            return View(model);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditOilInStockViewModel model)
        {

            OilInStock oilInStock = await _context.OilsInStock.FindAsync(id);

            if (id != oilInStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oilInStock.Margin = model.Margin + Convert.ToInt32(Math.Truncate(oilInStock.PurchasePrice));
                    oilInStock.PurchaseDate = model.PurchaseDate;
                    oilInStock.PurchasePrice = Convert.ToDecimal(model.PurchasePriceRub) + Convert.ToDecimal(model.PurchasePrice) / 100;
                    _context.Update(oilInStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OilInStockExists(oilInStock.Id))
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

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oilInStock = await _context.OilsInStock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oilInStock == null)
            {
                return NotFound();
            }

            return View(oilInStock);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oilInStock = await _context.OilsInStock.FindAsync(id);
            _context.OilsInStock.Remove(oilInStock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oilInStock = await _context.OilsInStock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oilInStock == null)
            {
                return NotFound();
            }

            return View(oilInStock);
        }

        private bool OilInStockExists(int id)
        {
            return _context.OilsInStock.Any(e => e.Id == id);
        }
    }
}
