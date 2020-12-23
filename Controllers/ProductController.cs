using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeShopping.Datas;
using HomeShopping.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeShopping.Controllers
{


    public class ProductController : Controller
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        // GET: Product
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(int? page)
        {
            Product item = new Product();
            var list = await _context.Products.ToListAsync();
            int number = 0;
            ViewBag.Dem = list.Count() / 8;
            if (page != null) { number = page.GetValueOrDefault() * 8; }
            List<Product> productlist = list.OrderBy(s => s.ProductId).Skip(number).Take(8).ToList();
            return View(productlist);
            //var dataContext = _context.Products.Include(p => p.Category);
            //return View(await dataContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductQuantity,ProductImage,ProductPrice,CreateDate,Descriptions,CategoryID,ManHinh,BoNhoTrong,CPU,RAM,DungLuongPin,HeDieuHanh")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductQuantity,ProductImage,ProductPrice,CreateDate,Descriptions,,ManHinh,BoNhoTrong,CPU,RAM,DungLuongPin,HeDieuHanh,CategoryID")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        public IActionResult Samsung()
        {
            //var dataContext = _context.Products.Include(p => p.Category)
            //                                    .Where(p=>p.CategoryID==2);
            //return View(dataContext.ToList());
            return View(_context.Products.Where(p => p.Category.CategoryName.Equals("Samsung")).ToList());
        }
        public IActionResult Sony()
        {

            return View(_context.Products.Where(p => p.Category.CategoryName.Equals("Sony")).ToList());
        }
        public IActionResult Iphone()
        {

            return View(_context.Products.Where(p => p.Category.CategoryName.Equals("Iphone")).ToList());
        }
        public IActionResult Oppo()
        {

            return View(_context.Products.Where(p => p.Category.CategoryName.Equals("Oppo")).ToList());
        }
        public IActionResult Huawei()
        {

            return View(_context.Products.Where(p => p.Category.CategoryName.Equals("Huawei")).ToList());
        }
        public IActionResult TimTheoGia1()
        {
            var item = _context.Products.Where(p => p.ProductPrice <= 200).ToList();
            return View(item);
        }
        public IActionResult TimTheoGia2()
        {
            var item = _context.Products.Where(p => p.ProductPrice >= 200 && p.ProductPrice <= 500).ToList();
            return View(item);
        }
        public IActionResult TimTheoGia3()
        {
            var item = _context.Products.Where(p => p.ProductPrice >= 500 && p.ProductPrice <= 1000).ToList();
            return View(item);
        }
        public IActionResult Search(string search)
        {
            var item = _context.Products.Where(p => p.ProductName.Contains(search));
            return View(item.ToList());
        }

       
    }
}
