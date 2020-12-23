using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace HomeShopping.Controllers
{
    public class TinTucController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NewsDetail()
        {
            return View();
        }

    }
}