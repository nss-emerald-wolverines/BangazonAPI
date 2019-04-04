using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
namespace BangazonAPI.Controllers

{
    [Route("api/Customer")]

    [ApiController]

    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomerController(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Customer (FirstName, LastName) OUTPUT INSERTED.Id VALUES(@FirstName, @LastName)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", customer.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", customer.LastName));

                    int newId = (int)cmd.ExecuteScalar();
                    customer.Id = newId;
                    return CreatedAtRoute("GetCustomer", new
                    {
                        id = newId
                    }, customer);
                }
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Customer customer)
        {
            try
            {

                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Customer set
                            FirstName = @FirstName ,
                            LastName = @LastName
                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@FirstName", customer.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", customer.LastName));
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName  FROM Customer WHERE Id = @id ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Customer customer = null;
                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        };
                    }
                    reader.Close();

                    return Ok(customer);
                }
            }
        }



        [HttpGet]
        public IEnumerable<Customer> Get(string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd2 = conn.CreateCommand())
                {
                    cmd2.CommandText = @"select c.Id as CustomerId,
                                               c.FirstName,
                                               c.LastName
                                            from Customer c
                                                WHERE 1 = 1";
                    if (!string.IsNullOrWhiteSpace(q))
                    {
                        cmd2.CommandText += @" AND
                                             (c.FirstName LIKE @q OR
                                              c.LastName LIKE @q)";
                        cmd2.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                    }

                    SqlDataReader reader = cmd2.ExecuteReader();

                    Dictionary<int, Customer> customerdic = new Dictionary<int, Customer>();
                    while (reader.Read())
                    {

                        int CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                        if (!customerdic.ContainsKey(CustomerId))
                        {
                            Customer customer = new Customer
                            {
                                Id = CustomerId,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            };

                            customerdic.Add(CustomerId, customer);
                        }



                    }
                    reader.Close();
                    return customerdic.Values.ToList();
                }
            }
        }


        private bool CustomerExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName FROM Customer WHERE Id = @id ";

                    cmd.Parameters.Add(new SqlParameter("@Id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }

            }
        }
    }
}
