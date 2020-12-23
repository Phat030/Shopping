using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeShopping.Datas;
using HomeShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeShopping.Controllers
{
    public class NewsTypeController : Controller
    {
        public readonly DataContext dataContext;
        public NewsTypeController(DataContext context)
        {
            dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await dataContext.NewsTypes.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var type = await dataContext.NewsTypes
                .FirstOrDefaultAsync(m => m.NewsTypeID == id);
            if (type == null)
            {
                return NotFound();
            }

            return View(type);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsTypeID,NewsTypeName")] NewsType newsType)
        {
            if (ModelState.IsValid)
            {
                dataContext.Add(newsType);
                await dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsType);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await dataContext.NewsTypes.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsTypeID,NewsTypeName")] NewsType type)
        {
            if (id != type.NewsTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dataContext.Update(type);
                    await dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsTypeExists(type.NewsTypeID))
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
            return View(type);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await dataContext.NewsTypes
                .FirstOrDefaultAsync(m => m.NewsTypeID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await dataContext.NewsTypes.FindAsync(id);
            dataContext.NewsTypes.Remove(category);
            await dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsTypeExists(int id)
        {
            return dataContext.NewsTypes.Any(e => e.NewsTypeID == id);
        }
    }
}
