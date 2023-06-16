using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DapperORM_ConsoleApp.Models;

namespace DapperORM_ConsoleApp
{
    public static class DataAccess
    {
        private static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DapperApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static int CreateOrder(Order order)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var orderId = connection.ExecuteScalar<int>(
                    "INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, RequiredDate, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)" +
                    " VALUES (@CustomerID, @EmployeeID, @OrderDate, @RequiredDate, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry); SELECT CAST(SCOPE_IDENTITY() as int)", 
                    order
                    );
                return orderId;
            }
        }

        public static int CreateProduct(Product product)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var p = new
                {
                    product.ProductName,
                    product.SupplierID,
                    CategoryID = product.Category.CategoryID,
                    product.QuantityPerUnit,
                    product.UnitPrice,
                    product.UnitsInStock,
                    product.ReorderLevel,
                    product.LastDateUpdated
                };

                var productId = connection.ExecuteScalar<int>(
                    "INSERT INTO Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel, LastDateUpdated)" +
                    " VALUES(@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @ReorderLevel, @LastDateUpdated);" +
                    "SELECT CAST(SCOPE_IDENTITY() AS INT); ", p);
                return productId;
            }
        }

        public static void CreateOrderDetail(OrderDetail orderDetail)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new
                {
                    OrderId = orderDetail.Order.OrderID,
                    ProductId = orderDetail.Product.ProductID,
                    UnitPrice = orderDetail.UnitPrice,
                    Quantity = orderDetail.Quantity,
                    Discount = orderDetail.Discount
                };

                connection.ExecuteScalar<int>
                    ("INSERT INTO OrderDetails (OrderId, ProductId, UnitPrice, Quantity, Discount) VALUES (@OrderId, @ProductId, @UnitPrice, @Quantity, @Discount);", parameters);
            }
        }

        public static List<Order> GetOrdersSortedByDate()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var orders = connection.Query<Order>("SELECT * FROM Orders ORDER BY OrderDate").ToList();
                return orders;
            }
        }

        public static List<Product> GetProductsSortedByMostSold()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var products = connection.Query<Product>(
                    "SELECT p.* " +
                    "FROM Products p " +
                    "LEFT JOIN OrderDetails od ON p.ProductID = od.ProductId " +
                    "GROUP BY p.ProductID, p.ProductName, p.UnitPrice, p.CategoryID, p.SupplierID, p.QuantityPerUnit, p.UnitsInStock, p.ReorderLevel, p.Discontinued, p. LastDateUpdated, p.LastUserID " +
                    "ORDER BY SUM(od.Quantity) DESC "
                    ).ToList();
                return products;
            }
        }

        public static List<Category> GetCategoriesSortedByMostSold()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var categories = connection.Query<Category>(
                    "SELECT c.* " +
                    "FROM Categories c " +
                    "LEFT JOIN Products p ON c.CategoryID = p.CategoryId " +
                    "LEFT JOIN OrderDetails od ON p.ProductID = od.ProductId " +
                    "GROUP BY c.CategoryID, c.CategoryName, c.Description, c.Picture " +
                    "ORDER BY SUM(od.Quantity) DESC ")
                    .ToList();

                return categories;
            }
        }
    }
}
