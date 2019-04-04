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

        // CODE FOR GETTING A LIST - GET: api/Order -  GET: api/Orders?q=joe&include=customer
        // public async Task<IActionResult> Get(string include, string q)
        // public IEnumerable<Order> Get(string include, string q)
        // public IActionResult Get(string q, string include)

        [HttpGet]
        public IEnumerable<order> Get(string include, string q)
        {
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        if (include == "customer")
                        {
                            cmd.CommandText = @"SELECT o.Id AS OrderId,
                                        o.CustomerId,
                                        o.PaymentTypeId,
                                        c.FirstName,
                                        c.LastName
                                    FROM [Order] o
                                    LEFT JOIN Customer c on o.CustomerId = c.Id
                                    WHERE 1 = 1";
                        }
                        else if (include == "product")
                        {
                            cmd.CommandText = @"SELECT o.Id AS OrderId,
                                        o.CustomerId,
                                        o.PaymentTypeId,
		                                op.Id as OrderProductId,
                                        p.Id AS PId,
                                        p.ProductTypeId,
                                        p.CustomerId,
                                        p.Price,
                                        p.Title,
                                        p.Description,
                                        p.Quantity
                                    FROM [Order] o
	                                LEFT JOIN OrderProduct op on o.Id = op.OrderId
	                                LEFT JOIN Product p on op.ProductId =  p.Id 
                                    WHERE 1 = 1";
                        }
                        else
                        {
                            cmd.CommandText = @"SELECT o.Id AS OrderId, o.CustomerId, o.PaymentTYpeId FROM [Order] o";
                        }

                        if (!string.IsNullOrWhiteSpace(q))
                        {
                            cmd.CommandText += @" AND 
                                             ( o.CustomerId LIKE @q OR
                                               o.PaymentTypeId LIKE @q";

                            cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                        }

                        SqlDataReader reader = cmd.ExecuteReader();

                        List<order> orders = new List<order>();

                        while (reader.Read())
                        {
                            if (include == "product")
                            {
                                order newOrderP = new order()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                                    ListofOrderProducts = new List<OrderProduct>()
                                };

                                //if (!reader.IsDBNull(reader.GetOrdinal("OrderProductId")))

                                int orderProductId = reader.GetInt32(reader.GetOrdinal("OrderProductId"));
                                if (newOrderP.ListofOrderProducts.Any(i => i.Id == orderProductId))
                                {
                                    OrderProduct orderProduct = new OrderProduct
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("OrderProductId")),
                                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        OrderId = newOrderP.Id,
                                        Product = new Product
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                            Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                            Title = reader.GetString(reader.GetOrdinal("Title")),
                                            Description = reader.GetString(reader.GetOrdinal("Description")),
                                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                                        }
                                    };
                                    orders.Add(newOrderP);
                                }
                                else if (include == "customer")
                                {
                                    order newOrderC = new order()
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
                                    };
                                    orders.Add(newOrderC);
                                }
                                else
                                {
                                    order newOrder = new order
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                                    };
                                    orders.Add(newOrder);
                                }
                            }

                            reader.Close();
                            return orders;
                            ;
                        }
                    }
                }
            }
        }


        // Get: api/Order/5?include=customer
        [HttpGet("{id}", Name = "GetOneOrder")]

        public order Get(int id, string include)
        // public async Task<IActionResult> Get([FromRoute] int id)                
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "customer")
                    {
                        cmd.CommandText = @"SELECT o.Id AS OrderId,
                                    o.CustomerId,
                                    o.PaymentTypeId,
                                    c.Id,
                                    c.FirstName,
                                    c.LastName
                                FROM [Order] o
                                LEFT JOIN Customer c on o.CustomerId = c.Id
                                WHERE 1 = 1";
                    }
                    else if (include == "product")
                    {
                        cmd.CommandText = @"SELECT o.Id AS OrderId,
                                    o.CustomerId,
                                    o.PaymentTypeId,
		                            op.Id as OrderProductId,
                                    p.Id AS PId,
                                    p.ProductTypeId,
                                    p.CustomerId,
                                    p.Price,
                                    p.Title,
                                    p.Description,
                                    p.Quantity
                                FROM [Order] o
	                            LEFT JOIN OrderProduct op on o.Id = op.OrderId
	                            LEFT JOIN Product p on op.ProductId =  p.Id 
                                WHERE 1 = 1";
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT o.Id AS OrderId, o.CustomerId, o.PaymentTYpeId FROM [Order] o";
                    }

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    order order = null;
                    while (reader.Read())
                    {
                        if (order == null)
                        {
                            if (include == "product")
                            {
                                order = new order
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                                };

                                int orderProductId = reader.GetInt32(reader.GetOrdinal("OrderProductId"));
                                if (!order.ListofOrderProducts.Any(i => i.Id == orderProductId))
                                {
                                    OrderProduct orderProduct = new OrderProduct
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("OrderProductId")),
                                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        OrderId = order.Id,
                                        Product = new Product
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                            Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                            Title = reader.GetString(reader.GetOrdinal("Title")),
                                            Description = reader.GetString(reader.GetOrdinal("Description")),
                                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))

                                        }
                                    };
                                }
                            }
                            else if (include == "customer")
                            {
                                order = new order
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
                                };
                            }
                            else
                            {
                                order = new order
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                                };
                            }
                        }
                    }
                    reader.Close();
                    return order;
                }
            }
        }
                          
        
        // CODE FOR CREATING AN ORDER

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] order order)
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
        public IActionResult Put([FromRoute] int id, [FromBody] order order)
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



        // DELETE: api/ApiWithActions/5

        // CODE FOR DELETING A COHORT

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Order WHERE Id = @id";
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

        /*    private bool OrderExists(int id)
            {
                throw new NotImplementedException();
            }   */

        private bool OrderExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, CustomerId, PaymentTypeId
                                        FROM Order
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}
