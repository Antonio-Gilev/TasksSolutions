using DapperORM_ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperORM_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var order = new Order
            {
                CustomerID = 1,
                EmployeeID = 1,
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now,
                ShipAddress = "Address",
                ShipCity = "City",
                ShipRegion = "Region",
                ShipPostalCode = 1000,
                ShipCountry = "Country"

            };


            int orderId = DataAccess.CreateOrder(order);
            Console.WriteLine($"Order created with ID: {orderId}");


            var product = new Product
            {
                ProductName = "Mleko",
                SupplierID = 1,
                Category = new Category { CategoryID = 1 },
                QuantityPerUnit = 10,
                UnitPrice = 99,
                UnitsInStock = 100,
                ReorderLevel = 1,
                LastDateUpdated = DateTime.Now
            };


            int productId = DataAccess.CreateProduct(product);
            Console.WriteLine($"Product created with ID: {productId}");


            /*var orderDetail = new OrderDetail
            {
                Order = new Order { OrderID = 1007 },
                Product = new Product { ProductID = 1002},
                UnitPrice = product.UnitPrice,
                Quantity = 5,
            };

            DataAccess.CreateOrderDetail(orderDetail);*/


            List<Order> orders = DataAccess.GetOrdersSortedByDate();
            Console.WriteLine("Orders sorted by date:");
            foreach (var o in orders)
            {
                Console.WriteLine($"Order ID: {o.OrderID}, Date: {o.OrderDate}");
            }

            List<Product> products = DataAccess.GetProductsSortedByMostSold();
            Console.WriteLine("Products sorted by most sold:");
            foreach (var p in products)
            {
                Console.WriteLine($"Product ID: {p.ProductID}, Name: {p.ProductName}");
            }


            List<Category> categories = DataAccess.GetCategoriesSortedByMostSold();
            Console.WriteLine("Categories sorted by most sold:");
            foreach (var c in categories)
            {
                Console.WriteLine($"Category ID: {c.CategoryID}, Name: {c.CategoryName}");
            }

            bool shopping = true;
            while (true)
            {
                Console.WriteLine("Enter the ID of the product you want to buy (or type 'exit' to quit):");
                var line = Console.ReadLine();

                if (line == "exit")
                {
                    shopping = false;
                    break;
                }

                int productID;
                if (!int.TryParse(line, out productID))
                {
                    Console.WriteLine("Invalid product ID. Please try again.");
                    continue;
                }

                Console.WriteLine("Enter the quantity:");
                int quantity;
                if (!int.TryParse(Console.ReadLine(), out quantity))
                {
                    Console.WriteLine("Invalid quantity. Please try again.");
                    continue;
                }

                var orderDetail = new OrderDetail
                {
                    Order = new Order { OrderID = orderId },
                    Product = new Product { ProductID = productID },
                    UnitPrice = product.UnitPrice,
                    Quantity = quantity
                };

                DataAccess.CreateOrderDetail(orderDetail);
                Console.WriteLine("Product placed in Order");
            }

            Console.WriteLine("Thank you for shopping with us.");
            Console.ReadLine();
        }
    }

}
