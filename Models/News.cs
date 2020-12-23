using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeShopping.Models
{
    public class News
    {
        public int NewsID {get;set;}
        public string NewsName { get; set; }
        public string QuickView { get; set; }
        public string NewsDate { get; set; }
        public string NewsTitle1 { get; set; }
        public string NewsTitle2 { get; set; }
        public string NewsTitle3 { get; set; }
        public string NewsTitle4 { get; set; }
        public string NewsImage1 { get; set; }
        public string NewsImage2 { get; set; }
        public string NewsImage3 { get; set; }
        public string NewsImage4 { get; set; }
        public string Paragraph1  { get; set; }
        public string Paragraph2 { get; set; }
        public string Paragraph3 { get; set; }
        public string Paragraph4 { get; set; }
        public NewsType NewsType { get; set; }
        public int NewsTypeID { get; set; }
    }
}
