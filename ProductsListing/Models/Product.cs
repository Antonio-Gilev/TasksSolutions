using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsListing.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        public String ProductName { get; set; }

        public int SupplierID { get; set; }

        public Category Category { get; set; }

        public int QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        public int ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public int LastUserID { get; set; }

        public DateTime LastDateUpdated { get; set; }
    }
}
