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

        public Customer Customer { get; private set; }

        // ***** CODE FOR GETTING A LIST - GET: api/Order -  GET: api/Orders?completed=true&include=customer
        // url: http://localhost:5000/order?include=product or http://localhost:5000/order?include=customer
        // url: http://localhost:5000/order?completed=true or http://localhost:5000/order?completed=false

        [HttpGet]
        public async Task<IActionResult> Get(string include, string completed)        
        {            
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {

                    // The "include" if statements reference what additional parameters need to be to SELECTED in the object
                    // The "completed" if statements pertain to what query string(s) need to be added to the WHERE statement

                    // If string is not all on one line - auto makes it multiple strings added together - "" + ""
                    cmd.CommandText = "SELECT o.Id AS OrderId, o.CustomerId, o.PaymentTypeId, pt.[Name] AS PaymentType";

                    if (include == "customer")
                    {
                        cmd.CommandText += ", c.Id AS cId, c.FirstName, c.LastName";
                    }

                    if (include == "product")
                    {
                        cmd.CommandText += ", p.Id AS ProductId, p.ProductTypeId, p.Title, p.[Description], p.Price, p.Quantity";
                    }

                    cmd.CommandText += @" FROM [Order] o
                                        LEFT JOIN Customer c ON c.id = o.CustomerId
                                        LEFT JOIN PaymentType pt ON pt.Id = o.PaymentTypeId
                                        LEFT JOIN OrderProduct op ON op.Orderid = o.Id
                                        LEFT JOIN Product p ON p.Id = op.ProductId
                                        WHERE 1=1";

                    if (completed == "false")
                    {
                        cmd.CommandText += " And PaymentTypeId IS NULL";
                    }
                    if (completed == "true")
                    {
                        cmd.CommandText += " And PaymentTypeId IS NOT NULL";
                    }


                    /* In Andy's StudentExerciseAPI example he did seperate SELECT statements for 'getting all students' and 
                        'getting all students with exercises'. Then he added as many query statements as needed to the WHERE

                    if (!string.IsNullOrWhiteSpace(q))
                    {
                        cmd.CommandText += @" AND 
                                            ( o.CustomerId LIKE @q OR
                                            o.PaymentTypeId LIKE @q";

                        cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                    } */


                    SqlDataReader reader = cmd.ExecuteReader();

                    // Create a DICTIONARY for adding Order objects
                    Dictionary<int, Order> dictionaryO = new Dictionary<int, Order>();

                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));

                        // Does the Dictionary already have the key of the int orderId?
                        if (!dictionaryO.ContainsKey(orderId))
                        {                           
                            // Check to see if PaymentType is NOT NULL (order complete) and if so, add the PaymentType to dictionaryO object
                            if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                            {
                                Order newOrder = new Order
                                {
                                    OrderId = orderId,
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                                };
                                dictionaryO.Add(orderId, newOrder);
                            }
                            else
                            // Otherwise, don't include the PaymentType in the object
                            {
                                Order newOrder = new Order
                                {
                                    OrderId = orderId,
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                };
                                dictionaryO.Add(orderId, newOrder);

                            }
                        
                           if (include == "customer")
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("CustomerId")))
                                {                                    
                                    Order currentOrder = dictionaryO[orderId];
                                    
                                    currentOrder.Customer = new Customer
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                    };                                    
                                }
                            }     

                            if (include == "product")
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                                {
                                    // Add the product LIST to the current entry in Dictionary
                                    Order currentOrder = dictionaryO[orderId];
                                    currentOrder.ListofProducts.Add(
                                    new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                        Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                                    }
                                    );
                                }
                            }
                        }
                    }                
                    reader.Close();
                    return Ok(dictionaryO.Values);
                }
            }
        }


        // Get: api/Order/5?include=customer
        // public async Task<IActionResult> Get([FromRoute] int id)
        //arguments: id specifies which order to get

        [HttpGet("{id}", Name = "GetOneOrder")]

        public async Task<IActionResult> Get([FromRoute] int id, string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT o.Id AS OrderId, o.CustomerId, o.PaymentTypeId, pt.[Name] AS PaymentType";

                    if (include == "customer")
                    {
                        cmd.CommandText += ", c.Id AS CustomerId, c.FirstName, c.LastName";
                    }

                    if (include == "product")
                    {
                        cmd.CommandText += ", p.Id AS ProductId, p.ProductTypeId, p.Title, p.[Description], p.Price, p.Quantity";
                    }

                    cmd.CommandText += @" FROM [Order] o
                                        INNER JOIN Customer c ON c.id = o.CustomerId
                                        INNER JOIN PaymentType pt ON pt.Id = o.PaymentTypeId
                                        INNER JOIN OrderProduct op ON op.Orderid = o.Id
                                        INNER JOIN Product p ON p.Id = op.ProductId
                                        WHERE o.Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    Order order = null;

                    while (reader.Read())
                    {
                        if (order == null)
                        {
                            order = new Order
                            {
                                OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                            };
                        }

                        if (include == "product")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                            {
                                order.ListofProducts.Add(
                                    new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                        Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                                    }
                                    );
                            }
                        }

                        if (include == "customer")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("CustomerId")))
                            {
                                int customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));

                                order.Customer = new Customer
                                {
                                    Id = customerId,
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };                                
                            }
                        }
                    }

                    reader.Close();
                    return Ok(order);
                }
            }
        }
        
                          
        
        // CODE FOR CREATING AN ORDER

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order newOrder)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                  // Including the PaymentTypeId only works if the PaymentType is not null
                  // otherwise it crashes - so needs some kind of exception???
                  /*  cmd.CommandText = @"INSERT INTO [Order] (CustomerId, PaymentTypeId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@customerId, @paymentTypeId)";

                    cmd.Parameters.Add(new SqlParameter("@customerId", newOrder.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@paymentTypeId", newOrder.PaymentTypeId)); */

                    cmd.CommandText = @"INSERT INTO [Order] (CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@customerId)";

                    cmd.Parameters.Add(new SqlParameter("@customerId", newOrder.CustomerId));
                    
                    // PaymentTpe should be null when order added? When completing it update PaymentType???

                    int newId = (int)cmd.ExecuteScalar();
                    newOrder.OrderId = newId;
                    
                    // Re-route user back to order they created using GetOneOrder
                    return CreatedAtRoute("GetOneOrder", new { id = newId }, newOrder);
                }
            }
        }


        // CODE FOR UPDATING AN ORDER
        // public void Put(int id, [FromBody] string value)
        // public IActionResult Put([FromRoute] int id, [FromBody] Order order)

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
                        cmd.CommandText = @"UPDATE [Order]
                                            SET CustomerId = @customerId,
                                                PaymentTypeId = @paymentTypeId
                                            WHERE Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@customerId", order.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@paymentTypeId", order.PaymentTypeId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status200OK);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ObjectExists(id))
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
        // CODE FOR DELETING A ORDER

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM OrderProduct WHERE Id = @id
                                            DELETE FROM [Order] WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status200OK);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ObjectExists(id))
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

        private bool ObjectExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, CustomerId, PaymentTypeId
                                        FROM [Order]
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }        
    }
}
