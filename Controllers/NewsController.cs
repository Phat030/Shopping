using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeShopping.Datas;
using HomeShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeShopping.Controllers
{
    public class NewsController : Controller
    {

        private readonly DataContext dataContext;
        public NewsController(DataContext context)
        {
            dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            var data = dataContext.News.Include(p => p.NewsType);
            return View(await data.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await dataContext.News
                .FirstOrDefaultAsync(m => m.NewsID == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }
        public IActionResult Create()
        {
            ViewData["NewsTypeID"] = new SelectList(dataContext.NewsTypes, "NewsTypeID", "NewsTypeName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsID,NewsName,QuickView,NewsDate,NewsImage1,NewsImage2,NewsImage3,NewsImage4,Paragraph1,Paragraph2,Paragraph3,Paragraph4,NewsTypeID")] News news)
        {
            if (ModelState.IsValid)
            {
                dataContext.Add(news);
                await dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NewsTypeID"] = new SelectList(dataContext.NewsTypes, "NewsTypeID", "NewsTypeName", news.NewsTypeID);
            return View(news);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await dataContext.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["NewsTypeID"] = new SelectList(dataContext.NewsTypes, "NewsTypeID", "NewsTypeName", news.NewsTypeID);
            return View(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsID,NewsName,QuickView,NewsDate,NewsImage1,NewsImage2,NewsImage3,NewsImage4,Paragraph1,Paragraph2,Paragraph3,Paragraph4,NewsTypeID")] News news)
        {
            if (id != news.NewsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dataContext.Update(news);
                    await dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.NewsID))
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
            ViewData["NewsTypeID"] = new SelectList(dataContext.NewsTypes, "NewsTypeID", "NewsTypeID", news.NewsTypeID);
            return View(news);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await dataContext.News
                .Include(p => p.NewsType)
                .FirstOrDefaultAsync(m => m.NewsID == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await dataContext.News.FindAsync(id);
            dataContext.News.Remove(news);
            await dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return dataContext.News.Any(e => e.NewsTypeID == id);
        }
        public async Task<IActionResult> NewsPage(int? page)
        {
            News news = new News();
            var list = await dataContext.News.ToListAsync();
            int number = 0;
            ViewBag.Dem = list.Count() / 3;
            if (page != null) { number = page.GetValueOrDefault() * 3; }
            List<News> newslist = list.OrderBy(s => s.NewsID).Skip(number).Take(3).ToList();
            return View(newslist);
            //return View(await dataContext.News.ToListAsync());
        }
        public async Task<IActionResult> NewsDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await dataContext.News.FirstOrDefaultAsync(m => m.NewsID == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
            
        }
    }
}

