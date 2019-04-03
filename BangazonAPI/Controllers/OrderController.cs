using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrderController(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // CODE FOR GETTING A LIST - GET: api/Order

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT o.Id AS OrderId,
                                        o.CustomerId,
                                        o.PaymentTypeId,
                                        c.FirstName,
                                        c.LastName,
                                        pt.AcctNumber,
                                        pt.[Name] AS PaymentTypeName
                                    FROM [Order] o
                                    LEFT JOIN Customer c on o.CustomerId = c.Id
                                    LEFT JOIN PaymentType pt on o.PaymentTypeId = pt.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Order> orders = new List<Order>();

                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                            Customer = new Customer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            }

                            /* It's possible the Payment type could be null - What would that mean???
                               However, not asking for PaymentTpe link in issue ticket
                               PaymentType = new PaymentType
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                                AcctNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
                                Name = reader.GetString(reader.GetOrdinal("PaymentTypeName")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            } */
                        };

                        orders.Add(order);
                    }
                    reader.Close();

                    return Ok(orders);
                }
            }
        }


         // CODE FOR GETTING A SINGLE ORDER
        // GET: api/Order/5
        [HttpGet("{id}", Name = "GetOne")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, CustomerId, PaymentTypeId FROM [Order]
                                        WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Order order = null;

                    if (reader.Read())
                    {
                        order = new Order
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                        };
                    
                    }
                    reader.Close();

                    return Ok(order);
                }
            }
        }
                          
        
        // CODE FOR CREATING AN ORDER

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [Order] (CustomerId, PaymentTypeId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@cId, @ptId)";

                    cmd.Parameters.Add(new SqlParameter("@cId", order.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@ptId", order.PaymentTypeId));

                    int newId = (int)cmd.ExecuteScalar();
                    order.Id = newId;
                    // 
                    // Re-route user back to order they created
                    return CreatedAtRoute("GetOrder", new { id = newId }, order);
                }
            }
        }


        // CODE FOR UPDATING AN ORDER
        // public void Put(int id, [FromBody] string value)

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Order order)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Order
                                            SET CustomerId = @cId,
                                                PaymentTypeId = @ptId
                                            WHERE Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@cId", order.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@ptId", order.PaymentTypeId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

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
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool OrderExists(int id)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
