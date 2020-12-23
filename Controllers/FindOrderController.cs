using HomeShopping.Datas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeShopping.Datas;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeShopping.Models;

namespace HomeShopping.Controllers
{
    public class FindOrderController:Controller
    {
        private readonly DataContext _context;
        public FindOrderController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();        
        }
        [HttpPost]
        public IActionResult FindOrder(string sdt,OrderDetail orderDetail)
        {
            
            //var name = _context.OrderDetails.Where(x => x.Product.ProductName.Equals("SAMSUNG S10+"));
           
            var item = _context.OrderDetails.Where(x=>x.SDT.Equals(sdt)).ToList();
            return View(item);
        }
        public async Task<IActionResult> DeleteOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrderDetails
                .Include(p => p.Order)
                .FirstOrDefaultAsync(m => m.OrderDetailID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrderConfirmed(int id)
        {
            var order = await _context.OrderDetails.FindAsync(id);
            _context.OrderDetails.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
