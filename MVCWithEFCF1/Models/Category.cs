using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWithEFCF1.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        //public bool Status { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}