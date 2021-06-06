using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.ProductItems
{
    public class ProductItemModel
    {
        public int VendorCode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public string Measure { get; set; }
        public string Weight { get; set; }
        public int Discount { get; set; }
        
        public IFormFile[] Images { get; set; }
    }
}
