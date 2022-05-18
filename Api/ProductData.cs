using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.Extensions.Logging;

namespace Api;

public interface IProductData
{
    Task<Product> AddProduct(Product product);
    Task<bool> DeleteProduct(int id);
    Task<IEnumerable<Product>> GetProducts();
    Task<Product> UpdateProduct(Product product);
    Task<Product> GetProductById(int id);
}

public class ProductData : IProductData
{
    private readonly ILogger<ProductData> _logger;
    private readonly string _connectionString;

    public ProductData(ILogger<ProductData> logger)
    {
        _logger = logger;
        _connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
    }

    public Task<Product> AddProduct(Product product)
    {
        //product.Id = GetRandomInt();
        //products.Add(product);
        return Task.FromResult(product);
    }

    public Task<Product> UpdateProduct(Product product)
    {
        //var index = products.FindIndex(p => p.Id == product.Id);
        //products[index] = product;
        return Task.FromResult(product);
    }

    public Task<bool> DeleteProduct(int id)
    {
        //var index = products.FindIndex(p => p.Id == id);
        //products.RemoveAt(index);
        return Task.FromResult(true);
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        List<Product> products = new List<Product>();
        
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"Select * from ShoppingList";
                SqlCommand command = new SqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Quantity = (int)reader["Quantity"]

                    };

                    products.Add(product);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }

        return products.AsEnumerable();
    }

    public async Task<Product> GetProductById(int id)
    {
        Product product = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"Select * from ShoppingList where Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    product = new Product()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Quantity = (int)reader["Quantity"]

                    };
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }

        return product;
    }
}
