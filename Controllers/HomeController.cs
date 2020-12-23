using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeShopping.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using HomeShopping.Datas;
using Microsoft.EntityFrameworkCore;
using HomeShopping.Datas;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace HomeShopping.Controllers
{

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        DataContext dataContext;
        //IMapper mapper;

        ProductData productData;
        public HomeController(ProductData productData,DataContext dataContext )
        {
            this.productData = productData;
            this.dataContext = dataContext;
            //this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            Product item = new Product();
            var list = await dataContext.Products.ToListAsync();
            int number = 0;
            ViewBag.Dem = list.Count() / 8;
            if (page != null)
            { 
                number = page.GetValueOrDefault() * 8; 
            }
            List<Product> productlist = list.OrderBy(s => s.ProductId).Skip(number).Take(8).ToList();
            return View(productlist);
            //List<Product> productss = dataContext.Products.Include(p => p.Category).ToList();
            ////List<Product> productss = dataContext.Products.Where(p => p.Category.CategoryName.Equals("NewProduct")).ToList();
            //List<ProductModel> products = new List<ProductModel>();
            //foreach (Product p in productss)
            //{
            //    products.Add(new ProductModel()
            //    { 
            //        ProductID = p.ProductId,
            //        ProductName = p.ProductName,
            //        ProductImage = p.ProductImage,
            //        ProductPrice = p.ProductPrice,
            //        ProductQuantity = p.ProductQuantity,
            //    });
            //}
            //return View(productss);

        }

        [HttpGet("Detail/{id}/{name}")]
        public IActionResult Detail(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            Product product = new Product();
            ProductModel productModel = new ProductModel();
            productModel = productData.ProductList.FirstOrDefault(p => p.ProductID == id);
            productModel.ProductID = product.ProductId;
            productModel.ProductName = product.ProductName;
            productModel.ProductImage = product.ProductImage;
            productModel.ProductPrice = product.ProductPrice;
            productModel.ProductQuantity = product.ProductQuantity;
            productModel.RAM = product.RAM;
            productModel.ManHinh = product.ManHinh;
            productModel.HeDieuHanh = product.HeDieuHanh;
            productModel.CPU = product.CPU;
            productModel.BoNhoTrong = product.BoNhoTrong;
            productModel.DungLuongPin = product.DungLuongPin;
            if (productModel == null)
            {
                return NotFound();
            }
            return View(productModel);
        }



        [HttpGet]
        public IActionResult Add()
        {
            ProductModel productModel = new ProductModel();
            return View(productModel);
        }
        [HttpPost]
        public IActionResult Add(ProductModel productModel, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                ProductModel newProduct = new ProductModel();
                if (photo == null || photo.Length == 0)
                {
                    newProduct.ProductImage = "abc.png";
                }
                else
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"
                        , photo.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    photo.CopyToAsync(stream);
                    newProduct.ProductImage = photo.FileName;
                }
                newProduct.ProductName = productModel.ProductName;
                newProduct.ProductQuantity = productModel.ProductQuantity;
                newProduct.ProductPrice = productModel.ProductPrice;
                newProduct.CreateDate = DateTime.Now;
                productData.ProductList.Add(newProduct);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(productModel);
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ProductModel oldProduct = productData.ProductList.FirstOrDefault(p => p.ProductID == id);
            return View(oldProduct);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                ProductModel oldProduct = productData.ProductList.FirstOrDefault(p => p.ProductID == id);
                oldProduct.ProductName = productModel.ProductName;
                oldProduct.ProductQuantity = productModel.ProductQuantity;
                oldProduct.ProductPrice = productModel.ProductPrice;
                oldProduct.CreateDate = DateTime.Now;
                ViewBag.Status = 1;
            }
            return View(productModel);
        }
        public IActionResult Delete(int id)
        {
            ProductModel oldProduct = productData.ProductList.FirstOrDefault(p => p.ProductID == id);
            return View(oldProduct);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            productData.ProductList.RemoveAll(p => p.ProductID == id);
            return RedirectToAction("Index", "Home");
        }
    }
}
