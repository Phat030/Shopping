using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeShopping.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage="Not null")]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Price")]
        [Range(1,999)]
        public double ProductPrice { get; set; }
        [DisplayName("Quantity")]
        [Range(1,200)]
        public int ProductQuantity { get; set; }
        [DisplayName("Description")]
        public string Descriptions { get; set; }
        [DisplayName("Image")]
        public string ProductImage { get; set; }
        [DisplayName("Category")]
        public int ProductCategoryID { get; set; }
        [DisplayName("Create on")]
        public DateTime CreateDate { get; set;   }
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
        private static int nextId = 1;
        public ProductModel()
        {
            ProductID = nextId;
            nextId++;
        }
        public override int GetHashCode()
        {
            return ProductID;
        }
    }
}
