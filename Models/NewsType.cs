using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeShopping.Models
{
    public class NewsType
    {
        public int NewsTypeID { get; set; }
        public string NewsTypeName { get; set; }
        public List<News> News { get; set; }
    }
}
