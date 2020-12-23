using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeShopping.Models;

namespace HomeShopping.Models
{
    public class ProductData
    {
        public List<ProductModel> ProductList { get; set; }
        public static ProductData initData()
        {
            List<ProductModel> products = new List<ProductModel>();
            products.AddRange(new List<ProductModel>
            {
                new ProductModel()
                {
                    ProductName="Huawei Y9",
                    ProductImage="HuaweiY9.jpg",
                    ProductQuantity=100,
                    ProductPrice=1000.00,
                    CreateDate=DateTime.Now,
                    ManHinh="IPS LCD, 6.67, Full HD+",
                    HeDieuHanh="Android",
                    CPU="Snapdragon 720G 8 nhân",
                    RAM="8GB",
                    DungLuongPin="5020mAh,có sạc nhanh",
                    BoNhoTrong="128GB"
                },
               new ProductModel()
                {
                    ProductName="Oppo Find X",
                    ProductImage="OppofindX.jpg",
                    ProductQuantity=100,
                    ProductPrice=1000.00,
                    CreateDate=DateTime.Now,
                    ManHinh="IPS LCD, 6.67, Full HD+",
                    HeDieuHanh="Android",
                    CPU="Snapdragon 720G 8 nhân",
                    RAM="8GB",
                    DungLuongPin="5020mAh,có sạc nhanh",
                    BoNhoTrong="128GB"
                },
               new ProductModel()
                {
                    ProductName="Xiaomi Mi 9",
                     ProductImage="XMMi9.jpg",
                    ProductQuantity=100,
                    ProductPrice=1000.00,
                    CreateDate=DateTime.Now,
                    ManHinh="IPS LCD, 6.67, Full HD+",
                    HeDieuHanh="Android",
                    CPU="Snapdragon 720G 8 nhân",
                    RAM="8GB",
                    DungLuongPin="5020mAh,có sạc nhanh",
                    BoNhoTrong="128GB"
                },
               new ProductModel()
                {
                    ProductName="Sony Xperia Z1",
                     ProductImage="Z1.jpg",
                    ProductQuantity=100,
                    ProductPrice=1000.00,
                    CreateDate=DateTime.Now,
                    ManHinh="IPS LCD, 6.67, Full HD+",
                    HeDieuHanh="Android",
                    CPU="Snapdragon 720G 8 nhân",
                    RAM="8GB",
                    DungLuongPin="5020mAh,có sạc nhanh",
                    BoNhoTrong="128GB"
                },
               
            }); ;
            return new ProductData()
            {
                ProductList = products
            };
        }
    }
}
