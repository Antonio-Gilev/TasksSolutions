using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperORM_ConsoleApp.Models
{
    public class OrderDetail
    {
        public Order Order { get; set; }

        public Product Product { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }
    }
}
