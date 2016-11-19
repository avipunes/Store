using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModel
{
    public class SupllierModel
    {
    }

    public class SuplliersDataView
    {
        public IEnumerable<SupllierDataView> suplliers { get; set; }
    }

    public class SupllierDataView
    {
        [Key]
        public int SupllierID { get; set; }
        public string SupllierName { get; set; }
        public string SupllierPhone { get; set; }
        public string SupllierAddress { get; set; }
    }
}