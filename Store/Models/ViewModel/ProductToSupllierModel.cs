using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModel
{
    public class ProductToSupllierModel
    {
    }

    public class ProductsToSupllier
    {
        public IEnumerable<ProductToSupllier> PTSList { get; set; }
    }

    public class ProductToSupllier
    {
        public int PID { get; set; }
        public string PNAME { get; set; }
        public int PPRICE { get; set; }
        public int SID { get; set; }
        public string SNAME { get; set; }
        public string SAD { get; set; }
    }

}