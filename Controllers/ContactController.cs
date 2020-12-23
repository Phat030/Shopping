using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeShopping.Datas;
using HomeShopping.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HomeShopping.Controllers
{
    public class ContactController : Controller
    {
        private readonly DataContext dataContext;
        public ContactController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Send(string hoten, string email, string phone, string message)
        {
           
                Contact contact = new Contact()
                {
                    Date=DateTime.Now,
                    HoTen = hoten,
                    Email = email,
                    Phone = phone,
                    Message = message
                };
            try
            {
                dataContext.Contacts.Add(contact);
                dataContext.SaveChanges();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View("Index");
        }
    }
}