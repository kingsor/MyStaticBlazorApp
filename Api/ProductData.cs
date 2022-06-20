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
    Task<ProductResponse> AddProduct(Product product);
    Task<bool> DeleteProduct(int id);
    Task<ProductsResponse> GetProducts();
    Task<ProductResponse> UpdateProduct(Product product);
    Task<ProductResponse> GetProductById(int id);
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

    public Task<ProductResponse> AddProduct(Product product)
    {
        var response = new ProductResponse()
        {
            ErrorMessage = String.Empty,
            Product = product
        };

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
            response.ErrorMessage = e.Message;
        }

        return Task.FromResult(response);
    }

    public Task<ProductResponse> UpdateProduct(Product product)
    {
        var response = new ProductResponse()
        {
            ErrorMessage = String.Empty,
            Product = null
        };

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

            response.Product = product;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            response.ErrorMessage = e.Message;
        }

        return Task.FromResult(response);
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

    public async Task<ProductsResponse> GetProducts()
    {
        var response = new ProductsResponse()
        {
            ErrorMessage = string.Empty
        };

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
            response.ErrorMessage = ex.Message;
        }

        response.Products = products.AsEnumerable();

        return response;
    }

    public async Task<ProductResponse> GetProductById(int id)
    {
        var response = new ProductResponse()
        {
            ErrorMessage = String.Empty,
            Product = null
        };

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
                    response.Product = new Product()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Quantity = (int)reader["Quantity"]

                    };
                }

                if(response.Product == null)
                {
                    response.ErrorMessage = $"Product with id={id} was not found";
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
    
}
