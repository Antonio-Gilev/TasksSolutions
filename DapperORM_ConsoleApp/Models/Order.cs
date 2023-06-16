using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperORM_ConsoleApp.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public int CustomerID { get; set; }

        public int EmployeeID { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public DateTime ShippedDate { get; set; }

        public String ShipVia { get; set; }

        public decimal Freight { get; set; }

        public String ShipName { get; set; }

        public String ShipAddress { get; set; }

        public String ShipCity { get; set; }

        public String ShipRegion { get; set; }

        public int ShipPostalCode { get; set; }

        public String ShipCountry { get; set; }

    }
}
