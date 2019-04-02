﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BangazonAPI.Models;

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
                                          FROM PaymentType p;";
                    
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

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
