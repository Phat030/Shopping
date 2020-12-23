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
using HomeShopping.Models.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace HomeShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles ="Administrator")]
    [Authorize]
    public class ManageController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ManageController(DataContext context, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //Admin/UserManage
        public IActionResult ManageUser()
        {
            IEnumerable<User> users = _userManager.Users.AsEnumerable();
            return View(users);
        }
        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Products.Include(p => p.Category);
            return View(await dataContext.ToListAsync());
        }

        // GET: Admin/Product/Details/5
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

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductQuantity,ProductImage,ProductPrice,CreateDate,Descriptions,ManHinh,HeDieuHanh,CPU,RAM,DungLuongPin,BoNhoTrong,CategoryID")] Product product)
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

        // GET: Admin/Product/Edit/5
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductQuantity,ProductImage,ProductPrice,CreateDate,Descriptions,ManHinh,HeDieuHanh,CPU,RAM,DungLuongPin,BoNhoTrong,CategoryID")] Product product)
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
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

        // POST: Admin/Product/Delete/5
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

        //Quản lí đơn hàng
        public async Task<IActionResult> ManageOrder()
        {
            var data = _context.OrderDetails.Include(p => p.Order);
            return View(await data.ToListAsync());
        }
        public async Task<IActionResult> EditOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrderDetails.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["OrderID"] = new SelectList(_context.NewsTypes, "OrderID", "OrderID", order.OrderID);
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(int id, [Bind("OrderDetailID,OrderID,ProductId,Quantity,DiaChi,Email,HoTen,SDT,YeuCau,Status")] OrderDetail order)
        {
            if (id != order.OrderDetailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(order.OrderDetailID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ManageOrder");
            }
            //ViewData["Status"] = new SelectList(_context.OrderDetails, "Status", "Status", order.Status);
            ViewData["OrderID"] = new SelectList(_context.NewsTypes, "OrderID", "OrderID", order.OrderID);
            return View(order);
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
            return RedirectToAction("ManageOrder");
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderID == id);
        }


        //Quản lí tin tức:
        public async Task<IActionResult> ManageNews()
        {
            var data = _context.News.Include(p => p.NewsType);
            return View(await data.ToListAsync());
        }
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var news = await dataContext.News
        //        .FirstOrDefaultAsync(m => m.NewsID == id);
        //    if (news == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(news);
        //}
        public IActionResult CreateNews()
        {
            ViewData["NewsTypeID"] = new SelectList(_context.NewsTypes, "NewsTypeID", "NewsTypeName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNews([Bind("NewsID,NewsName,QuickView,NewsDate,NewsTitle1,NewsTitle2,NewsTitle3,NewsTitle4,NewsImage1,NewsImage2,NewsImage3,NewsImage4,Paragraph1,Paragraph2,Paragraph3,Paragraph4,NewsTypeID")] News news)
        {
            if (ModelState.IsValid)
            {
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageNews");
            }
            ViewData["NewsTypeID"] = new SelectList(_context.NewsTypes, "NewsTypeID", "NewsTypeName", news.NewsTypeID);
            return View(news);
        }
        public async Task<IActionResult> EditNews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["NewsTypeID"] = new SelectList(_context.NewsTypes, "NewsTypeID", "NewsTypeName", news.NewsTypeID);
            return View(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNews(int id, [Bind("NewsID,NewsName,QuickView,NewsDate,NewsTitle1,NewsTitle2,NewsTitle3,NewsTitle4,NewsImage1,NewsImage2,NewsImage3,NewsImage4,Paragraph1,Paragraph2,Paragraph3,Paragraph4,NewsTypeID")] News news)
        {
            if (id != news.NewsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction("ManageNews");
            }
            ViewData["NewsTypeID"] = new SelectList(_context.NewsTypes, "NewsTypeID", "NewsTypeID", news.NewsTypeID);
            return View(news);
        }
        public async Task<IActionResult> DeleteNews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(p => p.NewsType)
                .FirstOrDefaultAsync(m => m.NewsID == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("DeleteNews")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNewsConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageNews");
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsTypeID == id);
        }
        public IActionResult Demo()
        {
            return View();
        }

        //public async Task<IActionResult> EditUser(int id)
        //{
        //    var user = await _userManager.Users.FirstOrDefaultAsync(id);

        //}
        public async Task<IActionResult> ManageContact()
        {
            var item = await _context.Contacts.ToListAsync();
            return View(item);
        }
        public async Task<IActionResult> EditContact(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = await _context.Contacts.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContact(int id, [Bind("ContactID,Date,HoTen,Email,Phone,Message")] Contact contact)
        {
            if (id != contact.ContactID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.ContactID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ManageContact");
            }
          
            return View(contact);
        }
        public async Task<IActionResult> DeleteContact(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
                //.Include(p => p.NewsType)
                //.FirstOrDefaultAsync(m => m.NewsID == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("DeleteContact")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContactConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageContact");
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactID == id);
        }


    }
}
