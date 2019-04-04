using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;

namespace BangazonAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PaymentTypeController : ControllerBase
  {
    // Connection established
    public SqlConnection Connection
    {
      get
      {
        string connectionSTring = "Server=localhost\\SQLExpress;Database=BangazonDB;Integrated Security=true";
        return new SqlConnection(connectionSTring);
      }
    }

    /***********************
     GET ALL PAYMENT TYPES
    ***********************/
    // GET api/PaymentType
    [HttpGet]
    public IEnumerable<PaymentType> Get()
    {
      using (SqlConnection conn = Connection)
      {

        conn.Open();
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.CommandText = @"SELECT p.Id, p.AcctNumber, p.Name, p.CustomerId
                                          FROM PaymentType p";

          SqlDataReader reader = cmd.ExecuteReader();

          List<PaymentType> paymentTypes = new List<PaymentType>();

          while (reader.Read())
          {
            PaymentType paymentType = new PaymentType
            {
              Id = reader.GetInt32(reader.GetOrdinal("Id")),
              AcctNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
              Name = reader.GetString(reader.GetOrdinal("Name")),
              CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
            };
            paymentTypes.Add(paymentType);
          }
          reader.Close();
          return paymentTypes;
        }
      }
    }
    /***********************
     GET api/PaymentType/{id}
    ***********************/
    [HttpGet("{id}", Name = "GetPaymentType")]
    public PaymentType Get(int id)
    {
      using (SqlConnection conn = Connection)
      {
        conn.Open();
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.CommandText = @"SELECT p.Id, p.AcctNumber, p.Name, p.CustomerId
                                          FROM PaymentType p
                                         WHERE p.id = @id";
          cmd.Parameters.Add(new SqlParameter("@id", id));
          SqlDataReader reader = cmd.ExecuteReader();

          PaymentType paymentType = null;

          if (reader.Read())
          {
            paymentType = new PaymentType
            {
              Id = reader.GetInt32(reader.GetOrdinal("Id")),
              AcctNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
              Name = reader.GetString(reader.GetOrdinal("Name")),
              CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
            };
          }
          reader.Close();
          return paymentType;
        }
      }
    }
    /***********************
     POST api/PaymentType/{id}
    ***********************/
    [HttpPost]
    public ActionResult Post([FromBody] PaymentType newPaymentType)
    {
      using (SqlConnection conn = Connection)
      {
        conn.Open();
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO PaymentType (AcctNumber, Name, CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@acctNumber, @name, @customerId)";
          cmd.Parameters.Add(new SqlParameter("@acctNumber", newPaymentType.AcctNumber));
          cmd.Parameters.Add(new SqlParameter("@name", newPaymentType.Name));
          cmd.Parameters.Add(new SqlParameter("@customerId", newPaymentType.CustomerId));

          int newId = (int)cmd.ExecuteScalar();
          newPaymentType.Id = newId;
          return CreatedAtRoute("GetPaymentType", new { id = newId }, newPaymentType);
        }
      }
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PaymentType paymentType)
    {
      try
      {
        using (SqlConnection conn = Connection)
        {
          conn.Open();
          using (SqlCommand cmd = conn.CreateCommand())
          {
            cmd.CommandText = @"UPDATE PaymentType
                                                   SET AcctNumber = @acctNumber,
                                                       Name = @name,
                                                       CustomerId = @customerId
                                                 WHERE Id = @id";
            cmd.Parameters.Add(new SqlParameter("@acctNumber", paymentType.AcctNumber));
            cmd.Parameters.Add(new SqlParameter("@name", paymentType.Name));
            cmd.Parameters.Add(new SqlParameter("@customerId", paymentType.CustomerId));
            cmd.Parameters.Add(new SqlParameter("@id", paymentType.Id));

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
        if (!PaymentTypeExists(id))
        {
          return NotFound();
        }

        throw;

      }
    }
    private bool PaymentTypeExists(int id)
    {

      using (SqlConnection conn = Connection)
      {

        conn.Open();
        using (SqlCommand cmd = conn.CreateCommand())
        {

          cmd.CommandText = $@"SELECT Id, AcctName, Name, CustomerId 
                                               FROM PaymentType 
                                              WHERE Id = @id";

          cmd.Parameters.Add(new SqlParameter("@id", id));

          SqlDataReader reader = cmd.ExecuteReader();
          return reader.Read();
        }
      }
    }

    // DELETE api/values/5              
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
            cmd.CommandText = $@"DELETE FROM PaymentType 
                                  WHERE Id = @id";

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
        if (!PaymentTypeExists(id))
        {
          return NotFound();
        }
        throw;
      }
    }
  }
}


