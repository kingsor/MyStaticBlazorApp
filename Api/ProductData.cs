using Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
        try
        {
            using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))
            {
                connection.Open();
                if (!String.IsNullOrEmpty(product.Name) && !string.IsNullOrEmpty(product.Description))
                {
                    var query = $"INSERT INTO ShoppingList (Name,Description,Quantity) VALUES('{product.Name}', '{product.Description}' , '{product.Quantity}')";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
        }
        return Task.FromResult(product);
    }

    public Task<Product> UpdateProduct(Product product)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))
            {
                connection.Open();
                var query = @"Update ShoppingList Set Name = @Name, Description = @Description , Quantity = @Quantity, UpdatedAt = @UpdatedAt Where Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
                command.Parameters.AddWithValue("@Id", product.Id);
                int rows = command.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
        }
        return Task.FromResult(product);
    }

    public Task<bool> DeleteProduct(int id)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))
            {
                connection.Open();
                //var query = @"Delete from ShoppingList Where Id = @Id";
                var query = @"Update ShoppingList Set DeletedAt = GETDATE() Where Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Task.FromResult(false);
        }

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
                var query = @"Select * from ShoppingList Where DeletedAt IS NULL ";
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
