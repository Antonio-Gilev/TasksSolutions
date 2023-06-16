using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductsListing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsListing
{
    public class DataAccess
    {
        private readonly DatabaseHelper _databaseHelper;

        public DataAccess(IConfiguration configuration)
        {
            _databaseHelper = new DatabaseHelper(configuration);
        }

        public List<Product> GetAllProducts()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();

                string query = "SELECT p.*, c.* " +
                               "FROM Products p " +
                               "INNER JOIN Categories c ON p.CategoryID = c.CategoryID";

                List<Product> products = connection.Query<Product, Category, Product>(
                    query,
                    (product, category) =>
                    {
                        product.Category = category;
                        return product;
                    },
                    splitOn: "CategoryID"
                ).ToList();

                connection.Close();


                return products;
            }
        }

        public List<Category> GetAllCategories()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                return connection.Query<Category>("SELECT * FROM Categories").ToList();
            }
        }

        public bool CreateProduct(Product product)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                var query = @"
                         INSERT INTO Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel) " +
                    "VALUES(@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @ReorderLevel); " +
                    "SELECT CAST(SCOPE_IDENTITY() AS INT); ";

                var parameters = new
                {
                    product.ProductName,
                    SupplierID = product.SupplierID = 1,
                    CategoryID = product.Category.CategoryID,
                    product.QuantityPerUnit,
                    product.UnitPrice,
                    product.UnitsInStock,
                    product.ReorderLevel,
                    LastDateUpdated = DateTime.Now
                };

                try
                {
                    connection.ExecuteScalar<int>(query, parameters);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public bool DeleteProduct(int id)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                var query = "DELETE FROM OrderDetails WHERE ProductID = @ProductId; " +
                    "DELETE FROM Products WHERE ProductID = @ProductId;";
                var parameters = new { ProductId = id };

                try
                {
                    connection.Execute(query, parameters);
                    
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                
            }
        }
    }
}
