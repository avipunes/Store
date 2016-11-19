using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModel
{
    public class ProductModel
    {
    }

    public class ProductDataView
    {
        [Key]
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int product_price { get; set; }
        public string product_address { get; set; }
        public int SupllierID { get; set; }
    }

    public class ProductsDataView
    {
        public IEnumerable<ProductDataView> Products { get; set; }
    }
}