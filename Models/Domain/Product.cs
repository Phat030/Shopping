using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HomeShopping.Models
{
    public class Product
    {
        public int ProductId { get; set;}
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public DateTime CreateDate { get; set; }
        public string Descriptions { get; set; }
        [DisplayName("Screen")]
        public string ManHinh { get; set; }

        [DisplayName("HeDieuHanh")]
        public string HeDieuHanh { get; set; }
        [DisplayName("CPU")]
        public string CPU { get; set; }
        [DisplayName("RAM")]
        public string RAM { get; set; }
        [DisplayName("DungLuongPin")]
        public string DungLuongPin { get; set; }
        [DisplayName("BoNhoTrong")]
        public string BoNhoTrong { get; set; }
        public Category Category { get; set; }
        public int CategoryID { get; set; }
    }
}
