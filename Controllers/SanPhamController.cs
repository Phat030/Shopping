using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using HomeShopping.Datas;
using HomeShopping.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeShopping.Controllers
{

    public class SanPhamController : Controller
    {
        ProductData productData;
        DataContext dataContext;
       

      
        public SanPhamController(ProductData productData,DataContext dataContext)
        {
            this.productData = productData;
            this.dataContext = dataContext;
            
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<ProductModel> products = new List<ProductModel>();
            List<Product> productss =dataContext.Products.ToList();

            foreach (Product p in productss)
            {
                products.Add(new ProductModel()
                {
                    ProductID=p.ProductId,
                    ProductName=p.ProductName,
                    ProductImage=p.ProductImage,
                    ProductPrice=p.ProductPrice,
                    ProductQuantity=p.ProductQuantity,
                    Descriptions=p.Descriptions,
                    HeDieuHanh=p.HeDieuHanh,
                    DungLuongPin=p.DungLuongPin,
                    ManHinh=p.ManHinh,
                    CPU=p.CPU,
                    RAM=p.RAM,
                    ProductCategoryID=p.CategoryID
                });
            }
            return View(products);
            
        }
        [HttpGet]
        public IActionResult Add()
        {
            ProductModel productModel = new ProductModel();
           
            ViewData["Category"] = new SelectList(dataContext.Categories, "CategoryID", "CategoryName");
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
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", photo.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    photo.CopyToAsync(stream);
                    newProduct.ProductImage = photo.FileName;
                }
                newProduct.ProductName = productModel.ProductName;
                newProduct.ProductQuantity = productModel.ProductQuantity;
                newProduct.ProductPrice = productModel.ProductPrice;
                newProduct.CreateDate = DateTime.Now;
                newProduct.ProductImage = productModel.ProductImage;
                newProduct.ManHinh = productModel.ManHinh;
                newProduct.HeDieuHanh = productModel.HeDieuHanh;
                newProduct.RAM = productModel.RAM;
                newProduct.CPU = productModel.CPU;
                newProduct.DungLuongPin = productModel.DungLuongPin;
                newProduct.BoNhoTrong = productModel.BoNhoTrong;
                newProduct.Descriptions = productModel.Descriptions;
                // productData.ProductList.Add(newProduct);
                //ViewData["Category"] = new SelectList(dataContext.Categories, "CategoryID", "CategoryID",productModel.ProductCategoryID);
                //ViewData["Category"] = productModel.ProductCategoryID;
                newProduct.ProductCategoryID = productModel.ProductCategoryID;

                Product p = new Product()
                {
                    ProductName = newProduct.ProductName,
                    ProductImage = newProduct.ProductImage,
                    ProductQuantity = newProduct.ProductQuantity,
                    ProductPrice = newProduct.ProductPrice,
                    CreateDate = newProduct.CreateDate,
                    //var category= dataContext.Products.Where(t=>t.Category)
                    //CategoryID= new SelectList(_tagService.GetTags(), nameof(Tag.TagId), nameof(Tag.TagName)),
                    CategoryID=newProduct.ProductCategoryID,
                    CPU =newProduct.CPU,
                    BoNhoTrong=newProduct.BoNhoTrong,
                    ManHinh=newProduct.ManHinh,
                    DungLuongPin=newProduct.DungLuongPin,
                    HeDieuHanh=newProduct.HeDieuHanh,
                    Descriptions=newProduct.Descriptions
                };
                dataContext.Products.Add(p);
                dataContext.SaveChanges();

                return RedirectToAction("Index", "SanPham");
            }
            else
            {
                return View(productModel);
            }


        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // ProductModel oldProduct = productData.ProductList.FirstOrDefault(p => p.ProductId == id);

            Product product = dataContext.Products.FirstOrDefault(p => p.ProductId == id);  
            ProductModel oldProduct = new ProductModel()
            {
                ProductID = product.ProductId,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
                ProductPrice = product.ProductPrice,
                ProductQuantity = product.ProductQuantity,
                ManHinh = product.ManHinh,
                CPU = product.CPU,
                RAM=product.RAM,
                DungLuongPin=product.DungLuongPin,
                BoNhoTrong=product.BoNhoTrong,
                HeDieuHanh=product.HeDieuHanh,
                Descriptions=product.Descriptions
            };
            ViewData["Category"] = new SelectList(dataContext.Categories, "CategoryID", "CategoryName");
            return View(oldProduct);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                Product p = dataContext.Products.FirstOrDefault(p => p.ProductId == id);
                p.ProductName = productModel.ProductName;
                p.ProductQuantity = productModel.ProductQuantity;
                p.ProductImage = productModel.ProductImage;
                p.ProductId = productModel.ProductID;
                p.ProductPrice = productModel.ProductPrice;
                p.ManHinh = productModel.ManHinh;
                p.HeDieuHanh = productModel.HeDieuHanh;
                p.CPU = productModel.CPU;
                p.BoNhoTrong = productModel.BoNhoTrong;
                p.RAM = productModel.RAM;
                p.DungLuongPin = productModel.DungLuongPin;
                p.Descriptions = productModel.Descriptions;
                p.CategoryID = productModel.ProductCategoryID;
                dataContext.SaveChanges();
                ViewBag.Status = 1;
            }
            return View(productModel);
        }
        public IActionResult Delete(int id)
        {
            //ProductModel oldProduct = productData.ProductList.FirstOrDefault(p => p.ProductId == id);
            Product product = dataContext.Products.FirstOrDefault(p => p.ProductId == id);
            ProductModel oldProduct = new ProductModel()
            {
                ProductID = product.ProductId,
                ProductName = product.ProductName
            };
            return View(oldProduct);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // productData.ProductList.RemoveAll(p => p.ProductId == id);
            Product product = dataContext.Products.FirstOrDefault(p => p.ProductId == id);
            dataContext.Products.Remove(product);
            dataContext.SaveChanges();
            return RedirectToAction("Index", "SanPham");
        }
    }
}