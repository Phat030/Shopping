using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using HomeShopping.Datas;
using HomeShopping.Helper;
using HomeShopping.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Microsoft.AspNetCore.Authorization;
using PayPal.Core;
using PayPal.v1.Payments;
using Order = HomeShopping.Models.Order;
using BraintreeHttp;

namespace HomeShopping.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductData productData;
        private readonly DataContext dataContext;
        private readonly string _clientId;
        private readonly string _secretKey;
        public double TyGiaUSD = 23178;

        ///public List<Item> Items { get; private set; }

        public CartController(ProductData productData, DataContext dataContext, IConfiguration config)
        {
            this.productData = productData;
            this.dataContext = dataContext;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<ProductToCart>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.ProductPrice * item.Quantity );

            return View();
        }

        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {

            if (SessionHelper.GetObjectFromJson<List<ProductToCart>>(HttpContext.Session, "cart") == null)
            {

                List<ProductToCart> cart = new List<ProductToCart>();
                cart.Add(new ProductToCart { Product = dataContext.Products.FirstOrDefault(p => p.ProductId == id), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<ProductToCart> cart = SessionHelper.GetObjectFromJson<List<ProductToCart>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new ProductToCart { Product = dataContext.Products.FirstOrDefault(p => p.ProductId == id), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<ProductToCart> cart = SessionHelper.GetObjectFromJson<List<ProductToCart>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<ProductToCart> cart = SessionHelper.GetObjectFromJson<List<ProductToCart>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ProductId == id)
                {
                    return i;
                }
            }
            return -1;
        }

        //[Route("pay/{id}")]
        [HttpPost]
        public IActionResult Pay(int id, IFormCollection form)
        {
            List<ProductToCart> cart = SessionHelper.GetObjectFromJson<List<ProductToCart>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.ProductPrice * item.Quantity);

            return View();
        }

        //[Route("xacnhan/{id}")]
        [HttpPost]
        public IActionResult XacNhan(string fname, string email, string address, string phone, string message, IFormCollection form)
        {
            List<ProductToCart> cart = SessionHelper.GetObjectFromJson<List<ProductToCart>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.ProductPrice * item.Quantity);
            //string[] quantity = form["quantity"];
            //for (int i = 0; i < cart.Count; i++)
            //{
            //    cart[i].Quantity = Convert.ToInt32(quantity[i]);
            //}

            Order order = new Order()
            {
                OrderDate = DateTime.Now
            };
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            dataContext.Orders.Add(order);
            dataContext.SaveChanges();
            //return View("Success");
            foreach (ProductToCart item in cart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderID = order.OrderID,
                    ProductId = item.Product.ProductId,
                    Quantity = item.Quantity,
                    HoTen = fname,
                    Email = email,
                    DiaChi = address,
                    SDT = phone,
                    YeuCau = message,
                    Status = "Chờ xác nhận"
                };
                dataContext.OrderDetails.Add(orderDetail);
                dataContext.SaveChanges();
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> PaypalCheckout()
        {
            List<ProductToCart> cart = SessionHelper.GetObjectFromJson<List<ProductToCart>>(HttpContext.Session, "cart");
            //ViewBag.total = cart.Sum(item => item.Product.ProductPrice * item.Quantity);
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };
            var total = Math.Round(cart.Sum(p => p.Product.ProductPrice * p.Quantity) / TyGiaUSD, 2);
            foreach (var item in cart)
            {
                itemList.Items.Add(new Item()
                {
                    Name = item.Product.ProductName,
                    Currency = "USD",
                    Price = item.Product.ProductPrice.ToString(),
                    Quantity = item.Quantity.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
            }
            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}:{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = total.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = total.ToString()
                            }
                        },
                        ItemList = itemList,
                        Description = $"Invoice #001 {paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString(),
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}/Cart/CheckoutFail",
                    ReturnUrl = $"{hostname}/Cart/CheckoutSuccess"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };
            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);
            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect("/Cart/CheckoutFail");
            }
        }
        public IActionResult CheckoutFail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CheckoutSuccess(string fname, string email, string address, string phone, string message, IFormCollection form)
        {
            List<ProductToCart> cart = SessionHelper.GetObjectFromJson<List<ProductToCart>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.ProductPrice * item.Quantity);
            //string[] quantity = form["quantity"];
            //for (int i = 0; i < cart.Count; i++)
            //{
            //    cart[i].Quantity = Convert.ToInt32(quantity[i]);
            //}

            Order order = new Order()
            {
                OrderDate = DateTime.Now
            };
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            dataContext.Orders.Add(order);
            dataContext.SaveChanges();
            //return View("Success");
            foreach (ProductToCart item in cart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderID = order.OrderID,
                    ProductId = item.Product.ProductId,
                    Quantity = item.Quantity,
                    HoTen = fname,
                    Email = email,
                    DiaChi = address,
                    SDT = phone,
                    YeuCau = message,
                    Status = "Chờ xác nhận"
                };
                dataContext.OrderDetails.Add(orderDetail);
                dataContext.SaveChanges();
            }
            return View();
        }
    }
}


